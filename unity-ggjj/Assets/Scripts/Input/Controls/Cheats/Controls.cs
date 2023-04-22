// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Controls/Cheats/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Cheats
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Keyboard"",
            ""id"": ""b31be4ef-19d1-448d-a0c2-b131fc8b19f3"",
            ""actions"": [
                {
                    ""name"": ""ValidKeys"",
                    ""type"": ""Button"",
                    ""id"": ""e94147e3-0e83-456c-8080-106a99c27701"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f01a5b3e-f804-449f-ba98-419ada206b67"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a6aa8f0-3026-4e02-b5e9-b8a07b969087"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe2e9a1e-6dd5-4029-a1e2-9fd6dedb59cf"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94ed4b47-3f44-4200-8c09-4cd154faa198"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0400bdcb-3b89-4ea0-8a27-aafd65941081"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c82b489-0a2b-45f0-8b44-99962b03c246"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4185a8e4-073f-4404-a2c4-404fe9c0010f"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ValidKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Keyboard
            m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
            m_Keyboard_ValidKeys = m_Keyboard.FindAction("ValidKeys", throwIfNotFound: true);
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

        // Keyboard
        private readonly InputActionMap m_Keyboard;
        private IKeyboardActions m_KeyboardActionsCallbackInterface;
        private readonly InputAction m_Keyboard_ValidKeys;
        public struct KeyboardActions
        {
            private @Controls m_Wrapper;
            public KeyboardActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @ValidKeys => m_Wrapper.m_Keyboard_ValidKeys;
            public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
            public void SetCallbacks(IKeyboardActions instance)
            {
                if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
                {
                    @ValidKeys.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnValidKeys;
                    @ValidKeys.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnValidKeys;
                    @ValidKeys.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnValidKeys;
                }
                m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ValidKeys.started += instance.OnValidKeys;
                    @ValidKeys.performed += instance.OnValidKeys;
                    @ValidKeys.canceled += instance.OnValidKeys;
                }
            }
        }
        public KeyboardActions @Keyboard => new KeyboardActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        public interface IKeyboardActions
        {
            void OnValidKeys(InputAction.CallbackContext context);
        }
    }
}
