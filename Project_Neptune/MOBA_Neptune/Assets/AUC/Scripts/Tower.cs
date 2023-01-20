using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using GameStates;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public partial class Tower : Building
{
    [Space] [Header("Tower settings")] public List<Entity> enemiesInRange = new List<Entity>();
    public LayerMask canBeHitByTowerMask;
    public int damage;
    public float detectionRange;
    public float delayBeforeAttack;
    public float delayBeforeDamage;
    public float brainSpeed;
    public float reloadTime;
    public bool isCycleAttack = false;
    
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject towerProjectile;
    
    // Prep liaison tour - player
    public bool isActive;
    public bool isIoTower;
    public int entityLinkIndex;
    
    private float brainTimer;
    [SerializeField] private ActiveCapacitySO attackCapa;

    [SerializeField] private MeshRenderer[] colorfulMeshes;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private LineRenderer _lineRenderer;
    
    [SerializeField] private GameObject warningPoint;
    [SerializeField] private int playerID;
    [SerializeField] private int localEnemiesInRange;
    [SerializeField] private int localPlayerFocused;

    protected override void OnStart()
    {
        isActive = false;
        base.OnStart();
        EntityCollectionManager.towerList.Add(this);
        SetUpColor();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void SetUpColor()
    {
        
    }

    private float elapsedTime = 0f;
    private bool projectileAlive = true;
    protected override void OnUpdate()
    {
        if (!isActive) return;

        // Local
        if (playerID == 0) playerID = GameStateMachine.Instance.GetPlayerChampionPhotonViewId();
        if (localEnemiesInRange > 0) warningPoint.SetActive(localPlayerFocused == playerID);

        // Master
        if (!PhotonNetwork.IsMasterClient) return; 
        brainTimer += Time.deltaTime;
        if (brainTimer > brainSpeed)
        {
            TowerDetection();
            brainTimer = 0;
        }

        
        if (isCycleAttack && projectileAlive)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / delayBeforeDamage;
            
            tempProjectile.transform.position = Vector3.Lerp(transform.position + Vector3.up * 2, actualAimedEntity.transform.position, percentageComplete);
        }
        
        // Line Renderer 
        if (enemiesInRange.Count > 0)
        {
            photonView.RPC("SyncLineRendererRPC", RpcTarget.All, enemiesInRange[0].transform.position);
            photonView.RPC("SyncPlayerInfoRPC", RpcTarget.All, enemiesInRange.Count, enemiesInRange[0].GetComponent<Entity>().entityIndex);
        }
        else
        {
            photonView.RPC("ResetLrRPC", RpcTarget.All);
            photonView.RPC("SyncPlayerInfoRPC", RpcTarget.All, 0, 0);
        }
    }
    
    [PunRPC] [UsedImplicitly]
    private void SyncPlayerInfoRPC(int i, int j)
    {
        localEnemiesInRange = i;
        localPlayerFocused = j;
    }

    [PunRPC] [UsedImplicitly]
    private void SyncLineRendererRPC(Vector3 targetTransform)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.position + Vector3.up * 2.5f);
        _lineRenderer.SetPosition(1, targetTransform + Vector3.up * 2);
    }

    [PunRPC] [UsedImplicitly]
    private void ResetLrRPC()
    {
        _lineRenderer.positionCount = 0;
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

    private GameObject tempProjectile;

    private Entity actualAimedEntity;
    private IEnumerator AttackTarget()
    {
        isCycleAttack = true;
        actualAimedEntity = enemiesInRange[0]; // Choix de la cible
        // Instanciation du projectile
        tempProjectile = PhotonNetwork.Instantiate(towerProjectile.name, transform.position + Vector3.up * 2.5f, Quaternion.identity);
        projectileAlive = true;
        yield return new WaitForSeconds(delayBeforeDamage);
        projectileAlive = false;
        Destroy(tempProjectile);
        elapsedTime = 0;
        int[] targetEntity = new[] { enemiesInRange[0].GetComponent<Entity>().entityIndex };
        AttackRPC(attackCapa.indexInCollection, targetEntity, Array.Empty<Vector3>());
        yield return new WaitForSeconds(reloadTime);
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
    // TODO: Delete when TickManager is implemented
    public float respawnDuration = 3;
    private double respawnTimer;
    public GameObject desactivateIcon;
    
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
        return isActive;
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
        isActive = false;
        desactivateIcon.SetActive(true);
    }

    [PunRPC]
    public void DieRPC()
    {
        GameStateMachine.Instance.OnTick += ReActivate;
        photonView.RPC("SyncDieRPC", RpcTarget.All);
    }

    public event GlobalDelegates.NoParameterDelegate OnDie;
    public event GlobalDelegates.NoParameterDelegate OnDieFeedback;

    public void RequestRevive()
    {
        photonView.RPC("ReviveRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void SyncReviveRPC()
    {
        respawnDuration += 3f;
        isActive = true;
        desactivateIcon.SetActive(false);
    }

    [PunRPC]
    public void ReviveRPC()
    {
        photonView.RPC("SyncReviveRPC", RpcTarget.All);
    }

    private void ReActivate()
    {
        respawnTimer += 1 / GameStateMachine.Instance.tickRate;

        if (!(respawnTimer >= respawnDuration)) return;
        GameStateMachine.Instance.OnTick -= ReActivate;
        respawnTimer = 0f;
        RequestRevive();
    }
    
    public event GlobalDelegates.NoParameterDelegate OnRevive;
    public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
}