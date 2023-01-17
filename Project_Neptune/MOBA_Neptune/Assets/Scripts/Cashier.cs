using Entities;
using Entities.Minion;
using GameStates;
using Photon.Pun;
using UnityEngine;
using MinionStreamBehaviour = Entities.Minion.MinionStream.MinionStreamBehaviour;

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

        if (tempEntity == null) return;

        if (tempEntity.team == teamToGoCashier) CashierRequestIncreaseScore(tempEntity);
    }

    /* ------- CashierIncreaseScore ------- */

    public void CashierRequestIncreaseScore(Entity entityWhoScored)
    {
        Debug.Log(entityWhoScored.name);
        
        _photonView.RPC("CashierIncreaseScoreRPC", RpcTarget.MasterClient, entityWhoScored.currentPointCarried, entityWhoScored.entityIndex);
        

        if (entityWhoScored.GetComponent<MinionStreamBehaviour>())
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
    public void SyncCashierIncreaseScoreRPC(int value, int entityIndex)
    {
        cashierPoint = value;
        UICommonPlayers.Instance.OnScoreChange();
        var entity = EntityCollectionManager.GetEntityByIndex(entityIndex);
        entity.OnScore();
    }

    [PunRPC]
    public void CashierIncreaseScoreRPC(int value, int entityIndex)
    {
        if (value < 1)
        {
            Debug.Log("0 point.");
            return;
        }
        
        cashierPoint += value;

        _photonView.RPC("SyncCashierIncreaseScoreRPC", RpcTarget.All, cashierPoint, entityIndex);

        if (cashierPoint < pointsNeededToWin) return;

        Debug.Log($"Team {teamToGoCashier} won the game !");

        GameStateMachine.Instance.winner = teamToGoCashier; 
    }

    // ----------- Unused Methods ----------- //
    public void ChampionRequestIncreaseScore(int value, Entity entityToInscreasePoints)
    {
    }

    public void SyncChampionIncreaseScore(int value, int entityToInscreasePoints)
    {
    }

    public void ChampionIncreaseScore(int value, int entityToInscreasePoints)
    {
    }

    public void ChampionRequestRemoveScore(Entity entityWhoScored)
    {
    }

    public void SyncChampionRemoveScore(int entityWhoScored)
    {
    }
}