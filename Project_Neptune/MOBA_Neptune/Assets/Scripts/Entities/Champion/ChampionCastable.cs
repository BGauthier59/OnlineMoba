using System;
using Entities.Capacities;
using UnityEngine;
using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : ICastable
    {
        public byte[] abilitiesIndexes = new byte[2];
        public byte ultimateAbilityIndex;

        public bool canCast;

        public bool CanCast()
        {
            return canCast;
        }

        public void RequestSetCanCast(bool value)
        {
            photonView.RPC("CastRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SetCanCastRPC(bool value)
        {
            canCast = value;
            OnSetCanCast?.Invoke(value);
            photonView.RPC("SyncCastRPC", RpcTarget.All, canCast);
        }

        [PunRPC]
        public void SyncSetCanCastRPC(bool value)
        {
            canCast = value;
            OnSetCanCastFeedback?.Invoke(value);
        }

        public event GlobalDelegates.BoolDelegate OnSetCanCast;
        public event GlobalDelegates.BoolDelegate OnSetCanCastFeedback;

        public void RequestCast(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions,
            int cooldownIndex)
        {
            if (capacityIndex == 255)
            {
                Debug.LogWarning("No ability implemented!");
                return;
            }
            
            photonView.RPC("CastRPC", RpcTarget.MasterClient, capacityIndex, targetedEntities, targetedPositions,
                cooldownIndex);
        }

        [PunRPC]
        public void CastRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions, int cooldownIndex)
        {
            var activeCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);

            if (!activeCapacity.TryCast(entityIndex, targetedEntities, targetedPositions))
            {
                photonView.RPC("SyncCancelCooldown", RpcTarget.All, cooldownIndex);
                return;
            }

            OnCast?.Invoke(capacityIndex, targetedEntities, targetedPositions);
            photonView.RPC("SyncCastRPC", RpcTarget.All, capacityIndex, targetedEntities, targetedPositions);
        }

        [PunRPC]
        public void SyncCancelCooldown(int cooldownIndex)
        {
            if (!photonView.IsMine) return;
            switch (cooldownIndex)
            {
                case 0:
                    controller.canCastAutoAttack = true;
                    break;
                case 1:
                    controller.canCastCapacity1 = true;
                    break;
                case 2:
                    controller.canCastCapacity2 = true;
                    break;
                case 3:
                    controller.canCastUltimate = true;
                    break;
                default:
                    Debug.LogError("Index is not valid.");
                    return;
            }
            Debug.Log("Cancel cooldown.");
        }

        [PunRPC]
        public void SyncCastRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
        {
            var activeCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);
            activeCapacity.PlayFeedback(capacityIndex, targetedEntities, targetedPositions);
            OnCastFeedback?.Invoke(capacityIndex, targetedEntities, targetedPositions, activeCapacity);
        }

        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnCast;
        public event GlobalDelegates.ByteIntArrayVector3ArrayCapacityDelegate OnCastFeedback;
    }
}