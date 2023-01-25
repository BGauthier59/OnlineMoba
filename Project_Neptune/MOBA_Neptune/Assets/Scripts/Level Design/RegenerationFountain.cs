using System.Collections.Generic;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

public class RegenerationFountain : MonoBehaviourPun
{
    [SerializeField] private float healPerSecond;
    [SerializeField] private List<Champion> championsInRegenerationFountain;
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!other.CompareTag("Player")) return;
        var champion = other.GetComponent<Champion>();
        championsInRegenerationFountain.Add(champion);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!other.CompareTag("Player")) return;
        var champion = other.GetComponent<Champion>();
        championsInRegenerationFountain.Remove(champion);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (championsInRegenerationFountain.Count == 0) return;

        if (timer >= 1f)
        {
            timer -= 1f;
            foreach (var champion in championsInRegenerationFountain)
            {
                champion.IncreaseCurrentHpRPC(healPerSecond);
            }
        }
        else timer += Time.deltaTime;
    }
}