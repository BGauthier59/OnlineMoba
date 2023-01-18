using System;
using Photon.Pun;

namespace Entities.Minion.MinionJungle
{
    public partial class MinionJungle : IDamageable
    {
       public float GetMaxHp()
       {
           return maxHealth;
       }

        public float GetCurrentHp()
        {
            return currentHealth;
        }

        public float GetCurrentHpPercent()
        {
            return currentHealth / maxHealth * 100f;
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
        }

        public void SyncDecreaseMaxHpRPC(float amount)
        {
        }

        public void DecreaseMaxHpRPC(float amount)
        {
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseMaxHp;
        public event GlobalDelegates.FloatDelegate OnDecreaseMaxHpFeedback;

        public void RequestSetCurrentHp(float value)
        {
        }

        public void SyncSetCurrentHpRPC(float value)
        {
        }

        public void SetCurrentHpRPC(float value)
        {
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentHp;
        public event GlobalDelegates.FloatDelegate OnSetCurrentHpFeedback;

        public void RequestSetCurrentHpPercent(float value)
        {
        }

        public void SyncSetCurrentHpPercentRPC(float value)
        {
        }

        public void SetCurrentHpPercentRPC(float value)
        {
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercent;
        public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercentFeedback;

        public void RequestIncreaseCurrentHp(float amount)
        {
        }

        public void SyncIncreaseCurrentHpRPC(float amount)
        {
        }

        public void IncreaseCurrentHpRPC(float amount)
        {
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHp;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHpFeedback;


        public void RequestDecreaseCurrentHp(float amount, Entity entityWhoAttackedMinion)
        {
            photonView.RPC("DecreaseCurrentHpRPC", RpcTarget.MasterClient, amount, entityWhoAttackedMinion.entityIndex);
        }

        [PunRPC]
        public void SyncDecreaseCurrentHpRPC(float amount, int entityWhoAttackedMeIndex)
        {
            currentHealth = amount;
            lastEntityWhoAttackedMeIndex = entityWhoAttackedMeIndex;
            currentAttackTarget = EntityCollectionManager.GetEntityByIndex(lastEntityWhoAttackedMeIndex);
            currentState = MinionState.Attacking;
            OnDecreaseCurrentHpFeedback?.Invoke(amount);
        }

        [PunRPC]
        public void DecreaseCurrentHpRPC(float amount, int entityWhoAttackedMeIndex)
        {
            currentHealth -= amount;
            lastEntityWhoAttackedMeIndex = entityWhoAttackedMeIndex;
            
            if (currentHealth < 0) currentHealth = 0;
            
            photonView.RPC("SyncDecreaseCurrentHpRPC", RpcTarget.All, currentHealth, lastEntityWhoAttackedMeIndex);
            if (currentHealth <= 0) DieRPC();
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHp;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHpFeedback;
    }
}
