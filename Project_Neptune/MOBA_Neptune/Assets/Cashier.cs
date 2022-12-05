using Entities;
using Entities.Minion;
using GameStates;
using Photon.Pun;
using UnityEngine;

public class Cashier : MonoBehaviour, IScorable
{
    private PhotonView _photonView;
    public int cashierPoint;
    public int pointsNeededToWin;
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
            RequestIncreaseScore(tempEntity.currentPointCarried, tempEntity);
    }

    //------ IScorable Methods

    public void RequestIncreaseScore(int value, Entity entityWhoScored)
    {
        _photonView.RPC("SetIncreaseScoreRPC", RpcTarget.MasterClient, value);

        if (!entityWhoScored.GetComponent<MinionBehaviour>()) return; // Si c'est un creep alors,
        
        IDeadable deadable = entityWhoScored.GetComponent<IDeadable>(); // Tue
        deadable?.RequestDie();
    }
    
    [PunRPC]
    public void SyncIncreaseScoreRPC(int value)
    {
        cashierPoint = value;
        UICommonPlayers.Instance.OnScoreChange();
    }

    [PunRPC]
    public void SetIncreaseScoreRPC(int value)
    {
        cashierPoint += value;

        _photonView.RPC("SyncIncreaseScoreRPC", RpcTarget.All, cashierPoint);

        if (cashierPoint < pointsNeededToWin) return;

        Debug.Log($"Team {teamToGoCashier} won the game !");

        GameStateMachine.Instance.winner = teamToGoCashier;

        // TODO - Faire gagner l'équipe teamToGoCashier
    }
}