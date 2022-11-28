namespace Entities
{
    public interface IDeadable
    {
        /// <returns>true if the entity is not dead, false if not</returns>
        public bool IsAlive();
        
        /// <returns>true if the entity can die, false if not</returns>
        public bool CanDie();
        /// <summary>
        /// Sends an RPC to the master to set if the entity can die.
        /// </summary>
        public void RequestSetCanDie(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can die.
        /// </summary>
        public void SyncSetCanDieRPC(bool value);
        /// <summary>
        /// Sets if the entity can die.
        /// </summary>
        public void SetCanDieRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanDie;
        public event GlobalDelegates.BoolDelegate OnSetCanDieFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to kill the entity.
        /// </summary>
        public void RequestDie();
        /// <summary>
        /// Sends an RPC to all clients to kill the entity.
        /// </summary>
        public void SyncDieRPC();
        /// <summary>
        /// Kills the entity.
        /// </summary>
        public void DieRPC();

        public event GlobalDelegates.NoParameterDelegate OnDie;
        public event GlobalDelegates.NoParameterDelegate OnDieFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to revive the entity.
        /// </summary>
        public void RequestRevive();
        /// <summary>
        /// Sends an RPC to all clients to revive the entity.
        /// </summary>
        public void SyncReviveRPC();
        /// <summary>
        /// Revives the entity.
        /// </summary>
        public void ReviveRPC();

        public event GlobalDelegates.NoParameterDelegate OnRevive;
        public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
    }
}