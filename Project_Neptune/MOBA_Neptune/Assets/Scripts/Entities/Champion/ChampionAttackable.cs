using Entities.Capacities;
using UnityEngine;
using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : IAttackable
    {
        public byte attackAbilityIndex;
        public bool canAttack;
        public float attackDamage;

        private byte lastCapacityIndex;
        private int[] lastTargetedEntities;
        private Vector3[] lastTargetedPositions;
        
        public bool CanAttack()
        {
            return canAttack;
        }

        public void RequestSetCanAttack(bool value) { }

        [PunRPC]
        public void SyncSetCanAttackRPC(bool value) { }

        [PunRPC]
        public void SetCanAttackRPC(bool value) { }

        public event GlobalDelegates.BoolDelegate OnSetCanAttack;
        public event GlobalDelegates.BoolDelegate OnSetCanAttackFeedback;
        public float GetAttackDamage() => attackDamage;

        public void RequestSetAttackDamage(float value)
        {
            photonView.RPC("SetAttackDamageRPC",RpcTarget.MasterClient,value);
        }

        public void SetAttackDamageRPC(float value)
        {
            attackDamage = value;
            OnSetAttackDamage?.Invoke(value);
            photonView.RPC("SyncSetAttackDamageRPC",RpcTarget.All,attackDamage);
        }
        
        public void SyncSetAttackDamageRPC(float value)
        {
            attackDamage = value;
            OnSetAttackDamageFeedback?.Invoke(value);
        }
        
        public event GlobalDelegates.FloatDelegate OnSetAttackDamage;
        public event GlobalDelegates.FloatDelegate OnSetAttackDamageFeedback;
        
        

        public void RequestAttack(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
        {
            Debug.Log("Request Attack");
            photonView.RPC("AttackRPC",RpcTarget.MasterClient,capacityIndex,targetedEntities,targetedPositions);
        }

        [PunRPC]
        public void AttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
        {
            lastCapacityIndex = capacityIndex;
            lastTargetedEntities = targetedEntities;
            lastTargetedPositions = targetedPositions;
            
            var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex,this);
            var attackCapacitySO = CapacitySOCollectionManager.GetActiveCapacitySOByIndex(capacityIndex);
            var targetEntity = EntityCollectionManager.GetEntityByIndex(targetedEntities[0]);

            if (attackCapacitySO.shootType != Enums.CapacityShootType.Skillshot)
            {
                if (!attackCapacity.TryCast(entityIndex, targetedEntities, targetedPositions))
                {
                    Debug.Log("Not in range");
                    SendFollowEntity(targetedEntities[0], attackCapacitySO.maxRange);
                }
                else
                {
                    Debug.Log("Attack in Range");
                    agent.SetDestination(transform.position);
                    OnAttack?.Invoke(capacityIndex,targetedEntities,targetedPositions);
                    photonView.RPC("SyncAttackRPC",RpcTarget.All,capacityIndex,targetedEntities,targetedPositions);
                    
                }
            }
            else
            {
                Debug.Log("SkillShot");

                agent.SetDestination(transform.position);
                OnAttack?.Invoke(capacityIndex,targetedEntities,targetedPositions);
                photonView.RPC("SyncAttackRPC",RpcTarget.All,capacityIndex,targetedEntities,targetedPositions);
            }
        }

        void LaunchAttack()
        {
            
        }
        
        [PunRPC]
        public void SyncAttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
        {
            Debug.Log("SyncAttack");
            var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex,this);
            attackCapacity.PlayFeedback(capacityIndex,targetedEntities,targetedPositions);
            OnAttackFeedback?.Invoke(capacityIndex,targetedEntities,targetedPositions);
        }

        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnAttack;
        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnAttackFeedback;
    }
}