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
        public Vector3[] cursorWorldPos;
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

        private void OnPreviewAttack(InputAction.CallbackContext ctx)
        {
            if (!attackCapacity) return;

            attackCapacity.RequestSetPreview(true);
        }

        private void OnPreviewCapacity0(InputAction.CallbackContext ctx)
        {
            if (!capacity1) return;
            capacity1.RequestSetPreview(true);
        }

        private void OnPreviewCapacity1(InputAction.CallbackContext ctx)
        {
            if (!capacity2) return;
            capacity2.RequestSetPreview(true);
        }

        private void OnPreviewUltimate(InputAction.CallbackContext ctx)
        {
            if (!ultimateCapacity) return;
            ultimateCapacity.RequestSetPreview(true);
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

        void OnMouseClick(InputAction.CallbackContext ctx) { }

        private Vector3 GetMouseOverWorldPos()
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(mouseRay, out var hit, float.PositiveInfinity, groundLayer))
            {
                if (selectedEntity[0] == 0) return Vector3.zero;
                var ent = EntityCollectionManager.GetEntityByIndex(selectedEntity[0]);
                if(ent.outline) ent.outline.enabled = false;
                selectedEntity[0] = 0;
                return Vector3.zero;
            }
            
            var point = hit.point;
            point.y = 1;
            GetEntityOnMouse(hit);
            return point;
        }

        private void GetEntityOnMouse(RaycastHit hit)
        {
            var entity = hit.transform.GetComponent<Entity>();
            if (!entity || entity == champion)
            {
                if (selectedEntity[0] == 0) return;
                var ent = EntityCollectionManager.GetEntityByIndex(selectedEntity[0]);
                if(ent.outline) ent.outline.enabled = false;
                selectedEntity[0] = 0;
                return;
            }
            selectedEntity[0] = entity.entityIndex;
            if (!entity.outline) return;
            entity.outline.enabled = true;
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

            inputs.Attack.Attack.performed += OnPreviewAttack;
            inputs.Capacity.Capacity0.performed += OnPreviewCapacity0;
            inputs.Capacity.Capacity1.performed += OnPreviewCapacity1;
            inputs.Capacity.Capacity2.performed += OnPreviewUltimate;

            inputs.Attack.Attack.canceled += OnAttack;
            inputs.Capacity.Capacity0.canceled += OnActivateCapacity0;
            inputs.Capacity.Capacity1.canceled += OnActivateCapacity1;
            inputs.Capacity.Capacity2.canceled += OnActivateUltimateAbility;

            inputs.Movement.Move.performed += OnMoveChange;
            inputs.Movement.Move.canceled += OnMoveChange;
        }

        protected override void Unlink()
        {
            inputs.Attack.Attack.performed -= OnPreviewAttack;
            inputs.Capacity.Capacity0.performed -= OnPreviewCapacity0;
            inputs.Capacity.Capacity1.performed -= OnPreviewCapacity1;
            inputs.Capacity.Capacity2.performed -= OnPreviewUltimate;

            inputs.Attack.Attack.canceled -= OnAttack;
            inputs.Capacity.Capacity0.canceled -= OnActivateCapacity0;
            inputs.Capacity.Capacity1.canceled -= OnActivateCapacity1;
            inputs.Capacity.Capacity2.canceled -= OnActivateUltimateAbility;

            inputs.Movement.Move.performed -= OnMoveChange;
            inputs.Movement.Move.canceled -= OnMoveChange;

            CameraController.Instance.UnLinkCamera();
        }
    }
}