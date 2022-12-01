using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
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
            _myMinionBehaviour = controlledEntity.GetComponent<MinionBehaviour>();
            currentState = MinionState.LookingForPathing;
        }

        void Update()
        {
            // Créer des tick pour éviter le saut de frame en plus avec le multi ça risque d'arriver
            tickTimer += Time.deltaTime;
            if (tickTimer >= updateTickSpeed)
            {
                AiLogic();
                tickTimer = 0;
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