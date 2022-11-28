using System;
using GameStates;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clientDataText;

    private void Start()
    {
        var id = PhotonNetwork.LocalPlayer.ActorNumber;
        clientDataText.text = $"Client {id} / {GameStateMachine.Instance.GetPlayerTeam()}";
    }

    public void OnStartInGameState()
    {
        GameStateMachine.Instance.SwitchState(2);
    }
    
    public void OnTeamWins(int index)
    {
        GameStateMachine.Instance.winner = index == 0 ? Enums.Team.Team1 : Enums.Team.Team2;
    }

    public void OnDieButtonClick()
    {
        GameStateMachine.Instance.GetPlayerChampion().RequestDie();
    }

    public void OnDamageButtonClick()
    {
        GameStateMachine.Instance.GetPlayerChampion().DecreaseCurrentHpRPC(2);
    }
}
