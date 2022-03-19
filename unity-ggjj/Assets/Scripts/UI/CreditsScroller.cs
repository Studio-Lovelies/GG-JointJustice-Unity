using System.Collections;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private RectTransform _endBlock;

    private Transform _transform;
    private Vector2 _target;
    

    private IEnumerator Start()
    {
        _transform = transform;
        yield return null; // Wait for frame because sizeDelta is 0 on first frame
        _target = new Vector2(0, Mathf.Abs(_endBlock.position.y) + _endBlock.sizeDelta.y / 2);
    }

    private void Update()
    {
        _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime);
    }
}
