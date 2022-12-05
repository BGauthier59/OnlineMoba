using Photon.Pun;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : IDisplaceable
    {
        public bool canBeDisplaced;

        public bool CanBeDisplaced()
        {
            return canBeDisplaced;
        }

        public void RequestSetCanBeDisplaced(bool value) { }

        [PunRPC]
        public void SyncSetCanBeDisplacedRPC(bool value) { }

        [PunRPC]
        public void SetCanBeDisplacedRPC(bool value) { }

        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplaced;
        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplacedFeedback;

        public void RequestDisplace() { }

        [PunRPC]
        public void SyncDisplaceRPC() { }

        [PunRPC]
        public void DisplaceRPC() { }

        public event GlobalDelegates.NoParameterDelegate OnDisplace;
        public event GlobalDelegates.NoParameterDelegate OnDisplaceFeedback;
        
        public void SetVelocity(Vector3 value)
        {
            rb.velocity = value;
            photonView.RPC("SyncSetVelocityRPC", RpcTarget.All, rb.velocity);
        }

        [PunRPC]
        public void SyncSetVelocityRPC(Vector3 value)
        {
            rb.velocity = value;
        }
    }
}