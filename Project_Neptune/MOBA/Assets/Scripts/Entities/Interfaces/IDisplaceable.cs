namespace Entities
{
    public interface IDisplaceable
    {
        /// <returns>true if the entity can be displaced, false if not</returns>
        public bool CanBeDisplaced();
        /// <summary>
        /// Sends an RPC to the master to set if the entity can be displaced.
        /// </summary>
        public void RequestSetCanBeDisplaced(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can be displaced.
        /// </summary>
        public void SyncSetCanBeDisplacedRPC(bool value);
        /// <summary>
        /// Sets if the entity can be displaced.
        /// </summary>
        public void SetCanBeDisplacedRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplaced;
        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplacedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to displace the entity.
        /// </summary>
        public void RequestDisplace();
        /// <summary>
        /// Sends an RPC to all clients to displace the entity.
        /// </summary>
        public void SyncDisplaceRPC();
        /// <summary>
        /// Displaces the entity.
        /// </summary>
        public void DisplaceRPC();

        public event GlobalDelegates.NoParameterDelegate OnDisplace;
        public event GlobalDelegates.NoParameterDelegate OnDisplaceFeedback;
    }
}