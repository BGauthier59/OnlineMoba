using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : ITargetable
    {
        public bool CanBeTargeted()
        {
            throw new System.NotImplementedException();
        }

        public void RequestSetCanBeTargeted(bool value)
        {
            throw new System.NotImplementedException();
        }

        [PunRPC]
        public void SyncSetCanBeTargetedRPC(bool value) { }

        [PunRPC]
        public void SetCanBeTargetedRPC(bool value) { }

        public event GlobalDelegates.BoolDelegate OnSetCanBeTargeted;
        public event GlobalDelegates.BoolDelegate OnSetCanBeTargetedFeedback;

        public void RequestOnTargeted() { }

        [PunRPC]
        public void SyncOnTargetedRPC() { }

        [PunRPC]
        public void OnTargetedRPC() { }

        public event GlobalDelegates.NoParameterDelegate OnOnTargeted;
        public event GlobalDelegates.NoParameterDelegate OnOnTargetedFeedback;

        public void RequestOnUntargeted() { }

        [PunRPC]
        public void SyncOnUntargetedRPC() { }

        [PunRPC]
        public void OnUntargetedRPC() { }

        public event GlobalDelegates.NoParameterDelegate OnOnUntargeted;
        public event GlobalDelegates.NoParameterDelegate OnOnUntargetedFeedback;
    }
}