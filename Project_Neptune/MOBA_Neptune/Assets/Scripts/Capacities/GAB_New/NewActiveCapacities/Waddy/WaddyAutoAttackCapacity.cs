using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using GameStates;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class WaddyAutoAttackCapacity : NewActiveCapacity
{
    public double attackDuration;
    private double attackTimer;

    [SerializeField] private Collider attackCollider;
    public KickCollider kickCollider;

    [SerializeField] private ParticleSystem slashVfx;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        //if (kickCollider.team == Enums.Team.Neutral) photonView.RPC("SetKickColliderTeamRPC", RpcTarget.MasterClient, championCaster.entityIndex);
        photonView.RPC("CastWaddyAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
    }

    [PunRPC] [UsedImplicitly]
    public void SetKickColliderTeamRPC(int entityIndex)
    {
        kickCollider.team = EntityCollectionManager.GetEntityByIndex(entityIndex).team;
        photonView.RPC("SyncSetKickColliderTeamRPC", RpcTarget.All, entityIndex);
    }
    
    [PunRPC] [UsedImplicitly]
    public void SyncSetKickColliderTeamRPC(int entityIndex)
    {
        kickCollider.team = EntityCollectionManager.GetEntityByIndex(entityIndex).team;
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

        if (!TryCast()) return;
        StartCooldown();
        photonView.RPC("SyncCanCastWaddyAutoAttackCapacityRPC", RpcTarget.All, false);
        photonView.RPC("SyncWaddyAutoAttackCastCapacityRPC", RpcTarget.All, caster.entityIndex);
        GameStateMachine.Instance.OnTick += TimerCooldown;
    }

    [PunRPC]
    public void SyncWaddyAutoAttackCastCapacityRPC(int entityIndex)
    {
        if (photonView.IsMine)
        {
            //championCaster.GetComponent<Champion>().myHud.spellHolderDict[this].StartTimer(cooldownDuration);
        }
    }

    public override bool TryCast()
    {
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }

        Attack();
        return true;
    }

    private void Attack()
    {
        championCaster.animator.Play("A_AutoAttack");
        championCaster.SetCanRotate(false);
        photonView.RPC("AttackFeedbackRPC", RpcTarget.All);
        GameStateMachine.Instance.OnTick += CheckAttackTimer;
    }

    [PunRPC]
    public void AttackFeedbackRPC()
    {
        attackCollider.enabled = true;
        slashVfx.Play();
       
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
        championCaster.SetCanRotate(true);
        photonView.RPC("AttackEndFeedback", RpcTarget.All);
    }

    [PunRPC]
    public void AttackEndFeedback()
    {
        attackCollider.enabled = false;
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

    public override void RequestSetPreview(bool active)
    {
        photonView.RPC("SetPreviewWaddyAutoAttackRPC", RpcTarget.All, active, canCastCapacity);
    }

    [PunRPC]
    public void SetPreviewWaddyAutoAttackRPC(bool active, bool canCast)
    {
        if (!photonView.IsMine) return;
        previewActivate = active;
        previewObject.gameObject.SetActive(active);
        var color = canCast ? championCaster.previewColorEnable : championCaster.previewColorDisable;
        previewRenderer.material.SetColor("_EmissionColor", color);
    }

    public override void Update()
    {
        if (previewActivate) UpdatePreview();
    }

    public override void UpdatePreview()
    {
        if (!photonView.IsMine) return;
    }

    [PunRPC]
    private void SyncCanCastWaddyAutoAttackCapacityRPC(bool canCast)
    {
        Debug.Log("Deactivate");

        canCastCapacity = canCast;
        if (previewActivate && photonView.IsMine)
        {
            var color = canCast ? championCaster.previewColorEnable : championCaster.previewColorDisable;
            previewRenderer.material.SetColor("_EmissionColor", color);
        }
    }
}