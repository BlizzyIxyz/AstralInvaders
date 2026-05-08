using UnityEngine;

public class FollowerTank : Enemy
{
    [SerializeField] private float _rotationAngle;


    protected override void Move()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;

        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }

    protected override void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _rotationAngle * Time.deltaTime);
    }
}
