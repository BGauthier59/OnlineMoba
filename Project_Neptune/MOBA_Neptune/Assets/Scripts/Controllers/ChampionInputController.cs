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

        // Capacity 0 CD Variables
        [Space] [Header("COOLDOWN")] public bool canCastAA = true;
        private float AATimer;
        [SerializeField] private float AACooldown = 0.45f;

        // Capacity 1 CD Variables
        [Space] [Header("COOLDOWN")] public bool canCastCapa1 = true;
        private float capa1Timer;
        public float capa1Cooldown = 2.5f;

        // Capacity 2 CD Variables
        [Space] [Header("COOLDOWN")] public bool canCastCapa2 = true;
        private float capa2Timer;
        public float capa2Cooldown = 3f;

        // Capacity Ultimate CD Variables
        [Space] [Header("COOLDOWN")] public bool canCastUltimate = true;
        private float capa3Timer;
        public float capa3Cooldown = 3f;

        private void SetupAbilitiesCD()
        {
            if (!photonView.IsMine) return;

            if (champion.championSo.activeCapacities[1])
                AACooldown = champion.championSo.activeCapacities[1].cooldown;
            
            if (champion.championSo.activeCapacities[0])
                capa1Cooldown = champion.championSo.activeCapacities[0].cooldown;
        }

        private void FixedUpdate()
        {
            if (!canCastAA)
            {
                AATimer += Time.deltaTime;
                if (AATimer >= AACooldown)
                {
                    canCastAA = true;
                    AATimer = 0f;
                }
            }

            if (!canCastCapa1)
            {
                capa1Timer += Time.deltaTime;
                if (capa1Timer >= capa1Cooldown)
                {
                    canCastCapa1 = true;
                    capa1Timer = 0f;
                }
            }

            if (!canCastCapa2)
            {
                capa2Timer += Time.deltaTime;
                if (capa2Timer >= capa2Cooldown)
                {
                    canCastCapa2 = true;
                    capa2Timer = 0f;
                }
            }

            if (!canCastUltimate)
            {
                capa3Timer += Time.deltaTime;
                if (capa3Timer >= capa3Cooldown)
                {
                    canCastUltimate = true;
                    capa3Timer = 0f;
                }
            }
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            if (champion.abilitiesIndexes[0] == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            if (canCastAA)
            {
                champion.RequestCast(champion.abilitiesIndexes[1], selectedEntity, cursorWorldPos);
                canCastAA = false;
            }
        }

        private void OnActivateCapacity0(InputAction.CallbackContext ctx)
        {
            if (champion.abilitiesIndexes[0] == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            if (canCastCapa1)
            {
                champion.RequestCast(champion.abilitiesIndexes[0], selectedEntity, cursorWorldPos);
                canCastCapa1 = false;
            }
        }

        private void OnActivateCapacity1(InputAction.CallbackContext ctx)
        {
            if (champion.abilitiesIndexes[2] == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            if (canCastCapa2)
            {
                champion.RequestCast(champion.abilitiesIndexes[2], selectedEntity, cursorWorldPos);
                canCastCapa2 = false;
            }
        }

        private void OnActivateUltimateAbility(InputAction.CallbackContext ctx)
        {
            if (champion.ultimateAbilityIndex == 255)
            {
                Debug.LogWarning("No attack implemented!");
                return;
            }

            if (canCastUltimate)
            {
                champion.RequestCast(champion.ultimateAbilityIndex, selectedEntity, cursorWorldPos);
                canCastUltimate = false;
            }
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