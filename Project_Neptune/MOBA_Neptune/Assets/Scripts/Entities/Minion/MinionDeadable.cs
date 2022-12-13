using System;
using Entities.FogOfWar;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion
{
    public partial class MinionBehaviour : IDeadable
    {
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
            PoolNetworkManager.Instance.PoolRequeue(this);
            if(FogOfWarManager.Instance != null) FogOfWarManager.Instance.RemoveFOWViewable(this);
            gameObject.SetActive(false);
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