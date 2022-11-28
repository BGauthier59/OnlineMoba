using Photon.Pun;
using UnityEngine;

namespace GameStates.States
{
    public class PostGameState : GameState
    {
        public PostGameState(GameStateMachine sm) : base(sm) { }

        public override void StartState()
        {
            PostGameUIManager.Instance.DisplayPostGame(sm.winner);
        }

        public override void UpdateState() { }

        public override void ExitState() { }

        public override void OnAllPlayerReady()
        {
            sm.SwitchState(0);
        }
    }
}