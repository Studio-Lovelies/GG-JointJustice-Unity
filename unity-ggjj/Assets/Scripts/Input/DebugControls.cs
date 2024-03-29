// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/DebugControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DebugControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DebugControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DebugControls"",
    ""maps"": [
        {
            ""name"": ""Keyboard/Mouse"",
            ""id"": ""40c3f060-180e-43d6-981b-b4ece39e00eb"",
            ""actions"": [
                {
                    ""name"": ""ReloadScript"",
                    ""type"": ""Button"",
                    ""id"": ""948de251-4bce-431b-b749-fa13b7c4499b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenEditor"",
                    ""type"": ""Button"",
                    ""id"": ""dce34266-8a6c-4591-a6e9-d2e1fa6dca54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d75864a1-7b83-4223-bbda-c15887431023"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReloadScript"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""6989c641-2aa1-4c80-9640-99fc81755009"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEditor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""cd357d8f-24b0-477a-80bd-7bf1653f4940"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEditor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""1560bb35-bd93-4f6d-93f6-283feb1179f8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEditor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard/Mouse
        m_KeyboardMouse = asset.FindActionMap("Keyboard/Mouse", throwIfNotFound: true);
        m_KeyboardMouse_ReloadScript = m_KeyboardMouse.FindAction("ReloadScript", throwIfNotFound: true);
        m_KeyboardMouse_OpenEditor = m_KeyboardMouse.FindAction("OpenEditor", throwIfNotFound: true);
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

    // Keyboard/Mouse
    private readonly InputActionMap m_KeyboardMouse;
    private IKeyboardMouseActions m_KeyboardMouseActionsCallbackInterface;
    private readonly InputAction m_KeyboardMouse_ReloadScript;
    private readonly InputAction m_KeyboardMouse_OpenEditor;
    public struct KeyboardMouseActions
    {
        private @DebugControls m_Wrapper;
        public KeyboardMouseActions(@DebugControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ReloadScript => m_Wrapper.m_KeyboardMouse_ReloadScript;
        public InputAction @OpenEditor => m_Wrapper.m_KeyboardMouse_OpenEditor;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardMouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardMouseActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardMouseActions instance)
        {
            if (m_Wrapper.m_KeyboardMouseActionsCallbackInterface != null)
            {
                @ReloadScript.started -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnReloadScript;
                @ReloadScript.performed -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnReloadScript;
                @ReloadScript.canceled -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnReloadScript;
                @OpenEditor.started -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnOpenEditor;
                @OpenEditor.performed -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnOpenEditor;
                @OpenEditor.canceled -= m_Wrapper.m_KeyboardMouseActionsCallbackInterface.OnOpenEditor;
            }
            m_Wrapper.m_KeyboardMouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ReloadScript.started += instance.OnReloadScript;
                @ReloadScript.performed += instance.OnReloadScript;
                @ReloadScript.canceled += instance.OnReloadScript;
                @OpenEditor.started += instance.OnOpenEditor;
                @OpenEditor.performed += instance.OnOpenEditor;
                @OpenEditor.canceled += instance.OnOpenEditor;
            }
        }
    }
    public KeyboardMouseActions @KeyboardMouse => new KeyboardMouseActions(this);
    public interface IKeyboardMouseActions
    {
        void OnReloadScript(InputAction.CallbackContext context);
        void OnOpenEditor(InputAction.CallbackContext context);
    }
}
