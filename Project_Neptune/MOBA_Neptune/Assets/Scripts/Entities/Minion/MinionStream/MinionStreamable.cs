using System.Collections;
using System.Collections.Generic;
using Controllers;
using Entities.Interfaces;
using UnityEngine;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : IStreamable
    {
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
            throw new System.NotImplementedException();
        }

        public void SetIsUnderStreamEffectRPC(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetIsUnderStreamEffectRPC(bool value)
        {
            throw new System.NotImplementedException();
        }
    }
}