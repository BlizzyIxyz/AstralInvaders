using UnityEngine;

public class Dummy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.World);
    }
}