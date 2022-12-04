using Entities;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class Cashier : MonoBehaviour, IScorable
{
    private PhotonView _photonView;
    [SerializeField] private int cashierPoint;
    [SerializeField] private int pointsNeededToWin;
    public Enums.Team teamToGoCashier;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity tempEntity = other.gameObject.GetComponent<Entity>();

        if (!tempEntity) return;

        if (tempEntity.team == teamToGoCashier)
            RequestIncreaseScore(tempEntity.currentPointCarried);
    }

    //------ IScorable Methods

    public void RequestIncreaseScore(int value)
    {
        _photonView.RPC("SetIncreaseScoreRPC", RpcTarget.MasterClient, value);
    }

    public void SyncIncreaseScoreRPC(int value)
    {
        cashierPoint = value;
    }

    public void SetIncreaseScoreRPC(int value)
    {
        cashierPoint += value;

        _photonView.RPC("SyncIncreaseScoreRPC", RpcTarget.All, value);

        if (cashierPoint < pointsNeededToWin) return;

        Debug.Log($"Team {teamToGoCashier} won the game !");

        GameStateMachine.Instance.winner = teamToGoCashier;

        // TODO - Faire gagner l'équipe teamToGoCashier
    }
}