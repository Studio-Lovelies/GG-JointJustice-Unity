using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public Controls Controls { get; set; }
    
    // Add key press events here
    [field: SerializeField] public UnityEvent<Vector2Int> OnDirectionPressed { get; private set; }
    [field: SerializeField] public UnityEvent OnSelectPressed { get; private set; }
    [field: SerializeField] public UnityEvent OnPausePressed { get; private set; }
    
    private void Awake()
    {
        Controls = new Controls();
        
        // Subscribe to Input System events here
        Controls.Player.DirectionalButtons.performed += ctx => CallDirectionPressedEvent(ctx.ReadValue<Vector2>());
        Controls.Player.Select.performed += ctx => OnSelectPressed?.Invoke();
        Controls.Player.Pause.performed += ctx => OnPausePressed?.Invoke();
    }

    private void CallDirectionPressedEvent(Vector2 direction)
    {
        OnDirectionPressed?.Invoke(new Vector2Int((int)direction.x, (int)direction.y));
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
