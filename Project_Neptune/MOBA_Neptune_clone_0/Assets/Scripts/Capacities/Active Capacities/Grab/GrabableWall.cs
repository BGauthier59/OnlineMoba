using Entities;
using Entities.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Capacities.Active_Capacities.Grab
{
    public class GrabableWall : Entity, IGrabable
    {
        public Enums.Team GetGrabbedTeam()
        {
            return Enums.Team.Neutral;
        }

        public void RequestSetCanBeGrabbed(bool canBeGrabbed)
        {
            throw new System.NotImplementedException();
        }

        [PunRPC]
        public void SetCanBeGrabbedRPC(bool canBeGrabbed)
        {
            throw new System.NotImplementedException();
        }

        [PunRPC]
        public void SyncCanBeGrabbedRPC(bool canBeGrabbed)
        {
            throw new System.NotImplementedException();
        }

        public void OnGrabbed()
        {
            Debug.Log("Je suis grab mais je suis un mur!");
            photonView.RPC("SyncOnGrabbedRPC", RpcTarget.All);
        }

        [PunRPC]
        public void SyncOnGrabbedRPC()
        {
            // Todo - Implement grab feedbacks
        }

        public void OnUnGrabbed()
        {
            throw new System.NotImplementedException();
        }

        public void SyncOnUnGrabbed()
        {
            throw new System.NotImplementedException();
        }
    }
}