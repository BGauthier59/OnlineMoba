//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputActions/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""2b795710-b287-4812-ae8b-43eb8f73991b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""598f10a3-a71c-4ba7-bccd-c75ad4f936ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e33561b7-f02a-4f3c-99b3-bcb830ee5774"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6046c493-1af8-4773-b534-9edb26f34a04"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""db5831e6-3bad-49c1-9063-296195b540ed"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""89c756b9-e98f-4cd1-aa79-c1a0d2e5b28f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""612eafcd-da8d-4901-8258-b84b7f219246"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Attack"",
            ""id"": ""e779a209-6566-4beb-904b-aec78faf816d"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""cade776d-8858-4577-82ae-2aa0479b540e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d407f34f-474c-4197-aa07-60ff1e9494e0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Capacity"",
            ""id"": ""698439f3-8087-4c51-8d99-5115fa6e1253"",
            ""actions"": [
                {
                    ""name"": ""Capacity0"",
                    ""type"": ""Button"",
                    ""id"": ""3dd9532a-e0fc-4369-91a1-ada3d048afb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Capacity1"",
                    ""type"": ""Button"",
                    ""id"": ""f0ec7891-2904-4b4a-b410-426f3efbc135"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Capacity2"",
                    ""type"": ""Button"",
                    ""id"": ""5149e613-eee6-4a62-8048-6e49faf4b9eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""51ee870a-f512-42bd-87a8-5e549a2d0c34"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Capacity0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0c5895f-7a86-4ec0-a37d-36df2dd2f8cd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Capacity1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9023c4ba-5907-4e90-8ea0-9568be0c0c2b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Capacity2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""745bed8a-cfc0-4f8a-a3ae-b70cfb354251"",
            ""actions"": [
                {
                    ""name"": ""LockToggle"",
                    ""type"": ""Button"",
                    ""id"": ""54a45e78-fe95-4f9d-bf31-b0edcdd20b4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d00ecd55-ab7c-4100-8d0c-99e5e99eedbb"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MoveMouse"",
            ""id"": ""a7ceacff-6044-40be-a466-6447c599e434"",
            ""actions"": [
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""101c5f66-bd62-4b89-b5fd-0dcc2376f192"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ActiveButton"",
                    ""type"": ""Button"",
                    ""id"": ""423383cc-dfaf-401e-9009-8a937d79459c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9074bc1f-6077-45bd-89cd-62bd87652634"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0003a3b0-03b5-4a22-8db0-21af37bb3959"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActiveButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        // Attack
        m_Attack = asset.FindActionMap("Attack", throwIfNotFound: true);
        m_Attack_Attack = m_Attack.FindAction("Attack", throwIfNotFound: true);
        // Capacity
        m_Capacity = asset.FindActionMap("Capacity", throwIfNotFound: true);
        m_Capacity_Capacity0 = m_Capacity.FindAction("Capacity0", throwIfNotFound: true);
        m_Capacity_Capacity1 = m_Capacity.FindAction("Capacity1", throwIfNotFound: true);
        m_Capacity_Capacity2 = m_Capacity.FindAction("Capacity2", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_LockToggle = m_Camera.FindAction("LockToggle", throwIfNotFound: true);
        // MoveMouse
        m_MoveMouse = asset.FindActionMap("MoveMouse", throwIfNotFound: true);
        m_MoveMouse_MousePos = m_MoveMouse.FindAction("MousePos", throwIfNotFound: true);
        m_MoveMouse_ActiveButton = m_MoveMouse.FindAction("ActiveButton", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    public struct MovementActions
    {
        private @PlayerInputs m_Wrapper;
        public MovementActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Attack
    private readonly InputActionMap m_Attack;
    private IAttackActions m_AttackActionsCallbackInterface;
    private readonly InputAction m_Attack_Attack;
    public struct AttackActions
    {
        private @PlayerInputs m_Wrapper;
        public AttackActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Attack_Attack;
        public InputActionMap Get() { return m_Wrapper.m_Attack; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AttackActions set) { return set.Get(); }
        public void SetCallbacks(IAttackActions instance)
        {
            if (m_Wrapper.m_AttackActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_AttackActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_AttackActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_AttackActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_AttackActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public AttackActions @Attack => new AttackActions(this);

    // Capacity
    private readonly InputActionMap m_Capacity;
    private ICapacityActions m_CapacityActionsCallbackInterface;
    private readonly InputAction m_Capacity_Capacity0;
    private readonly InputAction m_Capacity_Capacity1;
    private readonly InputAction m_Capacity_Capacity2;
    public struct CapacityActions
    {
        private @PlayerInputs m_Wrapper;
        public CapacityActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Capacity0 => m_Wrapper.m_Capacity_Capacity0;
        public InputAction @Capacity1 => m_Wrapper.m_Capacity_Capacity1;
        public InputAction @Capacity2 => m_Wrapper.m_Capacity_Capacity2;
        public InputActionMap Get() { return m_Wrapper.m_Capacity; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CapacityActions set) { return set.Get(); }
        public void SetCallbacks(ICapacityActions instance)
        {
            if (m_Wrapper.m_CapacityActionsCallbackInterface != null)
            {
                @Capacity0.started -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity0;
                @Capacity0.performed -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity0;
                @Capacity0.canceled -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity0;
                @Capacity1.started -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity1;
                @Capacity1.performed -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity1;
                @Capacity1.canceled -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity1;
                @Capacity2.started -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity2;
                @Capacity2.performed -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity2;
                @Capacity2.canceled -= m_Wrapper.m_CapacityActionsCallbackInterface.OnCapacity2;
            }
            m_Wrapper.m_CapacityActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Capacity0.started += instance.OnCapacity0;
                @Capacity0.performed += instance.OnCapacity0;
                @Capacity0.canceled += instance.OnCapacity0;
                @Capacity1.started += instance.OnCapacity1;
                @Capacity1.performed += instance.OnCapacity1;
                @Capacity1.canceled += instance.OnCapacity1;
                @Capacity2.started += instance.OnCapacity2;
                @Capacity2.performed += instance.OnCapacity2;
                @Capacity2.canceled += instance.OnCapacity2;
            }
        }
    }
    public CapacityActions @Capacity => new CapacityActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_LockToggle;
    public struct CameraActions
    {
        private @PlayerInputs m_Wrapper;
        public CameraActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @LockToggle => m_Wrapper.m_Camera_LockToggle;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @LockToggle.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockToggle;
                @LockToggle.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockToggle;
                @LockToggle.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnLockToggle;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LockToggle.started += instance.OnLockToggle;
                @LockToggle.performed += instance.OnLockToggle;
                @LockToggle.canceled += instance.OnLockToggle;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // MoveMouse
    private readonly InputActionMap m_MoveMouse;
    private IMoveMouseActions m_MoveMouseActionsCallbackInterface;
    private readonly InputAction m_MoveMouse_MousePos;
    private readonly InputAction m_MoveMouse_ActiveButton;
    public struct MoveMouseActions
    {
        private @PlayerInputs m_Wrapper;
        public MoveMouseActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePos => m_Wrapper.m_MoveMouse_MousePos;
        public InputAction @ActiveButton => m_Wrapper.m_MoveMouse_ActiveButton;
        public InputActionMap Get() { return m_Wrapper.m_MoveMouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveMouseActions set) { return set.Get(); }
        public void SetCallbacks(IMoveMouseActions instance)
        {
            if (m_Wrapper.m_MoveMouseActionsCallbackInterface != null)
            {
                @MousePos.started -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnMousePos;
                @ActiveButton.started -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnActiveButton;
                @ActiveButton.performed -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnActiveButton;
                @ActiveButton.canceled -= m_Wrapper.m_MoveMouseActionsCallbackInterface.OnActiveButton;
            }
            m_Wrapper.m_MoveMouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
                @ActiveButton.started += instance.OnActiveButton;
                @ActiveButton.performed += instance.OnActiveButton;
                @ActiveButton.canceled += instance.OnActiveButton;
            }
        }
    }
    public MoveMouseActions @MoveMouse => new MoveMouseActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IAttackActions
    {
        void OnAttack(InputAction.CallbackContext context);
    }
    public interface ICapacityActions
    {
        void OnCapacity0(InputAction.CallbackContext context);
        void OnCapacity1(InputAction.CallbackContext context);
        void OnCapacity2(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnLockToggle(InputAction.CallbackContext context);
    }
    public interface IMoveMouseActions
    {
        void OnMousePos(InputAction.CallbackContext context);
        void OnActiveButton(InputAction.CallbackContext context);
    }
}