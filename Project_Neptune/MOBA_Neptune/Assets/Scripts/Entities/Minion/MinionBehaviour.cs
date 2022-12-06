using Controllers;
using GameStates;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Minion
{
    public partial class MinionBehaviour : Entity 
    {
        #region MinionVariables
        [Space] public NavMeshAgent myAgent;
        public MinionController myController;

        [Header("Pathfinding")] [SerializeField]
        private StreamModifier currentStreamModifier;

        public Transform myWayPoint;

        [Header("Stats")] public float currentHealth;
        public float maxHealth;

        #endregion

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            myAgent = GetComponent<NavMeshAgent>();
            myController = GetComponent<MinionController>();
            currentHealth = maxHealth;
            
            var color = Color.white;
            foreach (var tc in GameStateMachine.Instance.teamColors)
            {
                if (tc.team != (Enums.Team) team) continue;
                color = tc.color;
                break;
            }
            
            GetComponent<MeshRenderer>().material.color = color;
        }

        //------ State Methods

        public void IdleState()
        {
            myAgent.isStopped = true;
        }

        public void WalkingState()
        {
            var strength = StreamManager.GetStreamVector(currentStreamModifier, transform);
            var targetDestination = transform.position + strength;
            myAgent.SetDestination(targetDestination);
        }

        public void LookingForPathingState()
        {
            myAgent.SetDestination(myWayPoint.position);

            if (Vector3.Distance(transform.position, myWayPoint.position) < myAgent.stoppingDistance)
                myController.currentState = MinionController.MinionState.Walking;
        }
    }
}