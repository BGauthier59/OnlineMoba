using GameStates;
using UnityEngine;

namespace Entities.Capacities
{
    public class PassivePerseverance : PassiveCapacity
    {
        private double timeSinceLastAttack;
        private double timeSinceLastHeal;
        private PassivePerseveranceSO passiveCapacitySo;
        private IActiveLifeable activeLifeable;
        
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByIndex(indexOfSo);
        }

        protected override void OnAddedEffects(Entity target)
        {
            passiveCapacitySo = (PassivePerseveranceSO)AssociatedPassiveCapacitySO();

            activeLifeable = entity.GetComponent<IActiveLifeable>();
            Debug.Log("addedpassive" + entity.gameObject.name);
            activeLifeable.OnDecreaseCurrentHp += ResetTimeSinceLastAttack;
            GameStateMachine.Instance.OnTick += IncreasePerTick;
        }

        protected override void OnAddedFeedbackEffects(Entity target)
        {
            
        }

        protected override void OnRemovedEffects(Entity target)
        {
            Debug.LogWarning("RemovedEffect");
            activeLifeable.OnDecreaseCurrentHp -= ResetTimeSinceLastAttack;
            GameStateMachine.Instance.OnTick -= IncreasePerTick;
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
            
        }

        private void ActiveHealEffect()
        {
            float maxHP = activeLifeable.GetMaxHp();
            float modAmount = maxHP * passiveCapacitySo.percentage;
            activeLifeable.IncreaseCurrentHpRPC(modAmount);
 
          //  PoolLocalManager.Instance.PoolInstantiate(((PassivePerseveranceSO)AssociatedPassiveCapacitySO()).healEffectPrefab, entity.transform.position, Quaternion.identity,
            //    entity.transform);
        }

        private void IncreasePerTick()
        {

            timeSinceLastAttack += GameStateMachine.Instance.tickRate/60;
            timeSinceLastHeal += GameStateMachine.Instance.tickRate/60;

            if (timeSinceLastAttack > passiveCapacitySo.timeBeforeHeal)
            {
                if (timeSinceLastHeal >= 5)
                {
              
                    ActiveHealEffect();
                    timeSinceLastHeal = 0;
                }
            }
        }

        private void ResetTimeSinceLastAttack(float f)
        {
   
            timeSinceLastAttack = 0;
        }
    }

}
