using Controllers;
using Entities.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : IStreamable
    {
        public bool underStreamEffect;
        public StreamModifier currentStreamModifier;

        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        public StreamModifier GetCurrentStreamModifier()
        {
            return currentStreamModifier;
        }

        public void SetStreamModifier(StreamModifier modifier)
        {
            currentStreamModifier = modifier;
            
        }

        public void RequestSetIsUnderStreamEffect(bool value)
        {
            photonView.RPC("SetIsUnderStreamEffectRPC", RpcTarget.MasterClient, value);
        }
        
        [PunRPC]
        public void SetIsUnderStreamEffectRPC(bool value)
        {
            underStreamEffect = value;
            photonView.RPC("SyncSetIsUnderStreamEffectRPC", RpcTarget.All, underStreamEffect);
        }

        [PunRPC]
        public void SyncSetIsUnderStreamEffectRPC(bool value)
        {
            underStreamEffect = value;
            
        }
    }
}