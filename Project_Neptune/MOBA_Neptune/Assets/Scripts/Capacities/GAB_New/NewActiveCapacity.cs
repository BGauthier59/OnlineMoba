using Entities;
using Photon.Pun;
using UnityEngine;

public abstract class NewActiveCapacity : MonoBehaviourPun
{
    public Entity caster;
    
    public bool canCastCapacity;
    public double cooldownDuration;
    protected double cooldownTimer;
    
    public abstract void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions);

    public abstract bool TryCast();
    
    protected Vector3 GetCasterPos()
    {
        var casterPos = caster.transform.position;
        casterPos.y = 1;
        return casterPos;
    }

    protected abstract void StartCooldown();

    protected abstract void TimerCooldown();
}
