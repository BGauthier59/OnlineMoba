using Entities.FogOfWar;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : IDeadable
    {
        public bool isAlive;
        public bool canDie;

        // TODO: Delete when TickManager is implemented
        public float respawnDuration = 3;
        private double respawnTimer;
        public GameObject dieCanvas;


        public bool IsAlive()
        {
            return isAlive;
        }

        public bool CanDie()
        {
            return canDie;
        }

        public void RequestSetCanDie(bool value)
        {
            photonView.RPC("SetCanDieRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetCanDieRPC(bool value)
        {
            canDie = value;
            OnSetCanDieFeedback?.Invoke(value);
        }

        [PunRPC]
        public void SetCanDieRPC(bool value)
        {
            canDie = value;
            OnSetCanDie?.Invoke(value);
            photonView.RPC("SyncSetCanDieRPC", RpcTarget.All, canDie);
        }

        public event GlobalDelegates.BoolDelegate OnSetCanDie;
        public event GlobalDelegates.BoolDelegate OnSetCanDieFeedback;

        public void RequestDie()
        {
            photonView.RPC("DieRPC", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void SyncDieRPC()
        {
            if (photonView.IsMine)
            {
                InputManager.PlayerMap.Movement.Disable();
                InputManager.PlayerMap.Attack.Disable();
                InputManager.PlayerMap.Capacity.Disable();
                dieCanvas.SetActive(true);
                //viewRange = 0.1f;
            }

            OnDieFeedback?.Invoke();
        }

        [PunRPC]
        public void DieRPC()
        {
            if (!canDie) return;
            
            SetCanMoveRPC(false);
            isAlive = false;
            SetCanDieRPC(false);

            if (lastEntityWhoAttackedMeIndex != 0)
            {
                var entity = EntityCollectionManager.GetEntityByIndex(lastEntityWhoAttackedMeIndex);
                if (entity.GetComponent<Champion>())
                    ChampionRequestIncreaseScore(Mathf.Abs(currentPointCarried / 2), entity);
            }

            ChampionRequestRemoveScore(GetComponent<Entity>());
            
            // Tower disable
            EntityCollectionManager.GetEntityByIndex(towerLinkedIndex).GetComponent<Tower>().RequestDie();
            
            OnDie?.Invoke();
            GameStateMachine.Instance.OnTick += Revive;
            photonView.RPC("SyncDieRPC", RpcTarget.All);
        }

        public event GlobalDelegates.NoParameterDelegate OnDie;
        public event GlobalDelegates.NoParameterDelegate OnDieFeedback;

        public void RequestRevive()
        {
            photonView.RPC("ReviveRPC", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void SyncReviveRPC()
        {
            transform.position = respawnPos;
            if (photonView.IsMine)
            {
                InputManager.PlayerMap.Movement.Enable();
                InputManager.PlayerMap.Attack.Enable();
                InputManager.PlayerMap.Capacity.Enable();
                dieCanvas.SetActive(false);
                viewRange = baseViewRange;
            }

            photonView.RPC("SetReviveShaderRPC", RpcTarget.All);

            OnReviveFeedback?.Invoke();
        }

        [PunRPC]
        public void ReviveRPC()
        {
            isAlive = true;
            SetCanMoveRPC(true);
            SetCanDieRPC(true);
            SetCurrentHpRPC(maxHp);
            SetCurrentResourceRPC(maxResource);
            OnRevive?.Invoke();
            photonView.RPC("SyncReviveRPC", RpcTarget.All);
        }

        private void Revive()
        {
            respawnTimer += 1.0 / GameStateMachine.Instance.tickRate;
            photonView.RPC("SetDieShaderRPC", RpcTarget.All, 
                (float)(respawnTimer / respawnDuration));

            if (!(respawnTimer >= respawnDuration)) return;
            GameStateMachine.Instance.OnTick -= Revive;
            respawnTimer = 0f;
            RequestRevive();
        }
        
        [PunRPC]
        private void SetDieShaderRPC(float ratio)
        {
            foreach (var rd in meshes)
            {
                rd.material.SetFloat("_DieColor", Mathf.Lerp(1, 0, ratio));
                rd.material.SetFloat("_DieDissolve", Mathf.Lerp(-2, 2, ratio));
            }
        }

        [PunRPC]
        private void SetReviveShaderRPC()
        {
            foreach (var rd in meshes)
            {
                rd.material.SetFloat("_DieColor", 1);
                rd.material.SetFloat("_DieDissolve", -2);
            }
        }

        public event GlobalDelegates.NoParameterDelegate OnRevive;
        public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
    }
}