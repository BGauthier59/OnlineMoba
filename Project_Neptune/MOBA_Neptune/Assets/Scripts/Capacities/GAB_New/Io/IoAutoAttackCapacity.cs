using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class IoAutoAttackCapacity : NewActiveCapacity
{
    public float skillshotMaxDistance;
    public LayerMask targetableLayer;

    public double delayDuration;
    private double delayTimer;

    [SerializeField] private float radius;
    [SerializeField] private float damage;
    [SerializeField] private int maxCount;
    private int count;
    private bool canShootNewOne = true;

    private Vector3 direction;
    private Vector3 casterInitPos;

    private Vector3 hitPoint;
    private Champion champion;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastIoAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC]
    public void CastIoAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        // Set data
        photonView.RPC("SyncDataIoAutoAttackCapacityRPC", RpcTarget.All, targetedPositions[0]);

        if (TryCast())
        {
            if (count == maxCount)
            {
                StartCooldown();
                GameStateMachine.Instance.OnTick += TimerCooldown;
            }
        }
    }

    [PunRPC]
    public void SyncDataIoAutoAttackCapacityRPC(Vector3 target)
    {
        caster = GetComponent<Entity>();
        champion = (Champion) caster;
        casterInitPos = GetCasterPos();
        direction = -(casterInitPos - target);
        direction.y = 0;
        direction.Normalize();
    }

    [PunRPC]
    public void SyncCastIoAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCast()
    {
        Debug.DrawRay(casterInitPos + champion.rotateParent.forward, direction * skillshotMaxDistance, Color.yellow,
            3);

        // Check conditions
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }

        if (count >= maxCount) return false;
        if (!canShootNewOne) return false;

        // Cast Succeeded!

        count++;
        canShootNewOne = false;

        hitPoint = Physics.Raycast(casterInitPos + champion.rotateParent.forward, direction, out var hit,
            skillshotMaxDistance, targetableLayer)
            ? hit.point
            : casterInitPos + champion.rotateParent.forward + direction * skillshotMaxDistance;

        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    private void CheckTimer()
    {
        if (delayTimer > delayDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckTimer;
            delayTimer = 0f;
            CastSkillShot();
        }
        else delayTimer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    private void CastSkillShot()
    {
        var allTargets = Physics.OverlapSphere(hitPoint, radius, targetableLayer);
        Debug.DrawLine(hitPoint, hitPoint + Vector3.right * .5f, Color.red, 2f);
        Debug.DrawLine(hitPoint, hitPoint - Vector3.right * .5f, Color.red, 2f);
        Debug.DrawLine(hitPoint, hitPoint + Vector3.forward * .5f, Color.red, 2f);
        Debug.DrawLine(hitPoint, hitPoint - Vector3.forward * .5f, Color.red, 2f);
        foreach (var c in allTargets)
        {
            var damageable = c.GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(damage, caster.entityIndex);
        }

        canShootNewOne = true;
    }

    protected override void StartCooldown()
    {
        photonView.RPC("SyncCanCastIoAutoAttackCapacityRPC", RpcTarget.All, false);
    }

    protected override void TimerCooldown()
    {
        cooldownTimer += 1.0 / GameStateMachine.Instance.tickRate;

        if (cooldownTimer >= cooldownDuration)
        {
            photonView.RPC("SyncCanCastIoAutoAttackCapacityRPC", RpcTarget.All, true);
            cooldownTimer = 0f;
            count = 0;
            GameStateMachine.Instance.OnTick -= TimerCooldown;
        }
    }

    [PunRPC]
    private void SyncCanCastIoAutoAttackCapacityRPC(bool canCast)
    {
        Debug.Log(canCast);
        canCastCapacity = canCast;
    }
}