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

        public enum lastCapacityUsed
        {
            None,
            AutoAttackCapacity,
            SecondCapacity
        };

        [Header("Attack Logic")] 
        public lastCapacityUsed lastCapacity = lastCapacityUsed.None;
        public NewActiveCapacity autoAttackCapacity;
        public GameObject vfxAutoAttack;
        
        [Space]
        public NewActiveCapacity secondaryCapacity;
        [Range(1, 100)] public int capacityProbability = 20;
        public GameObject vfxStunAttack;
        
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
                currentAttackTarget = null;
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
                    
                    // Decision de l'attaque 
                    
                    if (lastCapacity == lastCapacityUsed.SecondCapacity)
                    {
                        lastCapacity = lastCapacityUsed.AutoAttackCapacity;
                        
                        StartCoroutine(AttackLogic());
                        return;
                    }

                    var rand = Random.Range(1, 101);
                    if (rand > capacityProbability) lastCapacity = lastCapacityUsed.AutoAttackCapacity;
                    else lastCapacity = lastCapacityUsed.SecondCapacity;
                    
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

            if (lastCapacity == lastCapacityUsed.AutoAttackCapacity)
            {
                PhotonNetwork.Instantiate(vfxAutoAttack.name, currentAttackTarget.transform.position, Quaternion.Euler(-90, 0, 0));
                photonView.RPC("SetTriggerAnimation", RpcTarget.MasterClient, "SpinAttack");
                yield return new WaitForSeconds(1f);
                autoAttackCapacity.RequestCastCapacity(uwu, owo); // Lancement de la capacité
                yield return new WaitForSeconds(0.95f);
                photonView.RPC("ResetTriggerAnimation", RpcTarget.MasterClient, "SpinAttack");
            }
            else if (lastCapacity == lastCapacityUsed.SecondCapacity)
            {
                PhotonNetwork.Instantiate(vfxStunAttack.name, currentAttackTarget.transform.position, Quaternion.Euler(-90, 0, 0));
                photonView.RPC("SetTriggerAnimation", RpcTarget.MasterClient, "StunAttack");
                yield return new WaitForSeconds(0.55f);
                secondaryCapacity.RequestCastCapacity(uwu, owo); // Lancement de la capacité
                yield return new WaitForSeconds(0.7f);
                photonView.RPC("ResetTriggerAnimation", RpcTarget.MasterClient, "StunAttack");
            }
            
            yield return new WaitForSeconds(attackSpeed);
            attackCycle = false;
        }
        
        [PunRPC]
        public void SetTriggerAnimation(string animation)
        {
            animator.SetTrigger(animation);
            photonView.RPC("SyncSetTriggerAnimation", RpcTarget.All, animation);
        }
        
        [PunRPC]
        public void SyncSetTriggerAnimation(string animation)
        {
            animator.SetTrigger(animation);
        }
        
        [PunRPC]
        public void ResetTriggerAnimation(string animation)
        {
            animator.ResetTrigger(animation);
            photonView.RPC("SyncSetTriggerAnimation", RpcTarget.All, animation);
        }
        
        [PunRPC]
        public void SyncResetTriggerAnimation(string animation)
        {
            animator.ResetTrigger(animation);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(basePos, wanderRange);
            Gizmos.DrawWireSphere(basePos, attackRange);
        }
    }
}
