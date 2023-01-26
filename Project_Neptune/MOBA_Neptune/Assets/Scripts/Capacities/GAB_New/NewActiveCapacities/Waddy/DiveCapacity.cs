using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class DiveCapacity : NewActiveCapacity
{
    public double delayDuration;
    private double delayTimer;
    [SerializeField] private float radius;
    public LayerMask targetableLayer;
    [SerializeField] private uint damage;

    [SerializeField] private ParticleSystem impactFx;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastDiveCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
    }

    [PunRPC]
    public void SyncDataDiveCapacityRPC()
    {
        caster = GetComponent<Entity>();
    }

    [PunRPC]
    public void CastDiveCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        // Set data
        photonView.RPC("SyncDataDiveCapacityRPC", RpcTarget.All);

        if (TryCast())
        {
            StartCooldown();
            GameStateMachine.Instance.OnTick += TimerCooldown;
        }
    }

    [PunRPC]
    public void SyncCastDiveCapacityRPC()
    {
        photonView.RPC("ResetTriggerAnimation", RpcTarget.MasterClient, "IsJumping");
        if (!photonView.IsMine) return;
        InputManager.EnablePlayerMap(false);
        championCaster.myHud.spellHolderDict[this].StartTimer(cooldownDuration);
    }

    public override bool TryCast()
    {
        // Check conditions
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }
        
        photonView.RPC("SetTriggerAnimation", RpcTarget.MasterClient, "IsJumping");
        photonView.RPC("SyncCastDiveCapacityRPC", RpcTarget.All);
        photonView.RPC("SyncImpactFeedback", RpcTarget.All);

        GameStateMachine.Instance.OnTick += CheckTimer;

        return true;
    }

    private void CheckTimer()
    {
        if (delayTimer > delayDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckTimer;
            delayTimer = 0f;
            HitGround();
        }
        else delayTimer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    private void HitGround()
    {
        var pos = GetCasterPos();
        var allTargets = Physics.OverlapSphere(pos, radius, targetableLayer);
        Debug.DrawLine(pos, pos + Vector3.right * radius, Color.red, 2f);
        Debug.DrawLine(pos, pos - Vector3.right * radius, Color.red, 2f);
        Debug.DrawLine(pos, pos + Vector3.forward * radius, Color.red, 2f);
        Debug.DrawLine(pos, pos - Vector3.forward * radius, Color.red, 2f);
        
        foreach (var c in allTargets)
        {
            var entity = c.GetComponent<Entity>();
            if (entity == null)
            {
                Debug.LogWarning("Entity is null!");
                continue;
            }

            if (entity.team == caster.team) continue;

            var damageable = c.GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(damage, caster.entityIndex);
        }

        photonView.RPC("EnableInputAfterHitGround", RpcTarget.All);
    }

    [PunRPC]
    public void SyncImpactFeedback()
    {
        impactFx.Play();
    }

    [PunRPC]
    public void EnableInputAfterHitGround()
    {
        if (!photonView.IsMine) return;
        InputManager.EnablePlayerMap(true);
    }

    protected override void StartCooldown()
    {
        photonView.RPC("SyncCanCastDiveCapacityRPC", RpcTarget.All, false);
    }

    protected override void TimerCooldown()
    {
        cooldownTimer += 1.0 / GameStateMachine.Instance.tickRate;

        if (cooldownTimer >= cooldownDuration)
        {
            photonView.RPC("SyncCanCastDiveCapacityRPC", RpcTarget.All, true);
            cooldownTimer = 0f;
            GameStateMachine.Instance.OnTick -= TimerCooldown;
        }
    }

    public override void RequestSetPreview(bool active)
    {
        photonView.RPC("SetPreviewDiveRPC", RpcTarget.All, active, canCastCapacity);
    }

    [PunRPC]
    public void SetPreviewDiveRPC(bool active, bool canCast)
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
        var pos = GetCasterPos();
        previewObject.position = pos;
    }

    [PunRPC]
    private void SyncCanCastDiveCapacityRPC(bool canCast)
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