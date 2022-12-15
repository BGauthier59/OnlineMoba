using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public abstract class NewPassiveCapacity : MonoBehaviour
{
    public abstract void OnAddEffect(Entity giver = null, Vector3 position = default);

    public abstract void OnUpdateEffect();

    public abstract void OnRemoveEffect();
}
