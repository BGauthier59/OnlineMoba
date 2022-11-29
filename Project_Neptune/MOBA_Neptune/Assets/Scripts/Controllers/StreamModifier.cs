using System;
using System.Collections.Generic;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Controllers
{
    public class StreamModifier : MonoBehaviour
    {
        public StreamModifierType streamModifierType;
        public StreamDirection direction;
        public CircleEntrance circleEntrance;

        public Transform circleStreamCenter;
        public float streamStrength;
        public float linearFactor = 60;

        private List<IStreamable> entitiesInStream = new List<IStreamable>();

        private void Update()
        {
            if (streamModifierType == StreamModifierType.Circular) CheckPlayerRelativePosition();
        }

        private void CheckPlayerRelativePosition()
        {
            foreach (var s in entitiesInStream)
            {
                if (s.GetCurrentStreamModifier() == this) return;
                
                switch (circleEntrance)
                {
                    case CircleEntrance.Bottom:
                        if(s.GetCurrentPosition().x <= circleStreamCenter.position.x) s.SetStreamModifier(this);
                        break;
                    case CircleEntrance.Top:
                        if(s.GetCurrentPosition().x >= circleStreamCenter.position.x) s.SetStreamModifier(this);
                        break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var streamable = other.GetComponent<IStreamable>();

            if (streamable == null) return;

            if (streamModifierType == StreamModifierType.Linear) streamable.SetStreamModifier(this);
            else entitiesInStream.Add(streamable);
        }

        private void OnTriggerExit(Collider other)
        {
            var streamable = other.GetComponent<IStreamable>();

            if (streamable == null) return;

            if (streamable.GetCurrentStreamModifier() != this) return;
            streamable.SetStreamModifier(null);
        }
    }

    public enum StreamModifierType
    {
        Linear,
        Circular
    }

    public enum StreamDirection
    {
        LeftToRight,
        RightToLeft
    }

    public enum CircleEntrance
    {
        Bottom, Top
    }
}