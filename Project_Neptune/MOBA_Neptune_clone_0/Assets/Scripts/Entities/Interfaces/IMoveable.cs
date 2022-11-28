using UnityEngine;

namespace Entities
{
    public interface IMoveable
    {
        /// <returns>true if the entity can move, false if not</returns>
        public bool CanMove();
        /// <returns>the entity's reference move speed</returns>
        public float GetReferenceMoveSpeed();
        /// <returns>the entity's current move speed</returns>
        public float GetCurrentMoveSpeed();
        
        /// <summary>
        /// Sends an RPC to the master to set if the entity can move.
        /// </summary>
        public void RequestSetCanMove(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can move.
        /// </summary>
        public void SyncSetCanMoveRPC(bool value);
        /// <summary>
        /// Sets if the entity can move.
        /// </summary>
        public void SetCanMoveRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanMove;
        public event GlobalDelegates.BoolDelegate OnSetCanMoveFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to set the entity's reference move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetReferenceMoveSpeed(float value);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's reference move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetReferenceMoveSpeedRPC(float value);
        /// <summary>
        /// Sets the entity's reference move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetReferenceMoveSpeedRPC(float value);

        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to increase the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void RequestIncreaseReferenceMoveSpeed(float amount);
        /// <summary>
        /// Sends an RPC to all clients to increase the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncIncreaseReferenceMoveSpeedRPC(float amount);
        /// <summary>
        /// Increases the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void IncreaseReferenceMoveSpeedRPC(float amount);

        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to decrease the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void RequestDecreaseReferenceMoveSpeed(float amount);
        /// <summary>
        /// Sends an RPC to all clients to decrease the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncDecreaseReferenceMoveSpeedRPC(float amount);
        /// <summary>
        /// Decreases the entity's reference move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void DecreaseReferenceMoveSpeedRPC(float amount);

        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to set the entity's current move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetCurrentMoveSpeed(float value);
        /// <summary>
        /// Sends an RPC to all clients to set the entity's current move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetCurrentMoveSpeedRPC(float value);
        /// <summary>
        /// Sets the entity's current move speed.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetCurrentMoveSpeedRPC(float value);

        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to increase the entity's current move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void RequestIncreaseCurrentMoveSpeed(float amount);
        /// <summary>
        /// Sends an RPC to all clients to increase the entity's current move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncIncreaseCurrentMoveSpeedRPC(float amount);
        /// <summary>
        /// Increases the entity's current move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void IncreaseCurrentMoveSpeedRPC(float amount);

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeedFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to decrease the entity's current move speed.
        /// </summary>
        /// <param name="amount">the decrease amount</param>
        public void RequestDecreaseCurrentMoveSpeed(float amount);
        /// <summary>
        /// Sends an RPC to all clients to decrease the entity's current move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void SyncDecreaseCurrentMoveSpeedRPC(float amount);
        /// <summary>
        /// Decreases the entity's current move speed.
        /// </summary>
        /// <param name="amount">the increase amount</param>
        public void DecreaseCurrentMoveSpeedRPC(float amount);

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeedFeedback;

        public event GlobalDelegates.Vector3Delegate OnMove;
        public event GlobalDelegates.Vector3Delegate OnMoveFeedback;
    }
}