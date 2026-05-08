using UnityEngine;

public class Follower : Enemy
{
    protected override void Move()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;

        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }

    protected override void Rotate()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}
