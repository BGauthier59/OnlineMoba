using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Entities;
using Entities.Capacities;
using Entities.Champion;

namespace Controllers.Inputs
{
    public class ChampionInputController : PlayerInputController
    {
        private Champion champion;
        private int[] selectedEntity;
        private Vector3[] cursorWorldPos;
        private bool isMoving;
        private Vector2 mousePos;
        private Vector2 moveInput;
        private Vector3 moveVector;
        private Camera cam;
        private bool isActivebuttonPress;
        public LayerMask groundLayer;
        [SerializeField] private Transform mousePositionSphere;

        public NewActiveCapacity attackCapacity;
        public NewActiveCapacity capacity1;
        public NewActiveCapacity capacity2;
        public NewActiveCapacity ultimateCapacity;

        private void Update()
        {
            if (!photonView.IsMine) return;

            cursorWorldPos[0] = GetMouseOverWorldPos();
            var pos = transform.position;
            pos.y = 1;
            mousePositionSphere.position =
                Vector3.Lerp(mousePositionSphere.position, cursorWorldPos[0], Time.deltaTime * 15);
            Debug.DrawLine(pos, cursorWorldPos[0], Color.black);
        }
        
        private void OnAttack(InputAction.CallbackContext ctx)
        {
            if (!attackCapacity) return;
            
            attackCapacity.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity0(InputAction.CallbackContext ctx)
        {
            if (!capacity1) return;
            
            capacity1.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity1(InputAction.CallbackContext ctx)
        {
            if (!capacity2) return;
            
            capacity2.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateUltimateAbility(InputAction.CallbackContext ctx)
        {
            if (!ultimateCapacity) return;

            ultimateCapacity.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        void OnMouseClick(InputAction.CallbackContext ctx)
        {
        }

        private Vector3 GetMouseOverWorldPos()
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            var point = Physics.Raycast(mouseRay, out var hit, float.PositiveInfinity, groundLayer)
                ? hit.point
                : Vector3.zero;
            point.y = 1;
            return point;
        }

        void OnMoveChange(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            moveVector = new Vector3(moveInput.x, 0, moveInput.y);
            champion.SetMoveDirection(moveVector);
            champion.animator.SetBool("IsRunning", true);
            champion.grabbed.MoveWhileHooked();
        }

        protected override void Link(Entity entity)
        {
            champion = controlledEntity as Champion;
            base.Link(entity);

            cam = Camera.main;
            selectedEntity = new int[1];
            cursorWorldPos = new Vector3[1];
            mousePositionSphere.gameObject.SetActive(true);

            inputs.Attack.Attack.performed += OnAttack;

            inputs.Capacity.Capacity0.performed += OnActivateCapacity0;
            inputs.Capacity.Capacity1.performed += OnActivateCapacity1;
            inputs.Capacity.Capacity2.performed += OnActivateUltimateAbility;

            inputs.Movement.Move.performed += OnMoveChange;
            inputs.Movement.Move.canceled += OnMoveChange;
        }

        protected override void Unlink()
        {
            inputs.Attack.Attack.performed -= OnAttack;

            inputs.Capacity.Capacity0.performed -= OnActivateCapacity0;
            inputs.Capacity.Capacity1.performed -= OnActivateCapacity1;
            inputs.Capacity.Capacity2.performed -= OnActivateUltimateAbility;

            inputs.Movement.Move.performed -= OnMoveChange;
            inputs.Movement.Move.canceled -= OnMoveChange;

            CameraController.Instance.UnLinkCamera();
        }
    }
}