using System;
using Entities;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public abstract class NewActiveCapacity : MonoBehaviourPun
{
    public Entity caster;
    protected Champion championCaster;

    public bool canCastCapacity;
    public double cooldownDuration;
    protected double cooldownTimer;
    protected bool previewActivate = false;
    [SerializeField] protected Transform previewObject;

    private void Start()
    {
        caster = GetComponent<Entity>();
        championCaster = (Champion) caster;
    }

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

    public abstract void RequestSetPreview(bool active);

    public abstract void Update();

    public abstract void UpdatePreview();
}
