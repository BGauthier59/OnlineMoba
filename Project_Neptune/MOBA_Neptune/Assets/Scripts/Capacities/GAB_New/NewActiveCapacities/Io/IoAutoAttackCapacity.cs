using System.Collections;
using System.Collections.Generic;
using Controllers.Inputs;
using Entities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class IoAutoAttackCapacity : NewActiveCapacity
{
    public LayerMask targetableLayer;

    public double delayDuration;
    private double delayTimer;

    public double resetDuration;
    private double resetTimer;

    [SerializeField] private float radius;
    [SerializeField] private float damage;
    [SerializeField] private int maxCount = 1;
    private int count;
    private bool canShootNewOne = true;

    private Vector3 direction;
    private Vector3 casterInitPos;

    private Vector3 hitPoint;
    private ChampionInputController _championInputController;

    [SerializeField] private ParticleSystem iceImpactFx;
    [SerializeField] private ParticleSystem iceMuzzleFx;

    public override void Start()
    {
        base.Start();
        iceImpactFx.transform.SetParent(null);
    }

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastIoAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
    }

    [PunRPC]
    public void CastIoAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncDataIoAutoAttackCapacityRPC", RpcTarget.All, targetedPositions[0]);

        if (!TryCast()) return;
        if (count != maxCount) return;
        photonView.RPC("SyncCanCastIoAutoAttackCapacityRPC", RpcTarget.All, false);
        photonView.RPC("SyncCastIoAutoAttackCapacityRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SyncDataIoAutoAttackCapacityRPC(Vector3 target)
    {
        casterInitPos = GetCasterPos();
        direction = -(casterInitPos - target);
        direction.y = 0;
    }

    [PunRPC]
    public void SyncCastIoAutoAttackCapacityRPC()
    {
        if (!championCaster.myHud) return;
        if (photonView.IsMine) championCaster.myHud.spellHolderDict[this].StartTimer(resetDuration + delayDuration);
    }

    public override bool TryCast()
    {
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }

        if (count >= maxCount) return false;
        if (!canShootNewOne) return false;
        count++;
        canShootNewOne = false;

        hitPoint = Physics.Raycast(casterInitPos + championCaster.rotateParent.forward, direction, out var hit,
            direction.magnitude, targetableLayer)
            ? hit.point
            : casterInitPos + championCaster.rotateParent.forward + direction;
        
        photonView.RPC("PlayExplosionFeedback", RpcTarget.All, GetCasterPos());

        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    [PunRPC]
    public void PlayExplosionFeedback(Vector3 casterPos)
    {
        iceMuzzleFx.transform.position = casterPos;
        iceMuzzleFx.Play();

        iceImpactFx.transform.position = previewObject.position;
        iceImpactFx.Play();
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

        foreach (var c in allTargets)
        {
            var entity = c.GetComponent<Entity>();
            if (entity == null) continue;
            if (entity.team == caster.team) continue;

            var damageable = c.GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(damage, caster.entityIndex);

            if (entity.marked == null) continue;
            entity.marked.OnAddEffect();
        }

        resetTimer = 0f;
        if (count == 1) GameStateMachine.Instance.OnTick += CheckResetTimer;
        canShootNewOne = true;
    }

    private void CheckResetTimer()
    {
        if (resetTimer > resetDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckResetTimer;
            resetTimer = 0f;
            count = 0;
            StartCooldown();
            GameStateMachine.Instance.OnTick += TimerCooldown;
        }
        else resetTimer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    protected override void StartCooldown() { }

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

    public override void RequestSetPreview(bool active)
    {
        photonView.RPC("SetPreviewIoAutoAttackRPC", RpcTarget.All, active, canCastCapacity);
    }

    [PunRPC]
    public void SetPreviewIoAutoAttackRPC(bool active, bool canCast)
    {
        if (!photonView.IsMine) return;
        previewActivate = true;
        previewObject.gameObject.SetActive(true);
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
        var pos = championCaster.controller.cursorWorldPos[0];
        pos.y = 1;
        previewObject.position = pos;
    }

    [PunRPC]
    private void SyncCanCastIoAutoAttackCapacityRPC(bool canCast)
    {
        canCastCapacity = canCast;
        if (previewActivate && photonView.IsMine)
        {
            var color = canCast ? championCaster.previewColorEnable : championCaster.previewColorDisable;
            previewRenderer.material.SetColor("_EmissionColor", color);
        }

        canShootNewOne = true;
    }
}