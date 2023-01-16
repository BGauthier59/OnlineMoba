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
        public void SyncSetCurrentMoveSpeedRPC(float value)
        {
            currentMoveSpeed = value;
        }

        [PunRPC]
        public void SetCurrentMoveSpeedRPC(float value)
        {
            currentMoveSpeed = value;
            photonView.RPC("SyncSetCurrentMoveSpeedRPC", RpcTarget.Others, currentMoveSpeed);
        }

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

        public float accelerator = 0;
        public Vector3 velocity;
        private void Move()
        {
          
            
            if (!canMove)
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsSliding", false);
                return;
            }

            if (team == Enums.Team.Team2) // Border gauche & droite selon la team
            {
                if (transform.position.x <= -48 &&  moveDirection.x < 0) moveDirection.x = 0f;
            }
            else
            {
                if (transform.position.x >= 48 &&  moveDirection.x > 0) moveDirection.x = 0f;
            }
            
            
            
            //var strength = underStreamEffect ? StreamManager.GetStreamVector(currentStreamModifier, transform) : Vector3.zero;
            var strength = StreamManager.GetStreamVector(currentStreamModifier, transform);

            if (currentStreamModifier != null)
            {
                animator.SetBool("IsSliding", true);
                velocity = new Vector3(
                    Mathf.Lerp(velocity.x, currentMoveSpeed * moveDirection.x, accelerator * Time.deltaTime), 0,
                    Mathf.Lerp(velocity.z, currentMoveSpeed * moveDirection.z, accelerator * Time.deltaTime));
                
                if (strength.x > 3f)
                {
                    velocity.x = Mathf.Clamp(velocity.x, -strength.x/2,10);
                }
                else if (strength.x < -3f)
                {
                    velocity.x = Mathf.Clamp(velocity.x, -10,-strength.x/2);
                }

                if (strength.z > 3f)
                {
                    velocity.z = Mathf.Clamp(velocity.z, -strength.z/2,10);
                }
                else if (strength.z < -3f)
                {
                    velocity.z = Mathf.Clamp(velocity.z, -10,-strength.z/2);
                }
                
                rb.velocity = velocity + strength;
            }
            else
            {
                velocity = moveDirection * currentMoveSpeed;
                rb.velocity = velocity + strength;
            }
            
            
            if (rb.velocity.magnitude == 0)
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsSliding", false);
            }
            
            Debug.DrawRay(transform.position, velocity, Color.green);
            Debug.DrawRay(transform.position, strength, Color.magenta);
            Debug.Log(strength);
        }

        public void RequestCastOnMoveEvent()
        {
            photonView.RPC("CastOnMoveEventRPC", RpcTarget.MasterClient, rb.velocity);
        }

        [PunRPC]
        public void CastOnMoveEventRPC(Vector3 velocity)
        {
            //Debug.Log("Cast OnMove Event");
            OnMove?.Invoke();
        }

        private void RotateMath()
        {
            if (!photonView.IsMine) return;
            if (!canMove) return;
            
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

        public event GlobalDelegates.NoParameterDelegate OnMove;
        public event GlobalDelegates.NoParameterDelegate OnMoveFeedback;
    }
}