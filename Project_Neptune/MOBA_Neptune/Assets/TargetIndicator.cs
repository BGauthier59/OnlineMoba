using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class TargetIndicator : MonoBehaviourPun
{
    public PhotonView photonView;
    public Champion myEntity;
    public List<Vector3> targetPositions;
    public List<GameObject> arrowSprites;
    public bool isFirstIndicator;

    public float hideDistance;
    public int displayTime = 15;
    public bool displayEnemyPos;

    private void Start()
    {
        displayEnemyPos = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayEnemyPos)
        {
            arrowSprites[0].SetActive(false);
            return;
        }

        targetPositions.Clear();

        foreach (var t in EntityCollectionManager.AllChampion)
        {
            if (t.team != myEntity.team)
            {
                targetPositions.Add(t.transform.position);
            }
        }
        
        for (int i = 0; i < targetPositions.Count; i++)
        {
            if (isFirstIndicator)
            {
                if (i == 0)
                {
                    var dir = targetPositions[0] - transform.position;
                    var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
                    arrowSprites[0].SetActive(dir.magnitude > hideDistance);
                }
            }
            else
            {
                if (i == 1)
                {
                    var dir = targetPositions[1] - transform.position;
                    var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
                    arrowSprites[0].SetActive(dir.magnitude > hideDistance);
                }
            }
            Debug.Log(i + "  " + targetPositions[i]);
        }
    }
    
    public void RequestDisplayEnemies(int EntityIndex)
    {
        photonView.RPC("DisplayEnemiesRPC", RpcTarget.MasterClient, EntityIndex);
    }

    [PunRPC] [UsedImplicitly]
    public void DisplayEnemiesRPC(int EntityIndex)
    {   
        photonView.RPC("SyncDisplayEnemiesRPC", RpcTarget.All, EntityIndex);
    }

    [PunRPC] [UsedImplicitly]
    public void SyncDisplayEnemiesRPC(int EntityIndex)
    {
        for (int i = 0; i < EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators.Count; i++)
        {
            EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators[i].displayEnemyPos = true;
        }
        StartCoroutine(CoolDownIndicator(EntityIndex));
    }
    
    public void RequestUndisplayEnemies(int EntityIndex)
    {
        photonView.RPC("UndisplayEnemiesRPC", RpcTarget.MasterClient, EntityIndex);
    }

    [PunRPC] [UsedImplicitly]
    public void UndisplayEnemiesRPC(int EntityIndex)
    {
        photonView.RPC("SyncUndisplayEnemiesRPC", RpcTarget.All, EntityIndex);
    }

    [PunRPC] [UsedImplicitly]
    public void SyncUndisplayEnemiesRPC(int EntityIndex)
    {
        for (int i = 0; i < EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators.Count; i++)
        {
            EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators[i].displayEnemyPos = false;
        }
    }
    
    private IEnumerator CoolDownIndicator(int EntityIndex)
    {
        yield return new WaitForSeconds(displayTime);
        
        for (int i = 0; i < EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators.Count; i++)
        {
            EntityCollectionManager.GetEntityByIndex(EntityIndex).GetComponent<Champion>().targetIndicators[i].RequestUndisplayEnemies(EntityIndex);
        }
    }
}
