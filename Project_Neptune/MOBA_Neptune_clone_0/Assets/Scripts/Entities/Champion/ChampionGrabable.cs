using Entities.Interfaces;
using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : IGrabable
    {
        public bool canBeGrabbed;

        public Enums.Team GetGrabbedTeam()
        {
            return team;
        }

        public void RequestSetCanBeGrabbed(bool canBeGrabbed)
        {
            photonView.RPC("SetCanBeGrabbedRPC", RpcTarget.MasterClient, canBeGrabbed);
        }

        [PunRPC]
        public void SetCanBeGrabbedRPC(bool canBeGrabbed)
        {
            this.canBeGrabbed = canBeGrabbed;
            photonView.RPC("SyncCanBeGrabbedRPC", RpcTarget.All, this.canBeGrabbed);
        }

        [PunRPC]
        public void SyncCanBeGrabbedRPC(bool canBeGrabbed)
        {
            this.canBeGrabbed = canBeGrabbed;
        }

        public void OnGrabbed()
        {
            photonView.RPC("SyncOnGrabbedRPC", RpcTarget.All);
        }

        [PunRPC]
        public void SyncOnGrabbedRPC()
        {
        }
    }
}
