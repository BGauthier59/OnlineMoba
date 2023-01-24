using Controllers;
using Entities.Interfaces;
using GameStates;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Minion.MinionStream
{
    public partial class MinionStreamBehaviour : Entity//, IGrabable, IDisplaceable
    {
        #region MinionVariables

        [Space] public NavMeshAgent myAgent;
        public MinionStreamController myStreamController;

        [Header("Pathfinding")] [SerializeField]
        private StreamModifier currentStreamModifier;

        public Transform myWayPoint;
        
        public MeshRenderer myMeshRenderer;

        #endregion

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }

        [SerializeField] private Material[] minionMat;
        protected override void OnStart()
        {
            base.OnStart();
            myAgent = GetComponent<NavMeshAgent>();
            myStreamController = GetComponent<MinionStreamController>();
            currentHp = maxHp;
            myMeshRenderer.material = team == Enums.Team.Team1 ? minionMat[0] : minionMat[1];
            
            // foreach (var tc in GameStateMachine.Instance.teamColors)
            // {
            //     if (tc.team != team) continue;
            //     break;
            // }
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

        #region Grabable

        public Enums.Team GetGrabbedTeam()
        {
            throw new System.NotImplementedException();
        }

        public void RequestSetCanBeGrabbed(bool canBeGrabbed)
        {
        }

        public void SetCanBeGrabbedRPC(bool canBeGrabbed)
        {
        }

        public void SyncCanBeGrabbedRPC(bool canBeGrabbed)
        {
        }

        public void OnGrabbed()
        {
            myAgent.enabled = false;
        }

        public void SyncOnGrabbedRPC()
        {
        }

        public void OnUnGrabbed()
        {
            myAgent.enabled = true;
        }

        public void SyncOnUnGrabbed()
        {
        }

        #endregion

        #region Displaceable

        public bool CanBeDisplaced()
        {
            throw new System.NotImplementedException();
        }

        public void RequestSetCanBeDisplaced(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetCanBeDisplacedRPC(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SetCanBeDisplacedRPC(bool value)
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplaced;
        public event GlobalDelegates.BoolDelegate OnSetCanBeDisplacedFeedback;

        public void RequestDisplace()
        {
            throw new System.NotImplementedException();
        }

        public void SyncDisplaceRPC()
        {
            throw new System.NotImplementedException();
        }

        public void DisplaceRPC()
        {
            throw new System.NotImplementedException();
        }

        public event GlobalDelegates.NoParameterDelegate OnDisplace;
        public event GlobalDelegates.NoParameterDelegate OnDisplaceFeedback;

        public void SetVelocity(Vector3 value)
        {
            throw new System.NotImplementedException();
        }

        public void SyncSetVelocityRPC(Vector3 value)
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion
}