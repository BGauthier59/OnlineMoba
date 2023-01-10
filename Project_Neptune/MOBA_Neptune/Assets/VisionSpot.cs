using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public class VisionSpot : MonoBehaviourPun
{
    public bool isSpotUsable;
    public int timeToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if (!isSpotUsable) return;

        if (other.GetComponent<Champion>())
        {
            RequestDisableClairvoyanceSpot();
            foreach (var t in EntityCollectionManager.AllChampion)
            {
                if (t.team == other.GetComponent<Champion>().team)
                {
                    foreach (var t1 in t.targetIndicators)
                    {
                        t1.RequestDisplayEnemies(t.entityIndex);
                    }
                }
            }
        }
    }

    #region DisableObj

    private void RequestDisableClairvoyanceSpot()
    {
        photonView.RPC("DisableClairvoyanceSpotRpc", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void DisableClairvoyanceSpotRpc()
    {
        // Methods
        StartCoroutine(EnableObj());
        photonView.RPC("SyncDisableClairvoyanceSpotRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SyncDisableClairvoyanceSpotRPC()
    {
        // Methods
        isSpotUsable = false;
        //GetComponent<MeshRenderer>().enabled = false;
    }

    #endregion

    #region EnableObj

    private void RequestEnableClaivoyanceSpot()
    {
        photonView.RPC("EnableClaivoyanceSpotRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void EnableClaivoyanceSpotRPC()
    {
        photonView.RPC("SyncEnableClaivoyanceSpotRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SyncEnableClaivoyanceSpotRPC()
    {
        isSpotUsable = true;
        //GetComponent<MeshRenderer>().enabled = true;
    }

    #endregion


    private IEnumerator EnableObj()
    {
        yield return new WaitForSeconds(timeToEnable);
        RequestEnableClaivoyanceSpot();
    }
}