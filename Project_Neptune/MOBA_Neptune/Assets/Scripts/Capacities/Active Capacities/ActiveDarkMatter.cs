using Entities;
using Entities.Capacities;
using GameStates;
using UnityEngine;

public class ActiveDarkMatter : ActiveCapacity,IPrevisualisable
{
    private double timer;
    private ActiveDarkMatterSO activeCapacitySo;
    private Vector3[] dir;

    public override bool TryCast(int casterIndex, int[] targets, Vector3[] position)
    {
        base.TryCast(casterIndex, targets, position);
        
        Debug.Log("Performed dark matter at " + Time.time);
        activeCapacitySo = (ActiveDarkMatterSO)AssociatedActiveCapacitySO();
        
       // if (Vector3.Distance(position[0], caster.transform.position) > activeCapacitySo.maxRange){return false;}
        
        GameStateMachine.Instance.OnTick += DelayWaitingTick;
        
        dir = position;

        return true;
    }

    private void ApplyEffect()
    {
        ITeamable casterTeam = caster.GetComponent<ITeamable>();
        
        Collider[] detected = Physics.OverlapSphere(dir[0], activeCapacitySo.zoneRadius);

        foreach (var hit in detected)
        {
            Entity entityTouch = hit.GetComponent<Entity>();

            if (entityTouch)
            {
                ITeamable entityTeam = entityTouch.GetComponent<ITeamable>();

                if (entityTeam.GetTeam() != casterTeam.GetTeam())
                {
                    IActiveLifeable entityActiveLifeable = entityTouch.GetComponent<IActiveLifeable>();

                    entityActiveLifeable.DecreaseCurrentHpRPC( activeCapacitySo.damageAmount);
                }
            }
        }
        
        Debug.Log("Dark Matter is end at " + Time.time);
    }

    private void DelayWaitingTick()
    {
        timer += 1 / GameStateMachine.Instance.tickRate;

        if (timer >=  activeCapacitySo.delay)
        {
            Debug.Log("Delay is over at " + Time.time);
            ApplyEffect();
            GameStateMachine.Instance.OnTick -= DelayWaitingTick;
            timer = 0;
        }
    }
    
    public override void PlayFeedback(int entityIndex, int[] targets, Vector3[] position)
    {
        Debug.Log("Test");
    }

    public void EnableDrawing()
    {
        throw new System.NotImplementedException();
    }

    public void DisableDrawing()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitiateCooldown()
    {
        base.InitiateCooldown();
    }

    protected override void CooldownTimer()
    {
        base.CooldownTimer();
    }
}
