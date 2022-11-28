using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : Controllers.Controller
{
    public enum MinionState { Idle, Walking, LookingForPathing, Attacking }
    public MinionState currentState = MinionState.Idle;
    public float brainSpeed = .7f;
    private float brainTimer;
    private MinionTest myMinionTest;
    
    private void Start()
    {
        myMinionTest = controlledEntity.GetComponent<MinionTest>();
        currentState = MinionState.LookingForPathing;
    }
    
    void Update()
    {
        // Créer des tick pour éviter le saut de frame en plus avec le multi ça risque d'arriver
        brainTimer += Time.deltaTime;
        if (brainTimer >= brainSpeed)
        {
            AiLogic();
            brainTimer = 0;
        }
    }

    private void AiLogic()
    {
        switch (currentState)
        {
            case MinionState.Idle: myMinionTest.IdleState(); break;
            case MinionState.Walking: myMinionTest.WalkingState(); break;
            case MinionState.LookingForPathing: myMinionTest.LookingForPathingState(); break;
            case MinionState.Attacking: myMinionTest.AttackingState(); break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}
