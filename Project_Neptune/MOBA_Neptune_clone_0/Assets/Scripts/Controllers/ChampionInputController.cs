using UnityEngine;
using UnityEngine.InputSystem;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using UnityEngine.AI;

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
        
        private void OnAttack(InputAction.CallbackContext ctx)
        {
            if (champion.attackAbilityIndex == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            champion.RequestAttack(champion.attackAbilityIndex, selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity0(InputAction.CallbackContext ctx)
        {
            if (champion.abilitiesIndexes[0] == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            champion.RequestCast(champion.abilitiesIndexes[0], selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity1(InputAction.CallbackContext ctx)
        {
            if (champion.abilitiesIndexes[1] == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            champion.RequestCast(champion.abilitiesIndexes[1], selectedEntity, cursorWorldPos);
        }

        private void OnActivateUltimateAbility(InputAction.CallbackContext ctx)
        {
            if (champion.ultimateAbilityIndex == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            champion.RequestCast(champion.ultimateAbilityIndex, selectedEntity, cursorWorldPos);
        }


        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            // Todo - set selected entity
            cursorWorldPos[0] = GetMouseOverWorldPos();
        }

        void OnMouseClick(InputAction.CallbackContext ctx)
        {
            
        }

        private Vector3 GetMouseOverWorldPos()
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(mouseRay, out var hit) ? hit.point : Vector3.zero;
        }
        
        void OnMoveChange(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            moveVector = new Vector3(moveInput.x, 0, moveInput.y);
            champion.SetMoveDirection(moveVector);
        }

        protected override void Link(Entity entity)
        {
            champion = controlledEntity as Champion;
            base.Link(entity);

            cam = Camera.main;
            selectedEntity = new int[1];
            cursorWorldPos = new Vector3[1];

            inputs.Attack.Attack.performed += OnAttack;

            inputs.Capacity.Capacity0.performed += OnActivateCapacity0;
            inputs.Capacity.Capacity1.performed += OnActivateCapacity1;
            inputs.Capacity.Capacity2.performed += OnActivateUltimateAbility;
            
            inputs.Movement.Move.performed += OnMoveChange;
            inputs.Movement.Move.canceled += OnMoveChange;

            inputs.MoveMouse.MousePos.performed += OnMouseMove;
        }

        protected override void Unlink()
        {
            inputs.Attack.Attack.performed -= OnAttack;

            inputs.Capacity.Capacity0.performed -= OnActivateCapacity0;
            inputs.Capacity.Capacity1.performed -= OnActivateCapacity1;
            inputs.Capacity.Capacity2.performed -= OnActivateUltimateAbility;
            
            inputs.Movement.Move.performed -= OnMoveChange;
            inputs.Movement.Move.canceled -= OnMoveChange;
            
            inputs.MoveMouse.MousePos.performed -= OnMouseMove;

            CameraController.Instance.UnLinkCamera();
        }
    }
}