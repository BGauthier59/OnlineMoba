using Entities;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : IMoveable
    {
        public bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        public float GetReferenceMoveSpeed()
        {
            throw new System.NotImplementedException();
        }

        public float GetCurrentMoveSpeed()
        {
            throw new System.NotImplementedException();
        }

        public void RequestSetCanMove(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetCanMoveRPC(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SetCanMoveRPC(bool value)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.BoolDelegate OnSetCanMove;
        public event GlobalDelegates.BoolDelegate OnSetCanMoveFeedback;

        public void RequestSetReferenceMoveSpeed(float value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetReferenceMoveSpeedRPC(float value)
        {
            throw new System.NotImplementedException();
        }

        public void SetReferenceMoveSpeedRPC(float value)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeedFeedback;

        public void RequestIncreaseReferenceMoveSpeed(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void SyncIncreaseReferenceMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseReferenceMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeedFeedback;

        public void RequestDecreaseReferenceMoveSpeed(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void SyncDecreaseReferenceMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void DecreaseReferenceMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeedFeedback;

        public void RequestSetCurrentMoveSpeed(float value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetCurrentMoveSpeedRPC(float value)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentMoveSpeedRPC(float value)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeedFeedback;

        public void RequestIncreaseCurrentMoveSpeed(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void SyncIncreaseCurrentMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseCurrentMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeedFeedback;

        public void RequestDecreaseCurrentMoveSpeed(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void SyncDecreaseCurrentMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void DecreaseCurrentMoveSpeedRPC(float amount)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeedFeedback;
        public event GlobalDelegates.NoParameterDelegate OnMove;
        public event GlobalDelegates.NoParameterDelegate OnMoveFeedback;
    }
}