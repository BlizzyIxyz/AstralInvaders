using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private AudioClip _hitClip;

    private Vector2 _direction;

    private void Start()
    {
        Destroy(gameObject, 7f);
    }

    public void Launch(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    private void Update()
    {
        float moveDistance = _projectileSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, moveDistance, _enemyMask);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Health>(out var health))
            {
                if (!health.IsDead)
                {
                    health.ReduceHP(_projectileDamage);
                    Destroy(gameObject);
                    //SingleAudioSource.Instance.AudioSource.PlayOneShot(_hitClip);
                    return;
                }
            }
        }

        transform.Translate(_direction * moveDistance, Space.World);
    }
}