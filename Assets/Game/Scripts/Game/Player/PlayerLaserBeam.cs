using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerLaserBeam : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private Vector2 _damageArea;
    [SerializeField] private Vector2 _damageAreaCenter;
    [SerializeField] private float _damage;
    [SerializeField] private LayerMask _enemyMask;

    public event Action OnShotEnd;
    public event Action OnShotStart;

    private bool _canDamage;

    public void Shoot()
    {
        _playableDirector.Play();
    }

    private void Update()
    {
        if (_canDamage)
            Damage();
    }

    private void Damage()
    {
        Physics2D.SyncTransforms();

        Vector2 worldCenter = (Vector2)transform.TransformPoint(_damageAreaCenter);

        Vector2 worldSize = new Vector2(
            _damageArea.x * Mathf.Abs(transform.localScale.x),
            _damageArea.y * Mathf.Abs(transform.localScale.y)
        );

        Collider2D[] objects = Physics2D.OverlapBoxAll(worldCenter, worldSize, transform.eulerAngles.z, _enemyMask);

#if UNITY_EDITOR
        Debug.Log($"Found {objects.Length} obejcts to attack");
#endif

        foreach (var obj in objects)
        {
            if (obj.TryGetComponent<Health>(out var health) && !health.IsDead)
            {
                health.ReduceHP(_damage);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_damageAreaCenter, _damageArea);
    }
#endif

    public void HandleShotStartSignal()
    {
        _canDamage = true;
        OnShotStart?.Invoke();
    }

    public void HandleShotEndSignal()
    {
        _canDamage = false;
        OnShotEnd?.Invoke();
    }
}