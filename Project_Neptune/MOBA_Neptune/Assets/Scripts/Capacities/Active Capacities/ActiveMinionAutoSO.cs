using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Capacities;
using UnityEngine;

[CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Auto Attack Minion", fileName = "AI Auto Attack for Minion")]
public class ActiveMinionAutoSO : ActiveCapacitySO
{
    public override Type AssociatedType()
    {
        return typeof(ActiveMinionAuto);
    }
}
