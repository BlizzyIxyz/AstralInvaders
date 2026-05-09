using UnityEngine;

[DisallowMultipleComponent]
public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private float _rotation;

    private void LateUpdate()
    {
        _rotation += _speed * Time.deltaTime % 360;

        transform.rotation = Quaternion.Euler(0f, 0f, _rotation);
    }
}