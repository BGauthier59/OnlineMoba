namespace Entities
{
    public interface IResourceable
    {
        /// <returns>The maxResource of the entity</returns>
        public float GetMaxResource();
        /// <returns>The currentResource of the entity</returns>
        public float GetCurrentResource();
        /// <returns>The percentage of currentResource on maxResource of the entity</returns>
        public float GetCurrentResourcePercent();
        
        /// /// <summary>
        /// Sends an RPC to the master to set the entity's maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetMaxResource(float value);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetMaxResourceRPC(float value);
        /// <summary>
        /// Sets the entity's maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetMaxResourceRPC(float value);

        public event GlobalDelegates.FloatDelegate OnSetMaxResource;
        public event GlobalDelegates.FloatDelegate OnSetMaxResourceFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to increase the entity's maxResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void RequestIncreaseMaxResource(float amount);
        /// <summary>
        /// Sends an RPC to all clients to increase the entity's maxResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncIncreaseMaxResourceRPC(float amount);
        /// <summary>
        /// Increases the entity's maxResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void IncreaseMaxResourceRPC(float amount);
        public event GlobalDelegates.FloatDelegate OnIncreaseMaxResource;
        public event GlobalDelegates.FloatDelegate OnIncreaseMaxResourceFeedback;

        /// <summary>
        /// Sends an RPC to the master to decrease the entity's maxResource.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void RequestDecreaseMaxResource(float amount);
        /// <summary>
        /// Sends an RPC to all clients to decrease the entity's maxResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncDecreaseMaxResourceRPC(float amount);
        /// <summary>
        /// Decreases the entity's maxResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void DecreaseMaxResourceRPC(float amount);
        public event GlobalDelegates.FloatDelegate OnDecreaseMaxResource;
        public event GlobalDelegates.FloatDelegate OnDecreaseMaxResourceFeedback;

        /// <summary>
        /// Sends an RPC to the master to set the entity's currentResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetCurrentResource(float value);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's currentResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetCurrentResourceRPC(float value);
        /// <summary>
        /// Set the entity's currentResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetCurrentResourceRPC(float value);
        public event GlobalDelegates.FloatDelegate OnSetCurrentResource;
        public event GlobalDelegates.FloatDelegate OnSetCurrentResourceFeedback;

        /// <summary>
        /// Sends an RPC to the master to set the entity's currentResource to a percentage of  its maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetCurrentResourcePercent(float value);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's currentResource to a percentage of  its maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetCurrentResourcePercentRPC(float value);
        /// <summary>
        /// Sets the entity's currentResource to a percentage of its maxResource.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetCurrentResourcePercentRPC(float value);
        public event GlobalDelegates.FloatDelegate OnSetCurrentResourcePercent;
        public event GlobalDelegates.FloatDelegate OnSetCurrentResourcePercentFeedback;

        /// <summary>
        /// Sends an RPC to the master to increase the entity's currentResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void RequestIncreaseCurrentResource(float amount);
        /// <summary>
        /// Sends an RPC to all clients to increase the entity's currentResource.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncIncreaseCurrentResourceRPC(float amount);
        /// <summary>
        /// Increases the entity's currentResource.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void IncreaseCurrentResourceRPC(float amount);
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentResource;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentResourceFeedback;

        /// <summary>
        /// Sends an RPC to the master to decrease the entity's currentResource.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void RequestDecreaseCurrentResource(float amount);
        /// <summary>
        /// Sends an RPC to all clients to decrease the entity's currentResource.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void SyncDecreaseCurrentResourceRPC(float amount);
        /// <summary>
        /// Decreases the entity's currentResource.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void DecreaseCurrentResourceRPC(float amount);
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentResource;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentResourceFeedback;

    }
}