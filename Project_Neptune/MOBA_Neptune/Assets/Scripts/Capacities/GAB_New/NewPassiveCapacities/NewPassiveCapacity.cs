using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Photon.Pun;
using UnityEngine;

public abstract class NewPassiveCapacity : MonoBehaviourPun
{
    public bool isActive;

    public virtual void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        isActive = true;
    }

    public abstract void OnUpdateEffect();

    public virtual void OnRemoveEffect()
    {
        isActive = false;
    }
}
