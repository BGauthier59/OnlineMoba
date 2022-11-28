using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Zappie", fileName = "new Zappie")]
    public class ActiveZappieSO : ActiveCapacitySO
    {
        public GameObject projectile;
        public float damageAmount;
        public float speed;
    
        public override Type AssociatedType()
        {
            return typeof(ActiveZappie);
        }
    }
}


