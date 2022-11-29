using Controllers;
using Entities.Interfaces;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : IStreamable
    {
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
    }
}
