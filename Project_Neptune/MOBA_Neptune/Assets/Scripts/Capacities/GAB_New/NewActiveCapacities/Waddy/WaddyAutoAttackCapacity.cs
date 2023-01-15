using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class WaddyAutoAttackCapacity : NewActiveCapacity
{
    public double delayDuration;
    private double delayTimer;
    public double attackDuration;
    private double attackTimer;

    [SerializeField] private Collider attackCollider;
    public KickCollider kickCollider;
    public Renderer colliderRd;
    
    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        kickCollider.team = GetComponent<Entity>().team;
        photonView.RPC("CastWaddyAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC]
    public void SyncDataWaddyAutoAttackCapacityRPC()
    {
        caster = GetComponent<Entity>();
    }

    [PunRPC]
    public void CastWaddyAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        // Set data
        photonView.RPC("SyncDataWaddyAutoAttackCapacityRPC", RpcTarget.All);

        if (TryCast())
        {
            StartCooldown();
            GameStateMachine.Instance.OnTick += TimerCooldown;
        }
    }

    [PunRPC]
    public void SyncWaddyAutoAttackCastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCast()
    {
        // Check conditions
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }

        // TODO - Play Anim
        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    private void CheckTimer()
    {
        if (delayTimer > delayDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckTimer;
            delayTimer = 0f;
            Attack();
        }
        else delayTimer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    private void Attack()
    {
        photonView.RPC("AttackFeedback", RpcTarget.All);
        GameStateMachine.Instance.OnTick += CheckAttackTimer;
    }

    [PunRPC]
    public void AttackFeedback()
    {
        attackCollider.enabled = true;
        colliderRd.enabled = true;
    }

    private void CheckAttackTimer()
    {
        if (attackTimer > attackDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckAttackTimer;
            attackTimer = 0f;
            AttackEnd();
        }
        else attackTimer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    private void AttackEnd()
    {
        photonView.RPC("AttackEndFeedback", RpcTarget.All);
    }

    [PunRPC]
    public void AttackEndFeedback()
    {
        attackCollider.enabled = false;
        colliderRd.enabled = false;
    }

    protected override void StartCooldown()
    {
        photonView.RPC("SyncCanCastWaddyAutoAttackCapacityRPC", RpcTarget.All, false);
    }

    protected override void TimerCooldown()
    {
        cooldownTimer += 1.0 / GameStateMachine.Instance.tickRate;

        if (cooldownTimer >= cooldownDuration)
        {
            photonView.RPC("SyncCanCastWaddyAutoAttackCapacityRPC", RpcTarget.All, true);
            cooldownTimer = 0f;
            GameStateMachine.Instance.OnTick -= TimerCooldown;
        }
    }

    [PunRPC]
    private void SyncCanCastWaddyAutoAttackCapacityRPC(bool canCast)
    {
        canCastCapacity = canCast;
    }
}