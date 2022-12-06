using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;

namespace Entities.Minion
{
    public class MinionController : Controller
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
        private MinionBehaviour _myMinionBehaviour;

        private void Start()
        {
            // Master client deals with State Machine
            if (!PhotonNetwork.IsMasterClient) return;
            
            _myMinionBehaviour = controlledEntity.GetComponent<MinionBehaviour>();
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
                    _myMinionBehaviour.IdleState();
                    break;
                case MinionState.Walking:
                    _myMinionBehaviour.WalkingState();
                    break;
                case MinionState.LookingForPathing:
                    _myMinionBehaviour.LookingForPathingState();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}