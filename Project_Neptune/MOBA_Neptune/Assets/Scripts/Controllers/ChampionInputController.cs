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
        
        /// <summary>
        /// Actions Performed on Attack Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnAttack(InputAction.CallbackContext ctx)
        {
            champion.RequestAttack(champion.attackAbilityIndex,selectedEntity,cursorWorldPos);
        }
        
        /// <summary>
        /// Actions Performed on Show or Hide Shop
        /// </summary>
        /// <param name="ctx"></param>
        private void OnShowHideShop(InputAction.CallbackContext ctx)
        {
            UIManager.Instance.ShowHideShop();
        }
        
        /// <summary>
        /// Actions Performed on Capacity 0 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateCapacity0(InputAction.CallbackContext ctx)
        {
            ActiveCapacitySO capacity0 = CapacitySOCollectionManager.GetActiveCapacitySOByIndex(champion.abilitiesIndexes[0]);

            champion.RequestCast(champion.abilitiesIndexes[0],selectedEntity,cursorWorldPos);
        }
        /// <summary>
        /// Actions Performed on Capacity 1 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateCapacity1(InputAction.CallbackContext ctx)
        {
            champion.RequestCast(champion.abilitiesIndexes[1],selectedEntity,cursorWorldPos);
        }
        /// <summary>
        /// Actions Performed on Capacity 2 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateUltimateAbility(InputAction.CallbackContext ctx)
        {
            champion.RequestCast(champion.ultimateAbilityIndex,selectedEntity,cursorWorldPos);
        }

        /// <summary>
        /// Actions Performed on Item 0 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateItem0(InputAction.CallbackContext ctx)
        {
            champion.RequestActivateItem(0,selectedEntity,cursorWorldPos);
        }
        
        /// <summary>
        /// Actions Performed on Item 1 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateItem1(InputAction.CallbackContext ctx)
        {
            champion.RequestActivateItem(1,selectedEntity,cursorWorldPos);
        }
        /// <summary>
        /// Actions Performed on Item 2 Activation
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActivateItem2(InputAction.CallbackContext ctx)
        {
            champion.RequestActivateItem(2,selectedEntity,cursorWorldPos);
        }

        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            mousePos = ctx.ReadValue<Vector2>();
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(mouseRay, out var hit)) return;
            cursorWorldPos[0] = hit.point;
            selectedEntity[0] = -1;
            var ent = hit.transform.GetComponent<Entity>();
            if (ent == null && hit.transform.parent != null) hit.transform.parent.GetComponent<Entity>();
            if(ent != null)
            { 
                selectedEntity[0] = ent.entityIndex;
                cursorWorldPos[0] = ent.transform.position;
            }

            if(isActivebuttonPress)
            {
                champion.MoveToPosition(GetMouseOverWorldPos());
            }
        }
        
        void OnMouseClick(InputAction.CallbackContext ctx)
        {
            champion.MoveToPosition(cursorWorldPos[0]);
            if (selectedEntity[0] != -1)
            {
                champion.RequestAttack(champion.attackAbilityIndex, selectedEntity, cursorWorldPos);
            }
        }

        
        
        /// <summary>
        /// Get World Position of mouse
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        public Vector3 GetMouseOverWorldPos()
        {
            Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Actions Performed on Move inputs direction Change
        /// </summary>
        /// <param name="ctx"></param>
        void OnMoveChange(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            moveVector = new Vector3(moveInput.x, 0, moveInput.y);
            champion.SetMoveDirection(moveVector);
            NavMeshAgent agent;
      

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

            if (champion.isBattlerite)
            {
                inputs.Movement.Move.performed += OnMoveChange; 
                inputs.Movement.Move.canceled += OnMoveChange;
                champion.GetComponent<NavMeshAgent>().enabled = false;
                champion.rb.isKinematic = false;
            }
            else
            {
                champion.GetComponent<NavMeshAgent>().enabled = true;
                inputs.MoveMouse.ActiveButton.performed += OnMouseClick;
                inputs.MoveMouse.ActiveButton.started += context => isActivebuttonPress = true;
                inputs.MoveMouse.ActiveButton.canceled += context => isActivebuttonPress = false;
                champion.rb.isKinematic = true;
            }

            inputs.MoveMouse.MousePos.performed += OnMouseMove;
            
            inputs.Inventory.ActivateItem0.performed += OnActivateItem0;
            inputs.Inventory.ActivateItem1.performed += OnActivateItem1;
            inputs.Inventory.ActivateItem2.performed += OnActivateItem2;
            inputs.Inventory.ShowHideInventory.started += context => UIManager.Instance.ShowHideInventory(true);
            inputs.Inventory.ShowHideInventory.canceled += context => UIManager.Instance.ShowHideInventory(false);
            inputs.Inventory.ShowHideShop.performed += OnShowHideShop;

        }
        
        protected override void Unlink()
        {
            inputs.Attack.Attack.performed -= OnAttack;
            
            inputs.Capacity.Capacity0.performed -= OnActivateCapacity0;
            inputs.Capacity.Capacity1.performed -= OnActivateCapacity1;
            inputs.Capacity.Capacity2.performed -= OnActivateUltimateAbility;
            inputs.Inventory.ShowHideShop.performed -= OnShowHideShop;

            if (champion.isBattlerite)
            {
                inputs.Movement.Move.performed -= OnMoveChange; 
                inputs.Movement.Move.canceled -= OnMoveChange;
            }
            else
            {
                inputs.MoveMouse.ActiveButton.performed -= OnMouseClick;
            }

            inputs.MoveMouse.MousePos.performed -= OnMouseMove;

            CameraController.Instance.UnLinkCamera();
        }
    }
}
