using Capacities.Passive_Capacities;
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
    public GrabbedCapacitySO passiveEffect;
    
    public double delayDuration;
    private double timer;

    private Vector3 direction;
    private Vector3 casterInitPos;

    private RaycastHit hitData;
    private Champion champion;

    public ParticleSystem grabVFX;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC]
    public override void CastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        // Set data
        photonView.RPC("SyncDataRPC", RpcTarget.All, targetedPositions[0]);

        if (TryCast())
        {
            Debug.Log("Grab worked!");
        }
        else
        {
            ((Champion) caster).controller.canCastCapacity1 = true;
        }
    }

    [PunRPC]
    public void SyncDataRPC(Vector3 target)
    {
        caster = GetComponent<Entity>();
        champion = (Champion) caster;
        casterInitPos = GetCasterPos();
        direction = -(casterInitPos - target);
        direction.y = 0;
        direction.Normalize();
    }

    [PunRPC]
    public override void SyncCastCapacityRPC(int[] targetedEntities, Vector3[] targetedPosition)
    {
    }

    public override bool TryCast()
    {
        Debug.DrawRay(casterInitPos + champion.rotateParent.forward, direction * grabMaxDistance, Color.yellow,
            3);

        // Check conditions
        if (!Physics.Raycast(casterInitPos + champion.rotateParent.forward, direction, out var hit,
            grabMaxDistance, grabableLayer))
        {
            Debug.Log("Raycast for grab hit nothing!");
            return false;
        }

        hitData = hit;
        GameStateMachine.Instance.OnTick += CheckTimer;
        return true;
    }

    private void CheckTimer()
    {
        if (timer > delayDuration)
        {
            GameStateMachine.Instance.OnTick -= CheckTimer;
            CastGrab();
        }
        else timer += 1.0 / GameStateMachine.Instance.tickRate;
    }

    private void CastGrab()
    {
        Debug.DrawLine(casterInitPos, hitData.point, Color.red, 3);
        photonView.RPC("PlayHitEffect", RpcTarget.All, hitData.point);

        // We get hit IGrabable data
        var grabable = hitData.collider.gameObject.GetComponent<IGrabable>();
        if (grabable == null) return;

        // We get hit entity data
        var entity = hitData.collider.gameObject.GetComponent<Entity>();
        if (entity == caster)
        {
            Debug.LogWarning("Touched itself!");
            return;
        }

        var team = entity.team;
        var capacityIndex = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(passiveEffect);

        if (team == caster.team)
        {
            caster.AddPassiveCapacityRPC(capacityIndex, entity.entityIndex);
        }
        else if (team == Enums.Team.Neutral)
        {
            var point = hitData.point;
            point.y = 1;
            caster.AddPassiveCapacityRPC(capacityIndex, default, point);
        }
        else
        {
            Debug.Log("You grabbed an enemy");
            var point = (entity.transform.position + caster.transform.position) * .5f;
            entity.AddPassiveCapacityRPC(capacityIndex, default, point);
            caster.AddPassiveCapacityRPC(capacityIndex, default, point);
        }
    }
    
    [PunRPC]
    private void PlayHitEffect(Vector3 pos)
    {
        grabVFX.transform.position = pos;
        grabVFX.Play();
    }
}