using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Controls Controls { get; set; }

    private void Awake()
    {
        Controls = new Controls();
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
