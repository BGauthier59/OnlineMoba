using System.Collections.Generic;

namespace Entities.FogOfWar
{
    public interface IFOWViewable
    {
        
        /// <returns>If the entity can see</returns>
        public bool CanView();
        /// <returns>The current view range of the entity</returns>
        public float GetFOWViewRange();
        /// <returns>The base view range of the entity</returns>
        public float GetFOWBaseViewRange();

        public List<IFOWShowable> SeenShowables();

        public void RequestSetCanView(bool value);
        public void SyncSetCanViewRPC(bool value);
        public void SetCanViewRPC(bool value);
        public event GlobalDelegates.BoolDelegate OnSetCanView;
        public event GlobalDelegates.BoolDelegate OnSetCanViewFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to set the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetViewRange(float value);

        /// <summary>
        /// Sends an RPC to all clients to set the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetViewRangeRPC(float value);

        /// <summary>
        /// Sets the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetViewRangeRPC(float value);

        public event GlobalDelegates.FloatDelegate OnSetViewRange;
        public event GlobalDelegates.FloatDelegate OnSetViewRangeFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to set the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void RequestSetViewAngle(float value);

        /// <summary>
        /// Sends an RPC to all clients to set the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SyncSetViewAngleRPC(float value);

        /// <summary>
        /// Sets the entity's view range.
        /// </summary>
        /// <param name="value">the value to set it to</param>
        public void SetViewAngleRPC(float value);

        public event GlobalDelegates.FloatDelegate OnSetViewAngle;
        public event GlobalDelegates.FloatDelegate OnSetViewAngleFeedback;

        
        //Demander à Gauthier et Hubert 
        public void RequestSetBaseViewRange(float value);
        public void SyncSetBaseViewRangeRPC(float value);
        public void SetBaseViewRangeRPC(float value);
        public event GlobalDelegates.FloatDelegate OnSetBaseViewRange;
        public event GlobalDelegates.FloatDelegate OnSetBaseViewRangeFeedback;

        public void AddShowable(int showableIndex);
        public void AddShowable(IFOWShowable showable);
        public void SyncAddShowableRPC(int showableIndex);
        public event GlobalDelegates.IntDelegate OnAddShowable;
        public event GlobalDelegates.IntDelegate OnAddShowableFeedback;
        
        public void RemoveShowable(int showableIndex);
        public void RemoveShowable(IFOWShowable showable);
        public void SyncRemoveShowableRPC(int showableIndex);
        public event GlobalDelegates.IntDelegate OnRemoveShowable;
        public event GlobalDelegates.IntDelegate OnRemoveShowableFeedback;
    }
}