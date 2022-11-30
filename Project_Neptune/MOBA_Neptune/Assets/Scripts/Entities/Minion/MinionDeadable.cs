using System;
using Entities.FogOfWar;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion
{
    public partial class MinionBehaviour : IDeadable
    {
        public float GetMaxHp()
        {
            throw new NotImplementedException();
        }

        public float GetCurrentHp()
        {
            throw new NotImplementedException();
        }

        public float GetCurrentHpPercent()
        {
            throw new NotImplementedException();
        }

        public void RequestSetMaxHp(float value)
        {
            throw new NotImplementedException();
        }

        public void SyncSetMaxHpRPC(float value)
        {
            throw new NotImplementedException();
        }

        public void SetMaxHpRPC(float value)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnSetMaxHp;
        public event GlobalDelegates.FloatDelegate OnSetMaxHpFeedback;

        public void RequestIncreaseMaxHp(float amount)
        {
            throw new NotImplementedException();
        }

        public void SyncIncreaseMaxHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public void IncreaseMaxHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseMaxHp;
        public event GlobalDelegates.FloatDelegate OnIncreaseMaxHpFeedback;

        public void RequestDecreaseMaxHp(float amount)
        {
            throw new NotImplementedException();
        }

        public void SyncDecreaseMaxHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public void DecreaseMaxHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseMaxHp;
        public event GlobalDelegates.FloatDelegate OnDecreaseMaxHpFeedback;

        public void RequestSetCurrentHp(float value)
        {
            throw new NotImplementedException();
        }

        public void SyncSetCurrentHpRPC(float value)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentHpRPC(float value)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentHp;
        public event GlobalDelegates.FloatDelegate OnSetCurrentHpFeedback;

        public void RequestSetCurrentHpPercent(float value)
        {
            throw new NotImplementedException();
        }

        public void SyncSetCurrentHpPercentRPC(float value)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentHpPercentRPC(float value)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercent;
        public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercentFeedback;

        public void RequestIncreaseCurrentHp(float amount)
        {
            throw new NotImplementedException();
        }

        public void SyncIncreaseCurrentHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public void IncreaseCurrentHpRPC(float amount)
        {
            throw new NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHp;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHpFeedback;


        public void RequestDecreaseCurrentHp(float amount)
        {
            photonView.RPC("DecreaseCurrentHpRPC", RpcTarget.MasterClient, amount);
        }

        [PunRPC]
        public void SyncDecreaseCurrentHpRPC(float amount)
        {
            currentHealth = amount;
        }

        [PunRPC]
        public void DecreaseCurrentHpRPC(float amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;

            photonView.RPC("SyncDecreaseCurrentHpRPC", RpcTarget.All, currentHealth);

            if (currentHealth <= 0)
            {
                RequestDie();
            }
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHp;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHpFeedback;

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
            FogOfWarManager.Instance.RemoveFOWViewable(this);
            gameObject.SetActive(false);
        }

        [PunRPC]
        public void DieRPC()
        {
            photonView.RPC("SyncDieRPC", RpcTarget.All);
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