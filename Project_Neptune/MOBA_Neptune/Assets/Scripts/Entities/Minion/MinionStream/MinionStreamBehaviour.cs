using Controllers;
using GameStates;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : Entity
    {
        #region MinionVariables

        [Space] public NavMeshAgent myAgent;
        public MinionStreamController myStreamController;

        [Header("Pathfinding")] [SerializeField]
        private StreamModifier currentStreamModifier;

        public Transform myWayPoint;

        [Header("Stats")] public float currentHealth;
        public float maxHealth;
        public MeshRenderer myMeshRenderer;
        #endregion

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }

        protected override void OnStart()
        {
            base.OnStart();
            myAgent = GetComponent<NavMeshAgent>();
            myStreamController = GetComponent<MinionStreamController>();
            currentHealth = maxHealth;

            foreach (var tc in GameStateMachine.Instance.teamColors)
            {
                if (tc.team != team) continue;
                myMeshRenderer.materials[1].SetColor("_EmissionColor", tc.color);
                break;
            }
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
                myStreamController.currentState = MinionStreamController.MinionState.Walking;
        }
    }
}