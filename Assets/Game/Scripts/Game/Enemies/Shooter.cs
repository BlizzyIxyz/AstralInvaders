using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _rotationAngle;
    [SerializeField] private float _shootDelay;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootClip;

    private bool _playerIsInRange;
    private float _delayTimer;

    protected override void Update()
    {
        base.Update();
        UpdateTimer();
        TryShoot();
    }

    private void UpdateTimer()
    {
        _delayTimer += Time.deltaTime;
    }

    private void TryShoot()
    {
        if (_delayTimer >= _shootDelay && _playerIsInRange && _isWithinScreen)
        {
            _delayTimer = 0;
            Shoot();
        }
    }

    protected override void Move()
    {
        Vector2 vector = _playerTransform.position - transform.position;
        Vector2 direction = vector.normalized;

        if (vector.sqrMagnitude <= _stopDistance * _stopDistance && _isWithinScreen)
        {
            _playerIsInRange = true;
            return;
        }
        else
            _playerIsInRange = false;

        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }

    protected override void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _rotationAngle * Time.deltaTime);
    }

    private void Shoot()
    {
        _audioSource.PlayOneShot(_shootClip);

        Vector2 vector = _playerTransform.position - transform.position;
        Vector2 direction = vector.normalized;

        var projectile = Object.Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.Launch(direction);
    }
}
