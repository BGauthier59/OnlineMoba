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

        [SerializeField] private NewActiveCapacity attackCapacity;
        [SerializeField] private NewActiveCapacity capacity1;
        [SerializeField] private NewActiveCapacity capacity2;
        [SerializeField] private NewActiveCapacity ultimateCapacity;

        [Space] [Header("Cooldown")] public bool canCastAutoAttack = true;
        private float autoAttackTimer;
        public float autoAttackCooldownDuration;

        public bool canCastCapacity1 = true;
        private float capacity1Timer;
        public float capacity1CooldownDuration;

        public bool canCastCapacity2 = true;
        private float capacity2Timer;
        public float capacity2CooldownDuration;

        public bool canCastUltimate = true;
        private float ultimateTimer;
        public float ultimateCooldownDuration;

        private void Update()
        {
            if (!photonView.IsMine) return;
            UpdateCooldown();

            cursorWorldPos[0] = GetMouseOverWorldPos();
            var pos = transform.position;
            pos.y = 1;
            mousePositionSphere.position =
                Vector3.Lerp(mousePositionSphere.position, cursorWorldPos[0], Time.deltaTime * 15);
            Debug.DrawLine(pos, cursorWorldPos[0], Color.black);
        }

        private void UpdateCooldown()
        {
            if (!canCastAutoAttack)
            {
                if (autoAttackTimer >= autoAttackCooldownDuration)
                {
                    canCastAutoAttack = true;
                    autoAttackTimer = 0f;
                }
                else autoAttackTimer += Time.deltaTime;
            }

            if (!canCastCapacity1)
            {
                if (capacity1Timer >= capacity1CooldownDuration)
                {
                    canCastCapacity1 = true;
                    capacity1Timer = 0f;
                }
                else capacity1Timer += Time.deltaTime;
            }

            if (!canCastCapacity2)
            {
                if (capacity2Timer >= capacity2CooldownDuration)
                {
                    canCastCapacity2 = true;
                    capacity2Timer = 0f;
                }
                else capacity2Timer += Time.deltaTime;
            }

            if (!canCastUltimate)
            {
                if (ultimateTimer >= ultimateCooldownDuration)
                {
                    canCastUltimate = true;
                    ultimateTimer = 0f;
                }
                else ultimateTimer += Time.deltaTime;
            }
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            if (!attackCapacity) return;

            if (!canCastAutoAttack)
            {
                Debug.LogWarning("Cooldown not over!");
                return;
            }
            canCastAutoAttack = false;
            attackCapacity.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity0(InputAction.CallbackContext ctx)
        {
            if (!capacity1) return;

            if (!canCastCapacity1)
            {
                Debug.LogWarning("Cooldown not over!");
                return;
            }
            canCastCapacity1 = false;
            capacity1.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateCapacity1(InputAction.CallbackContext ctx)
        {
            if (!capacity2) return;

            if (!canCastCapacity2)
            {
                Debug.LogWarning("Cooldown not over!");
                return;
            }
            canCastCapacity2 = false;
            capacity2.RequestCastCapacity(selectedEntity, cursorWorldPos);
        }

        private void OnActivateUltimateAbility(InputAction.CallbackContext ctx)
        {
            if (!ultimateCapacity) return;

            if (!canCastUltimate)
            {
                Debug.LogWarning("Cooldown not over!");
                return;
            }
            canCastUltimate = false;
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