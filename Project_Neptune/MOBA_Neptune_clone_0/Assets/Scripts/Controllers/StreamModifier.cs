using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class StreamModifier : MonoBehaviour
    {
        public StreamModifierType streamModifierType;
        public StreamDirection direction;

        public Transform circleStreamCenter;
        public float streamStrength;
    }

    public enum StreamModifierType
    {
        Linear, Circular
    }

    public enum StreamDirection
    {
        LeftToRight, RightToLeft
    }
}