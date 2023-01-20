using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
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

    [SerializeField] private ParticleSystem slashVfx;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        kickCollider.team = GetComponent<Entity>().team;
        photonView.RPC("CastWaddyAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
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
            photonView.RPC("SyncCanCastWaddyAutoAttackCapacityRPC", RpcTarget.Others,false);
            photonView.RPC("SyncWaddyAutoAttackCastCapacityRPC", RpcTarget.Others);
            GameStateMachine.Instance.OnTick += TimerCooldown;
        }
    }

    [PunRPC]
    public void SyncWaddyAutoAttackCastCapacityRPC()
    {
        if (photonView.IsMine)
        {
            championCaster.myHud.spellHolderDict[this].StartTimer(cooldownDuration);
        }
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
        //colliderRd.enabled = true;
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
        photonView.RPC("AttackEndFeedback", RpcTarget.All);
    }

    [PunRPC]
    public void AttackEndFeedback()
    {
        attackCollider.enabled = false;
        //colliderRd.enabled = false;
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
        var color = canCast ? Color.blue : Color.red;
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
            var color = canCast ? Color.blue : Color.red;
            previewRenderer.material.SetColor("_EmissionColor", color);
        }
    }
}