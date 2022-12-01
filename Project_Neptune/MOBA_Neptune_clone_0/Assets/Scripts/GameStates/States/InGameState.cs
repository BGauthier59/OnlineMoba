using Photon.Pun;
using UnityEngine;

namespace GameStates.States
{
    public class InGameState : GameState
    {
        public double lastTickTime;
        public InGameState(GameStateMachine sm) : base(sm) { }

        private double timer;

        public override void StartState()
        {
            InputManager.EnablePlayerMap(true);
            lastTickTime = PhotonNetwork.Time;
        }

        public override void UpdateState()
        {
            if (!sm.IsMaster) return;

            if (IsWinConditionChecked())
            {
                sm.SendWinner(sm.winner);
                sm.SwitchState(3);
                return;
            }

            timer = PhotonNetwork.Time - lastTickTime;

            if (timer >= 1.0 / sm.tickRate)
            {
                sm.Tick();
                timer = PhotonNetwork.Time;
            }
            else timer += Time.deltaTime;
        }

        public override void ExitState() { }

        public override void OnAllPlayerReady() { }

        private bool IsWinConditionChecked()
        {
            // Check win condition for any team
            return sm.winner != Enums.Team.Neutral;
        }
    }
}