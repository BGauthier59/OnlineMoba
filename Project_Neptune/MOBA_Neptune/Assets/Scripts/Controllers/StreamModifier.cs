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
        [HideInInspector] public float streamStrength;
        public float linearFactor = 60;

        private List<IStreamable> streamablesWaitingInCircle = new List<IStreamable>();

        private void Start()
        {
            streamStrength = StreamManager.Instance.streamStrength;
        }

        private void Update()
        {
            if (streamModifierType == StreamModifierType.Circular) CheckPlayerRelativePosition();
        }

        private void CheckPlayerRelativePosition()
        {
            for (int i = streamablesWaitingInCircle.Count - 1; i >= 0; i--)
            {
                var s = streamablesWaitingInCircle[i];
                
                if (s.GetCurrentStreamModifier() == this) return;

                var streamableReadyToTurn = false;
                switch (circleEntrance)
                {
                    case CircleEntrance.Bottom:
                        if (s.GetCurrentPosition().x <= circleStreamCenter.position.x) streamableReadyToTurn = true;

                        break;
                    case CircleEntrance.Top:
                        if (s.GetCurrentPosition().x >= circleStreamCenter.position.x) streamableReadyToTurn = true;

                        break;
                }

                if (streamableReadyToTurn)
                {
                    s.SetStreamModifier(this);
                    streamablesWaitingInCircle.Remove(s);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var streamable = other.GetComponent<IStreamable>();

            if (streamable == null) return;

            if (streamModifierType == StreamModifierType.Linear) streamable.SetStreamModifier(this);
            else
            {
                streamablesWaitingInCircle.Add(streamable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var streamable = other.GetComponent<IStreamable>();

            if (streamable == null) return;

            if (streamModifierType == StreamModifierType.Circular)
            {
                streamablesWaitingInCircle.Remove(streamable);
            }

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
        Bottom,
        Top
    }
}