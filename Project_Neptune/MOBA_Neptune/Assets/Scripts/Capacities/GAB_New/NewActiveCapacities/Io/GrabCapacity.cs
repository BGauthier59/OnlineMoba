using System;
using System.Collections;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using Entities.Interfaces;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class GrabCapacity : NewActiveCapacity
{
    public float grabMaxDistance;
    public LayerMask grabableLayer;

    public double delayDuration;
    private double delayTimer;

    private Vector3 direction;
    private Vector3 casterInitPos;

    private RaycastHit hitData;

    public ParticleSystem grabVFX;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastGrabCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
        RequestSetPreview(false);
    }

    [PunRPC]
    public void CastGrabCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        // Set data
        photonView.RPC("SyncDataGrabCapacityRPC", RpcTarget.All, targetedPositions[0]);
        
        if (TryCast())
        {
            StartCooldown();
            photonView.RPC("SyncCastGrabCapacityRPC", RpcTarget.All);
            GameStateMachine.Instance.OnTick += TimerCooldown;
        }
    }

    [PunRPC]
    public void SyncDataGrabCapacityRPC(Vector3 target)
    {
        casterInitPos = GetCasterPos();
        direction = -(casterInitPos - target);
        direction.y = 0;
        direction.Normalize();
    }

    [PunRPC]
    public void SyncCastGrabCapacityRPC()
    {
        photonView.RPC("ResetTriggerAnimation", RpcTarget.MasterClient, "IsGrabbing");
        if (photonView.IsMine) championCaster.myHud.spellHolderDict[this].StartTimer(cooldownDuration);
    }

    public override bool TryCast()
    {
        Debug.DrawRay(casterInitPos + championCaster.rotateParent.forward, direction * grabMaxDistance, Color.yellow,
            3);

        // Check conditions
        if (!canCastCapacity)
        {
            Debug.LogWarning("Still on cooldown!");
            return false;
        }
        photonView.RPC("SetTriggerAnimation", RpcTarget.MasterClient, "IsGrabbing");

        if (!Physics.Raycast(casterInitPos + championCaster.rotateParent.forward, direction, out var hit,
                grabMaxDistance, grabableLayer)) return false;

        // Cast Succeeded!
        hitData = hit;
        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    private void CheckTimer()
    {
        if (delayTimer > delayDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckTimer;
            delayTimer = 0f;
            CastGrab();
        }
        else delayTimer += 1.0 / GameStateMachine.Instance.tickRate;
    } // Timer between Input & RaycastHit application

    private void CastGrab()
    {
        Debug.DrawLine(casterInitPos, hitData.point, Color.red, 3);
        photonView.RPC("PlayHitEffect", RpcTarget.All, hitData.point);
        
        // We get hit IGrabable data
        var grabable = hitData.collider.gameObject.GetComponent<IGrabable>();
        if (grabable == null)
        {
            Debug.LogWarning("Target not grabable!");
            return;
        }

        // We get hit entity data
        var entity = hitData.collider.gameObject.GetComponent<Entity>();
        if (entity == caster)
        {
            Debug.LogWarning("Touched itself!");
            return;
        }

        var grabCaster = (Champion)caster;
        var team = entity.team;

        if (team == Enums.Team.Neutral)
        {
            var point = hitData.point;
            point.y = 1;
            grabCaster.grabbed.OnAddEffect(null, point);
        }
        else if (team != grabCaster.team)
        {
            entity.grabbed.OnAddEffect(grabCaster);
        }
        else
        {
            Debug.Log("You hit an ally!");
        }
    }
    
    [PunRPC]
    private void PlayHitEffect(Vector3 pos)
    {
        grabVFX.transform.position = pos;
        grabVFX.Play();
    }

    protected override void StartCooldown()
    {
        photonView.RPC("SyncCanCastGrabCapacityRPC", RpcTarget.All, false);
    }

    protected override void TimerCooldown()
    {
        cooldownTimer += 1.0 / GameStateMachine.Instance.tickRate;

        if (cooldownTimer >= cooldownDuration)
        {
            photonView.RPC("SyncCanCastGrabCapacityRPC", RpcTarget.All, true);
            cooldownTimer = 0f;
            GameStateMachine.Instance.OnTick -= TimerCooldown;
        }
    }

    public override void RequestSetPreview(bool active)
    {
        photonView.RPC("SetPreviewGrabRPC", RpcTarget.All, active, canCastCapacity);
    }

    [PunRPC]
    public void SetPreviewGrabRPC(bool active, bool canCast)
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
        var pos = championCaster.transform.position;
        previewObject.rotation = Quaternion.Lerp(previewObject.rotation,
            Quaternion.LookRotation(championCaster.controller.cursorWorldPos[0] - pos), Time.deltaTime * 15);
        var euler = previewObject.eulerAngles;
        euler.x = 90;
        euler.z = 0;
        previewObject.eulerAngles = euler;
    }

    [PunRPC]
    private void SyncCanCastGrabCapacityRPC(bool canCast)
    {
        canCastCapacity = canCast;
        if (previewActivate && photonView.IsMine)
        {
            var color = canCast ? championCaster.previewColorEnable : championCaster.previewColorDisable;
            previewRenderer.material.SetColor("_EmissionColor", color);
        }
    }
    
    public IEnumerator WaitForAnim(float timeToWait)
    {
        championCaster.animator.speed = 1;
        championCaster.isPlayingNonScalableAnim = true;
        yield return new WaitForSeconds(timeToWait);
        championCaster.isPlayingNonScalableAnim = false;
    }
}