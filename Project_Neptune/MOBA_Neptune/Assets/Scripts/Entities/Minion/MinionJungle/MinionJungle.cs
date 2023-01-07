using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace Entities.Minion.MinionJungle
{
    public partial class MinionJungle : Entity
    {
        #region Variables

        public enum MinionState
        {
            Idle,
            Attacking
        }

        [Space] [Header("State Machine")] public MinionState currentState = MinionState.Idle;
        public float brainSpeed;
        public Vector3 basePos;
        public LayerMask championMask;

        public enum minionAggroState
        {
            None,
            Champion
        };

        [Header("Attack Logic")] 
        public NewActiveCapacity autoAttackCapacity;
        public int autoAttackDamage;
        
        [Space]
        public NewActiveCapacity secondaryCapacity;
        [Range(1,10)] public float stunRange;
        [Range(0.1f, 6)] public float stunDuration;
        
        public minionAggroState currentAggroState = minionAggroState.None;
        public Entity currentAttackTarget;
        public List<GameObject> whoIsAttackingMe = new List<GameObject>();
        public bool attackCycle;

        [Header("Stats")] 
        public int maxHealth;
        public float currentHealth;
        
        public float attackSpeed;
        [Range(1.5f, 10)] public float attackRange;
        public float wanderRange;
        public CampJungle myCamp;

        // Private variable
        private NavMeshAgent myAgent;
        private float brainTimer;
        #endregion

        public override void OnInstantiated()
        {
            base.OnInstantiated();
        }

        protected override void OnStart()
        {
            // Master client deals with State Machine
            if (!PhotonNetwork.IsMasterClient) return;

            myAgent = GetComponent<NavMeshAgent>();
            currentHealth = maxHealth;
            currentState = MinionState.Attacking;
            basePos = this.transform.position;
        }

        protected override void OnUpdate()
        {
            if (!PhotonNetwork.IsMasterClient) return;

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
                case MinionState.Idle:
                    IdleState();
                    break;
                case MinionState.Attacking:
                    AttackState();
                    break;
            }
        }

        private void IdleState()
        {
            if (Vector3.Distance(transform.position, basePos) > wanderRange) myAgent.SetDestination(basePos);
        }

        private void AttackState()
        {
            if (Vector3.Distance(transform.position, basePos) >= wanderRange)
            {
                currentState = MinionState.Idle;
                return;
            }

            Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, championMask);

            float dist = attackRange;
            foreach (var t in cols)
            {
                var tempDist = Vector3.Distance(transform.position, t.gameObject.transform.position);
                if (tempDist > dist)
                {
                    dist = tempDist;
                    currentAttackTarget = t.gameObject.GetComponent<Entity>();
                }
            }

            if (currentAttackTarget != null)
            {
                if (Vector3.Distance(transform.position, currentAttackTarget.transform.position) > attackRange)
                {
                    myAgent.SetDestination(currentAttackTarget.transform.position);
                }
                else
                {
                    if (attackCycle) return;    
                    myAgent.SetDestination(this.transform.position);
                    // Lancement de la capacité !
                    Debug.Log("Lancement de la section Jungle Mob AA");
                    StartCoroutine(AttackLogic());
                }
            }
            else
            {
                currentState = MinionState.Idle;
            }
        }

        private IEnumerator AttackLogic()
        {
            attackCycle = true;
            
            // Receuil d'info
            int[] uwu = new int[1]; uwu[0] = currentAttackTarget.entityIndex;
            Vector3[] owo = new Vector3[1]; owo[0] = currentAttackTarget.transform.position;
            
            Debug.Log("Lancement de la capacité Jungle Mob AA avec : " + uwu[0] + " comme cible");
            autoAttackCapacity.RequestCastCapacity(uwu, owo); // Lancement de la capacité
            
            yield return new WaitForSeconds(attackSpeed);
            attackCycle = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(basePos, wanderRange);
        }
    }
}
