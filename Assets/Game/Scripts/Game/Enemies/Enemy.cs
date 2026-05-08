using System;
using UnityEngine;

public interface IIndicatorTarget
{
    Vector3 Position { get; }
    bool IsDead { get; }
    void OnScreenSpaceEnter();
    void SetRendererVisible(bool visible);
}

public abstract class Enemy : MonoBehaviour, IIndicatorTarget
{
    [SerializeField] protected GameObject _deathParticle;
    [SerializeField] protected Health _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    public event Action<Enemy> OnDeath;

    public Vector3 Position => transform.position;
    public bool IsDead => _health.IsDead;

    protected Transform _playerTransform;

    public void Construct(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    protected virtual void Awake()
    {
        _health.OnDeath += HandleDeath;
    }

    protected virtual void Update()
    {
        Move();
        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerInertial>(out _))
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (!health.IsDead)
            {
                health.ReduceHP(1);
                _health.Kill();
            }
        }
    }

    private void HandleDeath()
    {
        OnDeath?.Invoke(this);
        Instantiate(_deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void OnScreenEnter()
    {

    }

    public virtual void OnScreenSpaceEnter()
    {
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }

    public void SetRendererVisible(bool visible)
    {
        if (_spriteRenderer != null) _spriteRenderer.enabled = visible;
    }

    protected abstract void Move();
    protected abstract void Rotate();
}