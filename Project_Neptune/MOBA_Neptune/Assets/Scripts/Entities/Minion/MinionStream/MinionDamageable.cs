using System;
using Photon.Pun;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : IDamageable
    {
        public float maxHp;
        public float currentHp;
        
        public float GetMaxHp()
        {
            return maxHp;
        }

        public float GetCurrentHp()
        {
            return currentHp;
        }

        public float GetCurrentHpPercent()
        {
            return currentHp / maxHp * 100f;
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

        public S_GetHurtOnSimpleMesh hurtOnSkinnedMeshVfx;
        [PunRPC]
        public void SyncDecreaseCurrentHpRPC(float amount, int entityWhoAttackedMeIndex)
        {
            currentHp = amount;
            lastEntityWhoAttackedMeIndex = entityWhoAttackedMeIndex;
            OnDecreaseCurrentHpFeedback?.Invoke(amount);
            //hurtOnSkinnedMeshVfx.PlayFeedback();
        }

        [PunRPC]
        public void DecreaseCurrentHpRPC(float amount, int entityWhoAttackedMeIndex)
        {
            currentHp -= amount;
            lastEntityWhoAttackedMeIndex = entityWhoAttackedMeIndex;
            
            if (currentHp < 0) currentHp = 0;
            
            photonView.RPC("SyncDecreaseCurrentHpRPC", RpcTarget.All, currentHp, lastEntityWhoAttackedMeIndex);
            if (currentHp <= 0) DieRPC();
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHp;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHpFeedback;
    }
}
