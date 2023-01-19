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

    public double resetDuration;
    private double resetTimer;

    [SerializeField] private float radius;
    [SerializeField] private float damage;
    [SerializeField] private int maxCount;
    private int count;
    private bool canShootNewOne = true;

    private Vector3 direction;
    private Vector3 casterInitPos;

    private Vector3 hitPoint;

    [SerializeField] private ParticleSystem iceImpactFx;
    [SerializeField] private ParticleSystem iceMuzzleFx;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastIoAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
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
                caster.GetComponent<Champion>().myHud.spellHolderDict[this].StartTimer(cooldownDuration);
                photonView.RPC("SyncCanCastIoAutoAttackCapacityRPC", RpcTarget.All, false);
            }
        }
    }

    [PunRPC]
    public void SyncDataIoAutoAttackCapacityRPC(Vector3 target)
    {
        casterInitPos = GetCasterPos();
        direction = -(casterInitPos - target);
        direction.y = 0;
    }

    [PunRPC]
    public void SyncCastIoAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
    }

    public override bool TryCast()
    {
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

        hitPoint = Physics.Raycast(casterInitPos + championCaster.rotateParent.forward, direction, out var hit,
            direction.magnitude, targetableLayer)
            ? hit.point
            : casterInitPos + championCaster.rotateParent.forward + direction;

        photonView.RPC("PlayIceMuzzleFeedback", RpcTarget.All, GetCasterPos());

        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    [PunRPC]
    public void PlayIceMuzzleFeedback(Vector3 pos)
    {
        iceMuzzleFx.transform.position = pos;
        var rotation = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        iceMuzzleFx.transform.rotation = Quaternion.Euler(0, 0, rotation);
        iceMuzzleFx.Play();
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

        photonView.RPC("PlayIceImpactFeedback", RpcTarget.All, hitPoint);

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

        if (count == 1)
        {
            GameStateMachine.Instance.OnTick += CheckResetTimer;
        }

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

    [PunRPC]
    public void PlayIceImpactFeedback(Vector3 pos)
    {
        iceImpactFx.transform.position = pos;
        var rotation = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        iceImpactFx.transform.rotation = Quaternion.Euler(0, 0, rotation);
        iceImpactFx.Play();
    }

    protected override void StartCooldown()
    {
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

    public override void RequestSetPreview(bool active)
    {
        photonView.RPC("SetPreviewIoAutoAttackRPC", RpcTarget.All, active, canCastCapacity);
    }

    [PunRPC]
    public void SetPreviewIoAutoAttackRPC(bool active, bool canCast)
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
            var color = canCast ? Color.blue : Color.red;
            previewRenderer.material.SetColor("_EmissionColor", color);
        }

        canShootNewOne = true;
    }
}