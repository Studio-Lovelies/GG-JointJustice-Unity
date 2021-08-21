using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class InputRepeater : MonoBehaviour
{
    [field: SerializeField, Tooltip("Subscribe to this event to get repeating directional input.")]
    public UnityEvent<Vector2Int> OnDirectionRepeat { get; private set; }

    [SerializeField, Tooltip("The delay between first and second input.")]
    private float _initialDelay;

    [SerializeField, Tooltip("The delay between inputs.")]
    private float _repeatDelay;

    private float _delayTime;
    private float _timer;
    private Vector2Int _direction;
    private bool _gettingInput;

    private void Awake()
    {
        _delayTime = _initialDelay;
    }
    
    public void GetDirection(Vector2Int direction)
    { 
        if (direction == Vector2.zero)
        {
            _delayTime = _initialDelay;
            _gettingInput = false;
            _timer = 0;
        }
        else
        {
            _delayTime = _initialDelay;
            _timer = 0;

            _gettingInput = true;
        }

        if (_direction == Vector2.zero)
        {
            OnDirectionRepeat?.Invoke(direction);
        }
        
        _direction = direction;
    }

    public void GetHorizontal(int value)
    {
        _direction.y = 0;
        _direction.x = value;
        
        ExecuteFirstInput(value);
    }

    public void GetVertical(int value)
    {
        _direction.x = 0;
        _direction.y = value;
        
        ExecuteFirstInput(value);
        }

    private void ExecuteFirstInput(int value)
    {
        if (value != 0)
        {
            _delayTime = _initialDelay;
            _timer = Time.time;
            OnDirectionRepeat?.Invoke(_direction);
        }
    }

    private void Update()
    {
        if (_direction != Vector2Int.zero)
        {
            if (Time.time - _timer > _delayTime)
            {
                _timer = Time.time;
                _delayTime = _repeatDelay;
                OnDirectionRepeat?.Invoke(_direction);
            }
        }
    }
}
