using System;
using Entities.FogOfWar;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : IDeadable
    {
        public float respawnDuration = 3;
        private double respawnTimer;

        public bool IsAlive()
        {
            throw new NotImplementedException();
        }

        public bool CanDie()
        {
            throw new NotImplementedException();
        }

        public void RequestSetCanDie(bool value)
        {
            throw new NotImplementedException();
        }

        public void SyncSetCanDieRPC(bool value)
        {
            throw new NotImplementedException();
        }

        public void SetCanDieRPC(bool value)
        {
            throw new NotImplementedException();
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
            OnDieFeedback?.Invoke();
            PoolNetworkManager.Instance.PoolRequeue(this);
            if (FogOfWarManager.Instance != null) FogOfWarManager.Instance.RemoveFOWViewable(this);
            GameStateMachine.Instance.OnTick += Die;
        }

        [PunRPC]
        public void DieRPC()
        {
            photonView.RPC("SyncDieRPC", RpcTarget.All);

            if (lastEntityWhoAttackedMeIndex != 0)
            {
                var lastEntity = EntityCollectionManager.GetEntityByIndex(lastEntityWhoAttackedMeIndex);
                var entityChamp = lastEntity.GetComponent<Champion.Champion>();
                if (entityChamp) entityChamp.ChampionRequestIncreaseScore(this.currentPointCarried, lastEntity);
            }
        }

        private void Die()
        {
            respawnTimer += 1.0 / GameStateMachine.Instance.tickRate;
            photonView.RPC("SetDieShaderRPC", RpcTarget.All,
                (float) (respawnTimer / respawnDuration));

            if (!(respawnTimer >= respawnDuration)) return;
            photonView.RPC("SetReviveShaderRPC", RpcTarget.All);
            GameStateMachine.Instance.OnTick -= Die;
            respawnTimer = 0f;
        }

        [PunRPC]
        private void SetDieShaderRPC(float ratio)
        {
            foreach (var rd in meshes)
            {
                rd.material.SetFloat("_DieColor", Mathf.Lerp(0.7f, 0, ratio*2));
                rd.material.SetFloat("_DieDissolve", Mathf.Lerp(-1, 2, ratio*2));
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
            gameObject.SetActive(false);
        }

        public event GlobalDelegates.NoParameterDelegate OnDie;
        public event GlobalDelegates.NoParameterDelegate OnDieFeedback;

        public void RequestRevive()
        {
            throw new NotImplementedException();
        }

        public void SyncReviveRPC()
        {
            throw new NotImplementedException();
        }

        public void ReviveRPC()
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.NoParameterDelegate OnRevive;
        public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
    }
}