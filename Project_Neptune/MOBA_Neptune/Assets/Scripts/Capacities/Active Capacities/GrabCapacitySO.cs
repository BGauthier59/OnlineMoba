using System;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Grab Capacity", fileName = "Grab Capacity SO")]
    public class GrabCapacitySO : ActiveCapacitySO
    {
        public override Type AssociatedType()
        {
            return typeof(GrabCapacity);
        }
    }
}
