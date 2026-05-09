using UnityEngine;

public class BossLaserBeam : MonoBehaviour
{
    [SerializeField] private Vector2 _damageArea;
    [SerializeField] private Vector2 _damageAreaCenter;
    [SerializeField] private LayerMask _enemyMask;

    private bool _canDamage;

    private void Update()
    {
        if (_canDamage)
            Damage();
    }

    private void Damage()
    {
        Physics2D.SyncTransforms();

        Vector2 worldCenter = (Vector2)transform.TransformPoint(_damageAreaCenter);

        Collider2D[] objects = Physics2D.OverlapBoxAll(worldCenter, _damageArea, transform.eulerAngles.z, _enemyMask);

#if UNITY_EDITOR
        Debug.Log($"Found {objects.Length} obejcts to attack");
#endif

        foreach (var obj in objects)
        {
            if (obj.TryGetComponent<Health>(out var health) && !health.IsDead)
            {
                health.ReduceHP(1);
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

    public void HandleBeamStart()
    {
        _canDamage = true;
    }

    public void HandleBeamStop()
    {
        _canDamage = false;
    }
}
