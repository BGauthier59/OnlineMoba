using System;
using Controllers;
using UnityEngine;

namespace Entities.Interfaces
{
    public interface IStreamable
    {
        public Vector3 GetCurrentPosition();
        public StreamModifier GetCurrentStreamModifier();
        public void SetStreamModifier(StreamModifier modifier);

        public void RequestSetIsUnderStreamEffect(bool value);

        public void SetIsUnderStreamEffectRPC(bool value);

        public void SyncSetIsUnderStreamEffectRPC(bool value);
    }
}
