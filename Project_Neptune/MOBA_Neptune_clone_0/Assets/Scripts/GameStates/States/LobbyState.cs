using Photon.Pun;
using UnityEngine;

namespace GameStates.States
{
    public class LobbyState : GameState
    {
        public LobbyState(GameStateMachine sm) : base(sm) { }

        public override void StartState()
        {
            LobbyUIManager.Instance.Initialization();

            InputManager.EnablePlayerMap(false);
            InputManager.EnablePlayerUIMap(true);
        }

        public override void UpdateState() { }

        public override void ExitState()
        {
            InputManager.EnablePlayerMap(false);
            InputManager.EnablePlayerUIMap(false);
        }

        public override void OnAllPlayerReady()
        {
            sm.StartCoroutine(sm.StartingGame());
        }
    }
}