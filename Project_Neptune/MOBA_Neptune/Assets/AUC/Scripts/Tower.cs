using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;

public partial class Tower : Building
{
    [Space] [Header("Tower settings")] public List<Entity> enemiesInRange = new List<Entity>();
    public LayerMask canBeHitByTowerMask;
    public int damage;
    public float detectionRange;
    public float delayBeforeAttack;
    public float detectionDelay;
    public float brainSpeed;
    public float timeBetweenShots;
    public bool isCycleAttack = false;

    private float brainTimer;
    [SerializeField] private ActiveCapacitySO attackCapa;

    [SerializeField] private MeshRenderer[] colorfulMeshes;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private Transform attackFrom;
    private LineRenderer towerLr;
    private Vector3[] points;

    protected override void OnStart()
    {
        base.OnStart();
        SetUpColor();
        towerLr = GetComponent<LineRenderer>();
    }

    private void SetUpColor()
    {
        var color = Color.white;

        foreach (var tc in GameStateMachine.Instance.teamColors)
        {
            if (tc.team == team) color = tc.color;
        }

        foreach (var rd in colorfulMeshes)
        {
            rd.material.SetColor(EmissionColor, color * 1);
        }
    }

    protected override void OnUpdate()
    {
        brainTimer += Time.deltaTime;
        if (brainTimer > brainSpeed)
        {
            TowerDetection();
            brainTimer = 0;
        }

        if (enemiesInRange.Count > 0)
        {
            points[0] = attackFrom.transform.position;
            points[1] = enemiesInRange[0].transform.position;
            towerLr.SetPositions(points);
        }
        else ResetLr();
    }

    void ResetLr()
    {
        if (towerLr.positionCount > 0)
            towerLr.positionCount = 0;
    }

    private void TowerDetection()
    {
        enemiesInRange.Clear();

        var size = Physics.OverlapSphere(transform.position, detectionRange, canBeHitByTowerMask);

        if (size.Length == 0) return;

        float tempDist = detectionRange;
        Collider tempEntity = null;
        foreach (var result in size)
        {
            if (result.GetComponent<Entity>().GetTeam() == GetTeam()) continue;

            float dist = Vector3.Distance(transform.position, result.transform.position);

            if (dist < detectionRange)
            {
                tempEntity = result;
                tempDist = dist;
            }
        }

        if (tempEntity) enemiesInRange.Add(tempEntity.GetComponent<Entity>());

        tempDist = detectionRange;
        tempEntity = null;
        foreach (var result in size)
        {
            var resultChamp = result.GetComponent<Champion>();
            if (!resultChamp || resultChamp.GetTeam() == GetTeam()) continue;
            float dist = Vector3.Distance(transform.position, result.transform.position);

            if (dist < detectionRange)
            {
                tempEntity = result;
                tempDist = dist;
            }
        }

        if (tempEntity) enemiesInRange.Add(tempEntity.GetComponent<Entity>());


        if (isCycleAttack == false && enemiesInRange.Count > 0)
            StartCoroutine(AttackTarget());
    }

    private IEnumerator AttackTarget()
    {
        isCycleAttack = true;

        yield return new WaitForSeconds(detectionDelay);

        int[] targetEntity = new[] { enemiesInRange[0].GetComponent<Entity>().entityIndex };

        AttackRPC(attackCapa.indexInCollection, targetEntity, Array.Empty<Vector3>());

        yield return new WaitForSeconds(timeBetweenShots);
        isCycleAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (enemiesInRange.Count > 0)
        {
            Gizmos.DrawLine(transform.position, enemiesInRange[0].transform.position);
        }
    }
}

public partial class Tower : IAttackable, IDeadable
{
    public bool CanAttack()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetCanAttack(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCanAttackRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCanAttackRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.BoolDelegate OnSetCanAttack;
    public event GlobalDelegates.BoolDelegate OnSetCanAttackFeedback;

    public float GetAttackDamage()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetAttackDamage(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetAttackDamageRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetAttackDamageRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetAttackDamage;
    public event GlobalDelegates.FloatDelegate OnSetAttackDamageFeedback;

    public void RequestAttack(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    public void SyncAttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);
        attackCapacity.PlayFeedback(capacityIndex, targetedEntities, targetedPositions);
        OnAttackFeedback?.Invoke(capacityIndex, targetedEntities, targetedPositions);
    }

    [PunRPC]
    public void AttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);

        if (!attackCapacity.TryCast(photonView.ViewID, targetedEntities, targetedPositions)) return;

        OnAttack?.Invoke(capacityIndex, targetedEntities, targetedPositions);
        photonView.RPC("SyncAttackRPC", RpcTarget.All, capacityIndex, targetedEntities, targetedPositions);
    }

    public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnAttack;
    public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnAttackFeedback;

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
        isAlive = false;
        Destroy(gameObject);
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
}