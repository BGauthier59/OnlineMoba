using UnityEngine;

namespace Entities
{
    public interface ICastable
    {
        /// <returns>true if the entity can cast active capacities, false if not</returns>
        public bool CanCast();
        /// <summary>
        /// Sends an RPC to the master to set if the entity can cast active capacities.
        /// </summary>
        public void RequestSetCanCast(bool value);
        /// <summary>
        /// Sets if the entity can cast active capacities.
        /// </summary>
        public void SetCanCastRPC(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can cast active capacities.
        /// </summary>
        public void SyncSetCanCastRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanCast;
        public event GlobalDelegates.BoolDelegate OnSetCanCastFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to cast an ActiveCapacity.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to cast</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void RequestCast(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions);
        /// <summary>
        /// Casts an ActiveCapacity.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to cast</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void CastRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions);
        /// <summary>
        /// Sends an RPC to all clients to cast an ActiveCapacity.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to cast</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void SyncCastRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions);
        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnCast;
        public event GlobalDelegates.ByteIntArrayVector3ArrayCapacityDelegate OnCastFeedback;
    }
}