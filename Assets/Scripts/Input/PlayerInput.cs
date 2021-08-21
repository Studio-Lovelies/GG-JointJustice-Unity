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
        Controls.Player.Select.performed += ctx => OnSelectPressed?.Invoke();
        Controls.Player.Pause.performed += ctx => OnPausePressed?.Invoke();
        
        // Directional arrows
        Controls.Player.Left.performed += ctx => OnDirectionPressed?.Invoke(Vector2Int.left);
        Controls.Player.Right.performed += ctx => OnDirectionPressed?.Invoke(Vector2Int.right);
        Controls.Player.Up.performed += ctx => OnDirectionPressed?.Invoke(Vector2Int.up);
        Controls.Player.Down.performed += ctx => OnDirectionPressed?.Invoke(Vector2Int.down);
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
