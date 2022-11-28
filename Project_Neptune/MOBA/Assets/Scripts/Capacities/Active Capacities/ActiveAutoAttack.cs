using Entities;
using Entities.Capacities;
using GameStates;
using UnityEngine;

public class ActiveAutoAttack : ActiveCapacity
{
    
    private ActiveAutoAttackSO activeAutoAttackSO; 
    private double attackTimer;
    private int target;

    public override bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
    {
        return base.TryCast(casterIndex, targetsEntityIndexes, targetPositions);
    }

    public override void PlayFeedback(int entityIndex, int[] targets, Vector3[] position)
    {
        Debug.Log("AutoAtk Feedback");
    }

    


    private void ApplyEffect()
    {
        IActiveLifeable activeLifeable = EntityCollectionManager.GetEntityByIndex(target).GetComponent<IActiveLifeable>();
        activeLifeable.DecreaseCurrentHpRPC(activeAutoAttackSO.damage);
    }

    public void DelayAutoAttack()
    {
        attackTimer += GameStateMachine.Instance.tickRate;
        if (attackTimer >= activeAutoAttackSO.attackSpeed)
        {
            attackTimer = 0;
            ApplyEffect();
        }
    }
    
}
