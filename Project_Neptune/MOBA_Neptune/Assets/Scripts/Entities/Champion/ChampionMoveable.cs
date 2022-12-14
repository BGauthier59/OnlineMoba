using System;
using System.Runtime.CompilerServices;
using Controllers;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;


namespace Entities.Champion
{
    public partial class Champion : IMoveable
    {
        [Header("Movement")] public float referenceMoveSpeed;
        public float currentMoveSpeed;
        public float currentRotateSpeed;
        public bool canMove;
        private Vector3 moveDirection;

        private Vector3 rotateDirection;
        [SerializeField] private LayerMask groundMask;

        public bool CanMove()
        {
            return canMove;
        }

        public float GetReferenceMoveSpeed()
        {
            return referenceMoveSpeed;
        }

        public float GetCurrentMoveSpeed()
        {
            return currentMoveSpeed;
        }

        public void RequestSetCanMove(bool value)
        {
            photonView.RPC("SetCanMoveRPC", RpcTarget.MasterClient, value);
        }

        [PunRPC]
        public void SyncSetCanMoveRPC(bool value)
        {
            canMove = value;
            Debug.Log($"{name} set can move at {canMove}!");
        }

        [PunRPC]
        public void SetCanMoveRPC(bool value)
        {
            canMove = value;
            photonView.RPC("SyncSetCanMoveRPC", RpcTarget.All, canMove);
        }

        public event GlobalDelegates.BoolDelegate OnSetCanMove;
        public event GlobalDelegates.BoolDelegate OnSetCanMoveFeedback;

        public void RequestSetReferenceMoveSpeed(float value) { }

        [PunRPC]
        public void SyncSetReferenceMoveSpeedRPC(float value) { }

        [PunRPC]
        public void SetReferenceMoveSpeedRPC(float value) { }

        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetReferenceMoveSpeedFeedback;

        public void RequestIncreaseReferenceMoveSpeed(float amount) { }

        [PunRPC]
        public void SyncIncreaseReferenceMoveSpeedRPC(float amount) { }

        [PunRPC]
        public void IncreaseReferenceMoveSpeedRPC(float amount) { }

        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseReferenceMoveSpeedFeedback;

        public void RequestDecreaseReferenceMoveSpeed(float amount) { }

        [PunRPC]
        public void SyncDecreaseReferenceMoveSpeedRPC(float amount) { }

        [PunRPC]
        public void DecreaseReferenceMoveSpeedRPC(float amount) { }

        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseReferenceMoveSpeedFeedback;

        public void RequestSetCurrentMoveSpeed(float value) { }

        [PunRPC]
        public void SyncSetCurrentMoveSpeedRPC(float value) { }

        [PunRPC]
        public void SetCurrentMoveSpeedRPC(float value) { }

        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnSetCurrentMoveSpeedFeedback;

        public void RequestIncreaseCurrentMoveSpeed(float amount) { }

        [PunRPC]
        public void SyncIncreaseCurrentMoveSpeedRPC(float amount) { }

        [PunRPC]
        public void IncreaseCurrentMoveSpeedRPC(float amount) { }

        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnIncreaseCurrentMoveSpeedFeedback;

        public void RequestDecreaseCurrentMoveSpeed(float amount) { }

        [PunRPC]
        public void SyncDecreaseCurrentMoveSpeedRPC(float amount) { }

        [PunRPC]
        public void DecreaseCurrentMoveSpeedRPC(float amount) { }

        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeed;
        public event GlobalDelegates.FloatDelegate OnDecreaseCurrentMoveSpeedFeedback;

        private void Move()
        {
            if (!canMove)
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsSliding", false);
                return;
            }

            var velocity = moveDirection * currentMoveSpeed;
            var strength = underStreamEffect ? StreamManager.GetStreamVector(currentStreamModifier, transform) : Vector3.zero;

            if (currentStreamModifier == null) animator.SetBool("IsRunning", true);
            else animator.SetBool("IsSliding", true);


            Debug.DrawRay(transform.position, velocity, Color.green);
            Debug.DrawRay(transform.position, strength, Color.magenta);
            if (velocity + strength == rb.velocity) return;

            rb.velocity = velocity + strength;

            if (rb.velocity.magnitude == 0)
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsSliding", false);
            }
        }

        private void RotateMath()
        {
            if (!photonView.IsMine) return;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, float.PositiveInfinity, groundMask)) return;

            rotateDirection = -(transform.position - hit.point);
            rotateDirection.y = 0;
        }

        private void Rotate()
        {
            rotateParent.transform.rotation = Quaternion.Lerp(rotateParent.transform.rotation,
                Quaternion.LookRotation(rotateDirection),
                Time.fixedDeltaTime * currentRotateSpeed);
        }

        public void SetMoveDirection(Vector3 direction)
        {
            moveDirection = new Vector3(direction.x, 0, direction.z);
        }

        public Vector3 GetMoveDirection()
        {
            return moveDirection;
        }

        public event GlobalDelegates.Vector3Delegate OnMove;
        public event GlobalDelegates.Vector3Delegate OnMoveFeedback;
    }
}