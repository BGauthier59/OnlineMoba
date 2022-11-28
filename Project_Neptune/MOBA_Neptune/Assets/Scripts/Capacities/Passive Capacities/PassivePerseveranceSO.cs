using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Perseverance", fileName = "Perseverance")]
    public class PassivePerseveranceSO : PassiveCapacitySO
    {
        public override Type AssociatedType()
        {
            return typeof(PassivePerseverance);
        }
        
        [Range(0, 1)] public float percentage;
        public float timeBeforeHeal;
        public GameObject healEffectPrefab;
    }
}


