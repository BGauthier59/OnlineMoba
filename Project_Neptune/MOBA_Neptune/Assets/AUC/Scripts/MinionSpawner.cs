using System.Collections;
using System.Collections.Generic;
using Entities;
using Photon.Pun;
using UnityEngine;

public partial class MinionSpawner : Building
{
    public Transform spawnPointForMinion;
    public Entity minionPrefab;
    public int spawnMinionAmount = 5;
    public float spawnMinionInterval = 1.7f;
    public float spawnCycleTime = 30;
    private readonly float spawnSpeed = 30;
    public Color minionColor;
    public List<Transform> pathfinding = new List<Transform>();
    public List<Building> enemyTowers = new List<Building>();
    public string unitTag;
    
    private void Update()
    {
        // Spawn de minion tous les spawnCycleTime secondes
        spawnCycleTime += Time.deltaTime;
        if (spawnCycleTime >= spawnSpeed)
        {
            StartCoroutine(SpawnMinionCo());
            spawnCycleTime = 0;
        }
    }
    
    private IEnumerator SpawnMinionCo()
    {
        for (int i = 0; i < spawnMinionAmount; i++)
        {
            SpawnMinion();
            yield return new WaitForSeconds(spawnMinionInterval);
        }
    }

    private void SpawnMinion()
    {
        Entity minionGO = PoolNetworkManager.Instance.PoolInstantiate(minionPrefab, spawnPointForMinion.position, Quaternion.identity, transform.root.root.root.root);
        
        MinionTest minionScript = minionGO.GetComponent<MinionTest>();
        minionScript.myWaypoints = pathfinding;
        minionScript.TowersList = enemyTowers;
        minionScript.tag = unitTag;
        minionGO.GetComponent<MeshRenderer>().material.color = minionColor;
    }
}


public partial class MinionSpawner : IActiveLifeable, IDeadable
{
    public float GetMaxHp()
    {
        throw new System.NotImplementedException();
    }

    public float GetCurrentHp()
    {
        throw new System.NotImplementedException();
    }

    public float GetCurrentHpPercent()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetMaxHp(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetMaxHpRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetMaxHpRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetMaxHp;
    public event GlobalDelegates.FloatDelegate OnSetMaxHpFeedback;
    public void RequestIncreaseMaxHp(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncIncreaseMaxHpRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseMaxHpRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnIncreaseMaxHp;
    public event GlobalDelegates.FloatDelegate OnIncreaseMaxHpFeedback;
    public void RequestDecreaseMaxHp(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncDecreaseMaxHpRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseMaxHpRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseMaxHp;
    public event GlobalDelegates.FloatDelegate OnDecreaseMaxHpFeedback;
    public void RequestSetCurrentHp(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCurrentHpRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentHpRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetCurrentHp;
    public event GlobalDelegates.FloatDelegate OnSetCurrentHpFeedback;
    public void RequestSetCurrentHpPercent(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCurrentHpPercentRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentHpPercentRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercent;
    public event GlobalDelegates.FloatDelegate OnSetCurrentHpPercentFeedback;
    public void RequestIncreaseCurrentHp(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void SyncIncreaseCurrentHpRPC(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseCurrentHpRPC(float amount)
    {
        throw new System.NotImplementedException();
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
        
        if (currentHealth <= 0 && isAlive)
        {
            RequestDie();
            isAlive = false;
        }
    }

    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHp;
    public event GlobalDelegates.FloatDelegate OnDecreaseCurrentHpFeedback;
    public bool IsAlive()
    {
        throw new System.NotImplementedException();
    }

    public bool CanDie()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetCanDie(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCanDieRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCanDieRPC(bool value)
    {
        throw new System.NotImplementedException();
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
        Debug.Log("Equipe : " + team + " won");
        // Add Method Win
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
        throw new System.NotImplementedException();
    }

    public void SyncReviveRPC()
    {
        throw new System.NotImplementedException();
    }

    public void ReviveRPC()
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.NoParameterDelegate OnRevive;
    public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
}
