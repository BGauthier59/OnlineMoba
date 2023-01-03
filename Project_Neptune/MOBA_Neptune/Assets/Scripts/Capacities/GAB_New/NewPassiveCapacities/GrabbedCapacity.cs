using System;
using Entities;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public class GrabbedCapacity : NewPassiveCapacity
{
    [SerializeField] private float duration;
    private float timer;

    private Entity giver;
    private Vector3 point;

    private IDisplaceable displaceable;

    private float initDistance;
    [SerializeField] private float distanceSpeedFactor;
    [SerializeField] private float speed;

    private float securityTimer;

    private GrabbedState state;

    private enum GrabbedState
    {
        Moving,
        Hitting,
        Hooking
    }

    // Se lance sur le Master
    public override void OnAddEffect(Entity giver = null, Vector3 position = default)
    {
        if (giver) photonView.RPC("AddGrabbedEffectRPC", RpcTarget.All, giver.entityIndex, position);
        else photonView.RPC("AddGrabbedEffectRPC", RpcTarget.All, -1, position);

        var pointToReach = giver == null ? position : giver.transform.position;
        pointToReach.y = 1;
        initDistance = Vector3.Distance(transform.position, pointToReach);

        SwitchState(GrabbedState.Moving);
        base.OnAddEffect(giver, position);
    }

    [PunRPC]
    private void AddGrabbedEffectRPC(int index, Vector3 pos)
    {
        isActive = true;

        point = pos;
        giver = index == -1 ? null : EntityCollectionManager.GetEntityByIndex(index);

        displaceable = GetComponent<IDisplaceable>();
        if (displaceable == null)
        {
            Debug.LogWarning("Can't displace this grabable entity?");
            return;
        }

        if (!photonView.IsMine) return;
        InputManager.PlayerMap.Movement.Disable();
        ((Champion) entityUnderEffect).OnGrabbed();
    }

    private void Start()
    {
        entityUnderEffect = GetComponent<Entity>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!isActive) return;

        OnUpdateEffect();
    }

    public override void OnUpdateEffect()
    {
        // Si l'entité a touché sa cible, elle peut rester fixée ?
        // Si elle est fixée, on check si le joueur essaie de bouger, ce qui peut déclencher le remove effect, sinon c'est un timer

        switch (state)
        {
            case GrabbedState.Moving:
                SetVelocityToTarget(giver == null ? point : giver.transform.position);
                break;
            case GrabbedState.Hitting:
                GrabbedEntityHitTarget();
                break;
            case GrabbedState.Hooking:
                HookEntityToTarget();
                break;
        }
    }

    private void SwitchState(GrabbedState state)
    {
        switch (state)
        {
            case GrabbedState.Moving:
                break;
            case GrabbedState.Hitting:
                break;
            case GrabbedState.Hooking:
                break;
        }

        this.state = state;
    }

    private void SetVelocityToTarget(Vector3 point)
    {
        var distance = Vector3.Distance(transform.position, point);

        var crossedDistance = Mathf.Abs(distance - initDistance);

        var velocity = (point - transform.position) *
                       ((crossedDistance + .5f) * distanceSpeedFactor * speed);
        velocity.y = 0;
        displaceable.SetVelocity(velocity);

        CheckEntityHitTarget(distance);
    }

    private void CheckEntityHitTarget(float distance)
    {
        if (securityTimer >= 5)
        {
            Debug.LogWarning($"Can't reach its target at pos {point}");
            SwitchState(GrabbedState.Hitting);
        }
        else securityTimer += Time.deltaTime;

        if (!(distance < 1.2f)) return;
        SwitchState(GrabbedState.Hitting);
    }

    private void GrabbedEntityHitTarget()
    {
        // Set velocity to 0
        displaceable.SetVelocity(Vector3.zero);

        // RPC Feedbacks
        photonView.RPC("OnHitTargetRPC", RpcTarget.All);

        SwitchState(GrabbedState.Hooking);
    }

    private void HookEntityToTarget()
    {
        if (giver != null) timer = duration;

        if (timer >= duration)
        {
            timer = 0f;
            OnRemoveEffect();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public override void OnRemoveEffect()
    {
        base.OnRemoveEffect();
        photonView.RPC("RemoveGrabbedEffectRPC", RpcTarget.All);
    }

    public void MoveWhileHooked()
    {
        if (!isActive) return;
        photonView.RPC("MoveWhileHookedRPC", RpcTarget.MasterClient); 
    }

    [PunRPC]
    public void MoveWhileHookedRPC()
    {
        timer = duration;
    }

    [PunRPC]
    private void RemoveGrabbedEffectRPC()
    {
        if (!photonView.IsMine) return;
        ((Champion) entityUnderEffect).OnUnGrabbed();
    }

    [PunRPC]
    private void OnHitTargetRPC()
    {
        // Feedbacks


        // Global?


        if (!photonView.IsMine) return;
        InputManager.PlayerMap.Movement.Enable();
    }
}