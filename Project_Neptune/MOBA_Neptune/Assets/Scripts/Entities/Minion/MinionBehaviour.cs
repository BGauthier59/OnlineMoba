using Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Minion
{
    public partial class MinionBehaviour : Entity 
    {
        #region MinionVariables
        [Space] public NavMeshAgent myAgent;
        private MinionController myController;

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
        }

        //------ State Methods

        public void IdleState()
        {
            myAgent.isStopped = true;
        }

        public void WalkingState()
        {
            var strength = StreamManager.GetStreamVector(currentStreamModifier, transform);
            Debug.DrawRay(transform.position, strength, Color.magenta);
            Vector3 targetDestination = (transform.position + strength);
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