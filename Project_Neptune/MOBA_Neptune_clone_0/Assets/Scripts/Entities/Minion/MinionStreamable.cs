using System.Collections;
using System.Collections.Generic;
using Controllers;
using Entities.Interfaces;
using UnityEngine;

namespace Entities.Minion
{
    public partial class MinionBehaviour : IStreamable
    {
        public void OnInstantiatedFeedback()
        {
        }

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