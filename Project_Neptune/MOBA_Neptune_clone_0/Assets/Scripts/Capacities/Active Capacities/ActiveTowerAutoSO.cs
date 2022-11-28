using System;
using Entities.Capacities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Auto Attack Template", fileName = "AI Auto Attack for ")]
public class ActiveTowerAutoSO : ActiveCapacitySO
{
    public float damage;
    
    public override Type AssociatedType()
    {
        return typeof(ActiveTowerAuto);
    }
}
