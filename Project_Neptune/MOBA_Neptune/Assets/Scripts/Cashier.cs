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
            CashierRequestIncreaseScore(tempEntity.currentPointCarried, tempEntity);
    }

    /* ------- Scoreable Methods ------- */

    public void CashierRequestIncreaseScore(int value, Entity entityWhoScored)
    {
        _photonView.RPC("SetIncreaseScoreRPC", RpcTarget.MasterClient, value);
        
        if (entityWhoScored.GetComponent<MinionBehaviour>())
        {
            var deadable = entityWhoScored.GetComponent<IDeadable>(); // Tue le sbire
            deadable?.RequestDie();
        }
        else
        {
            var championScoreable = entityWhoScored.GetComponent<IScorable>(); // Retire les points
            championScoreable?.ChampionRequestRemoveScore(entityWhoScored);
        }
    }
    
    [PunRPC]
    public void SyncCashierIncreaseScoreRPC(int value)
    {
        cashierPoint = value;
        UICommonPlayers.Instance.OnScoreChange();
    }

    [PunRPC]
    public void CashierIncreaseScoreRPC(int value)
    {
        cashierPoint += value;

        _photonView.RPC("SyncIncreaseScoreRPC", RpcTarget.All, cashierPoint);

        if (cashierPoint < pointsNeededToWin) return;

        Debug.Log($"Team {teamToGoCashier} won the game !");

        GameStateMachine.Instance.winner = teamToGoCashier;

        // TODO - Faire gagner l'équipe teamToGoCashier
    }

    // ----------- Unused Methods ----------- //
    
    public void ChampionRequestIncreaseScore(int value, Entity entityToInscreasePoints)
    {
    }

    public void SyncChampionIncreaseScore(int value, Entity entityToInscreasePoints)
    {
    }

    public void ChampionIncreaseScore(int value, Entity entityToInscreasePoints)
    {
    }
    
    public void ChampionRequestRemoveScore(Entity entityWhoScored)
    {
    }

    public void SyncChampionRemoveScore(Entity entityWhoScored)
    {
    }
}