using System;
using System.Collections.Generic;
using Controllers;
using Entities;
using Entities.FogOfWar;
using Entities.Interfaces;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public partial class MinionBehaviour : Entity
{
    #region MinionVariables

    [Space]
    public NavMeshAgent myAgent;
    private MinionController myController;

    [Header("Pathfinding")] 
    [SerializeField] private StreamModifier currentStreamModifier;
    public Transform myWayPoint;
    public int wayPointIndex;

    [Header("Stats")] public float currentHealth;
    public float maxHealth;
    public float speed;

    #endregion

    protected override void OnStart()
    {
        base.OnStart();
        myAgent = GetComponent<NavMeshAgent>();
        myController = GetComponent<MinionController>();
        currentHealth = maxHealth;
    }

    //------ State Methods

    public void IdleState()
    {
        myAgent.isStopped = true;
    }

    public void WalkingState()
    {
        var strength = StreamManager.GetStreamVector(currentStreamModifier, transform);
        Debug.DrawRay(transform.position, strength, Color.magenta);
        Vector3 targetDestination = (transform.position + strength);
        myAgent.SetDestination(targetDestination);
    }

    public void LookingForPathingState()
    {
        myAgent.SetDestination(myWayPoint.position);
        
        if (Vector3.Distance(transform.position, myWayPoint.position) < myAgent.stoppingDistance)
            myController.currentState = MinionController.MinionState.Walking;
    }
}


public partial class MinionBehaviour : IDeadable, IMoveable, IDamageable, IStreamable
{
    //------

    public override void OnInstantiated()
    {
        base.OnInstantiated();
    }

    public override void OnInstantiatedFeedback()
    {
    }

    public bool CanMove()
    {
        throw new System.NotImplementedException();
    }

    public float GetReferenceMoveSpeed()
    {
        throw new System.NotImplementedException();
    }

    public float GetCurrentMoveSpeed()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetCanMove(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCanMoveRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCanMoveRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.BoolDelegate OnSetCanMove;
    public event GlobalDelegates.BoolDelegate OnSetCanMoveFeedback;

    public void RequestSetReferenceMoveSpeed(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetReferenceMoveSpeedRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetReferenceMoveSpeedRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeedFeedback;

    public void RequestIncreaseReferenceMoveSpeed(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncIncreaseReferenceMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseReferenceMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeedFeedback;

    public void RequestDecreaseReferenceMoveSpeed(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncDecreaseReferenceMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseReferenceMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeedFeedback;

    public void RequestSetCurrentMoveSpeed(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCurrentMoveSpeedRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentMoveSpeedRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeedFeedback;

    public void RequestIncreaseCurrentMoveSpeed(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncIncreaseCurrentMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseCurrentMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeedFeedback;

    public void RequestDecreaseCurrentMoveSpeed(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncDecreaseCurrentMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseCurrentMoveSpeedRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeed;
    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeedFeedback;

    public void RequestMove(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void RequestMoveDir(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void SyncMoveRPC(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void MoveRPC(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.Vector3Delegate OnMove;
    public event GlobalDelegates.Vector3Delegate OnMoveFeedback;

    public float GetMaxHp()
    {
        throw new NotImplementedException();
    }

    public float GetCurrentHp()
    {
        throw new NotImplementedException();
    }

    public float GetCurrentHpPercent()
    {
        throw new NotImplementedException();
    }

    public void RequestSetMaxHp(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetMaxHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetMaxHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetMaxHp;
    public event GlobalDelegates.FloatDelegate OnSetMaxHpFeedback;

    public void RequestIncreaseMaxHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncIncreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void IncreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnIncreaseMaxHp;
    public event GlobalDelegates.FloatDelegate OnIncreaseMaxHpFeedback;

    public void RequestDecreaseMaxHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncDecreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void DecreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseMaxHp;
    public event GlobalDelegates.FloatDelegate OnDecreaseMaxHpFeedback;

    public void RequestSetCurrentHp(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCurrentHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetCurrentHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetCurrentHp;
    public event GlobalDelegates.FloatDelegate OnSetCurrentHpFeedback;

    public void RequestSetCurrentHpPercent(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCurrentHpPercentRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetCurrentHpPercentRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercent;
    public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercentFeedback;

    public void RequestIncreaseCurrentHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncIncreaseCurrentHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void IncreaseCurrentHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHp;
    public event GlobalDelegates.FloatDelegate OnIncreaseCurrentHpFeedback;


    public void RequestDecreaseCurrentHp(float amount)
    {
        photonView.RPC("DecreaseCurrentHpRPC", RpcTarget.MasterClient, amount);
    }

    [PunRPC]
    public void SyncDecreaseCurrentHpRPC(float amount)
    {
        currentHealth = amount;
    }

    [PunRPC]
    public void DecreaseCurrentHpRPC(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        photonView.RPC("SyncDecreaseCurrentHpRPC", RpcTarget.All, currentHealth);

        if (currentHealth <= 0)
        {
            RequestDie();
        }
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHp;
    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHpFeedback;

    public bool IsAlive()
    {
        throw new NotImplementedException();
    }

    public bool CanDie()
    {
        throw new NotImplementedException();
    }

    public void RequestSetCanDie(bool value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCanDieRPC(bool value)
    {
        throw new NotImplementedException();
    }

    public void SetCanDieRPC(bool value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.BoolDelegate OnSetCanDie;
    public event GlobalDelegates.BoolDelegate OnSetCanDieFeedback;

    public void RequestDie()
    {
        photonView.RPC("DieRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void SyncDieRPC()
    {
        PoolNetworkManager.Instance.PoolRequeue(this);
        FogOfWarManager.Instance.RemoveFOWViewable(this);
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void DieRPC()
    {
        photonView.RPC("SyncDieRPC", RpcTarget.All);
    }

    public event GlobalDelegates.NoParameterDelegate OnDie;
    public event GlobalDelegates.NoParameterDelegate OnDieFeedback;

    public void RequestRevive()
    {
        throw new NotImplementedException();
    }

    public void SyncReviveRPC()
    {
        throw new NotImplementedException();
    }

    public void ReviveRPC()
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.NoParameterDelegate OnRevive;
    public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    public StreamModifier GetCurrentStreamModifier()
    {
        return currentStreamModifier;
    }

    public void SetStreamModifier(StreamModifier modifier)
    {
        currentStreamModifier = modifier;
    }
}