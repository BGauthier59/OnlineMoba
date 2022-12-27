using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion.MinionStream
{
    public class MinionStreamController : Controller
    {
        public enum MinionState
        {
            Idle,
            Walking,
            LookingForPathing
        }

        public MinionState currentState = MinionState.Idle;
        public float updateTickSpeed = .5f;
        private float tickTimer;
        private MinionStream.MinionStreamBehaviour _myMinionStreamBehaviour;

        private void Start()
        {
            // Master client deals with State Machine
            if (!PhotonNetwork.IsMasterClient) return;
            
            _myMinionStreamBehaviour = controlledEntity.GetComponent<MinionStream.MinionStreamBehaviour>();
            currentState = MinionState.LookingForPathing;
        }

        void Update()
        {
            // Master client deals with State Machine
            if (!PhotonNetwork.IsMasterClient) return;
            
            tickTimer += Time.deltaTime;
            if (tickTimer >= updateTickSpeed)
            {
                AiLogic();
                tickTimer -= updateTickSpeed;
            }
        }

        private void AiLogic()
        {
            switch (currentState)
            {
                case MinionState.Idle:
                    _myMinionStreamBehaviour.IdleState();
                    break;
                case MinionState.Walking:
                    _myMinionStreamBehaviour.WalkingState();
                    break;
                case MinionState.LookingForPathing:
                    _myMinionStreamBehaviour.LookingForPathingState();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}