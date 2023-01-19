using Entities;
using GameStates;
using JetBrains.Annotations;
using Photon.Pun;
using TMPro;
using UnityEngine;
using MinionStreamBehaviour = Entities.Minion.MinionStream.MinionStreamBehaviour;

public class Cashier : MonoBehaviour, IScorable
{
    private PhotonView _photonView;
    public int cashierPoint;
    public int pointsNeededToWin;
    public Enums.Team teamToGoCashier;

    public Animation scoreVFX;
    public ParticleSystem confettiVFX;
    public TextMeshPro scoreText;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity tempEntity = other.gameObject.GetComponent<Entity>();

        if (tempEntity == null) return;

        if (tempEntity.team == teamToGoCashier) CashierRequestIncreaseScore(tempEntity, tempEntity.currentPointCarried);
    }

    /* ------- CashierIncreaseScore ------- */

    public void CashierRequestIncreaseScore(Entity entityWhoScored, int value)
    {
        if (value == 0) return;

        _photonView.RPC("CashierIncreaseScoreRPC", RpcTarget.MasterClient, value);

        if (entityWhoScored.GetComponent<MinionStreamBehaviour>())
        {
            var deadable = entityWhoScored.GetComponent<IDeadable>(); // Tue le sbire
            deadable?.RequestDie();
        }
        else
        {
            var championScoreable = entityWhoScored.GetComponent<IScorable>(); // Retire les points
            championScoreable?.ChampionRequestRemoveScore(entityWhoScored);
            _photonView.RPC("SyncPlayerGoaledRPC", RpcTarget.All, value);
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

        _photonView.RPC("SyncCashierIncreaseScoreRPC", RpcTarget.All, cashierPoint);

        if (cashierPoint < pointsNeededToWin) return;

        Debug.Log($"Team {teamToGoCashier} won the game !");

        GameStateMachine.Instance.winner = teamToGoCashier;
    }

    [PunRPC] [UsedImplicitly]
    public void SyncPlayerGoaledRPC(int value)
    {
        scoreText.text = value.ToString();
        scoreVFX.PlayQueued("A_Scoring", QueueMode.CompleteOthers);
        confettiVFX.Play();
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