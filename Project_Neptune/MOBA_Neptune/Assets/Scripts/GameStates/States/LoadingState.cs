using Photon.Pun;
using UnityEngine;

namespace GameStates.States
{
    public class LoadingState : GameState
    {
        public LoadingState(GameStateMachine sm) : base(sm) { }

        public override void StartState()
        {
            if (!sm.IsMaster) return;
            sm.MoveToGameScene();
            sm.winner = Enums.Team.Neutral;
        }

        public override void UpdateState() { }

        public override void ExitState()
        {
            sm.LateLoad();
        }

        public override void OnAllPlayerReady()
        {
          
            //sm.SwitchState(2);
        }
    }
}