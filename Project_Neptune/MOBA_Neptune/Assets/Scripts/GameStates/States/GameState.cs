namespace GameStates.States
{
    public abstract class GameState
    {
        protected GameStateMachine sm;

        public GameState(GameStateMachine sm)
        {
            this.sm = sm;
        }

        public abstract void StartState();

        public abstract void UpdateState();

        public abstract void ExitState();

        public abstract void OnAllPlayerReady();
    }
}