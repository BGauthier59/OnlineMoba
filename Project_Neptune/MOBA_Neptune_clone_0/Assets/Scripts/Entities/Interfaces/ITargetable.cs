namespace Entities
{
    public interface ITargetable
    {
        /// <returns>true if the entity can be targeted, false if not</returns>
        public bool CanBeTargeted();
        
        /// <summary>
        /// Sends an RPC to the master to set if the entity can be targeted.
        /// </summary>
        public void RequestSetCanBeTargeted(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can be targeted.
        /// </summary>
        public void SyncSetCanBeTargetedRPC(bool value);
        /// <summary>
        /// Sets if the entity can be targeted.
        /// </summary>
        public void SetCanBeTargetedRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanBeTargeted;
        public event GlobalDelegates.BoolDelegate OnSetCanBeTargetedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master that the entity has been targeted.
        /// </summary>
        public void RequestOnTargeted();
        /// <summary>
        /// Sends an RPC to all clients that the entity has been targeted.
        /// </summary>
        public void SyncOnTargetedRPC();
        /// <summary>
        /// What happens when the entity has been targeted.
        /// </summary>
        public void OnTargetedRPC();

        public event GlobalDelegates.NoParameterDelegate OnOnTargeted;
        public event GlobalDelegates.NoParameterDelegate OnOnTargetedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master that the entity has been untargeted.
        /// </summary>
        public void RequestOnUntargeted();
        /// <summary>
        /// Sends an RPC to all clients that the entity has been untargeted.
        /// </summary>
        public void SyncOnUntargetedRPC();
        /// <summary>
        /// What happens when the entity has been untargeted.
        /// </summary>
        public void OnUntargetedRPC();

        public event GlobalDelegates.NoParameterDelegate OnOnUntargeted;
        public event GlobalDelegates.NoParameterDelegate OnOnUntargetedFeedback;
    }
}

