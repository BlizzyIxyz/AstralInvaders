using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private LayerMask _enemyMask;

    private Vector2 _direction;

    public void Launch(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    private void Update()
    {
        transform.Translate(_direction * _projectileSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerInertial>(out _))
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (!health.IsDead)
            {
                health.ReduceHP(1);
                Destroy(gameObject);
            }
        }
    }
}