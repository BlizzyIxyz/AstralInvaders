using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyAggregator _aggregator;
    [SerializeField] private Transform _screenCenter;
    [SerializeField] private Vector2 _screenSize = new Vector2(20f, 10f);
    [SerializeField] private GameObject _indicatorPrefab;

    private Dictionary<Enemy, IndicatorData> _activeIndicators = new Dictionary<Enemy, IndicatorData>();

    private void OnEnable()
    {
        if (_aggregator != null)
            _aggregator.OnEnemyAdded += HandleEnemyAdded;
    }

    private void OnDisable()
    {
        if (_aggregator != null)
            _aggregator.OnEnemyAdded -= HandleEnemyAdded;
    }

    private void HandleEnemyAdded(Enemy enemy)
    {
        if (_activeIndicators.ContainsKey(enemy)) return;

        GameObject indicatorObj = Instantiate(_indicatorPrefab, transform);
        indicatorObj.SetActive(false);

        _activeIndicators.Add(enemy, new IndicatorData
        {
            IndicatorObject = indicatorObj,
            IsInsideScreen = false
        });

        enemy.OnDeath += RemoveIndicator;
    }

    private void RemoveIndicator(Enemy enemy)
    {
        if (_activeIndicators.TryGetValue(enemy, out var data))
        {
            if (data.IndicatorObject != null)
                Destroy(data.IndicatorObject);

            _activeIndicators.Remove(enemy);
        }
    }

    private void LateUpdate()
    {
        List<Enemy> enemiesToCheck = new List<Enemy>(_activeIndicators.Keys);

        foreach (var enemy in enemiesToCheck)
        {
            if (enemy == null || enemy.IsDead)
            {
                RemoveIndicator(enemy);
                continue;
            }

            ProcessEnemyVisibility(enemy);
        }
    }

    private void ProcessEnemyVisibility(Enemy enemy)
    {
        if (!_activeIndicators.TryGetValue(enemy, out var data)) return;

        Vector3 enemyPos = enemy.Position;
        Vector3 center = _screenCenter.position;

        bool isInside = IsInsideBox(enemyPos, center, _screenSize);

        if (isInside)
        {
            if (!data.IsInsideScreen)
            {
                data.IsInsideScreen = true;
                data.IndicatorObject.SetActive(false);

                enemy.OnScreenSpaceEnter();
            }
        }
        else
        {
            data.IsInsideScreen = false;

            enemy.SetRendererVisible(false);

            data.IndicatorObject.SetActive(true);
            PositionIndicator(data.IndicatorObject.transform, enemyPos, center, _screenSize);
        }
    }

    private bool IsInsideBox(Vector3 pos, Vector3 center, Vector2 size)
    {
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        return (pos.x >= center.x - halfWidth && pos.x <= center.x + halfWidth &&
                pos.y >= center.y - halfHeight && pos.y <= center.y + halfHeight);
    }

    private void PositionIndicator(Transform indicator, Vector3 enemyPos, Vector3 center, Vector2 size)
    {
        Vector2 dir = (enemyPos - center).normalized;
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        float minT = float.MaxValue;

        if (dir.x != 0)
        {
            float t = halfWidth / Mathf.Abs(dir.x);
            if (t < minT) minT = t;
        }

        if (dir.y != 0)
        {
            float t = halfHeight / Mathf.Abs(dir.y);
            if (t < minT) minT = t;
        }

        Vector3 targetPos = center + (Vector3)(dir * minT * 0.9f);

        indicator.position = new Vector3(targetPos.x, targetPos.y, 0f);
    }

    private struct IndicatorData
    {
        public GameObject IndicatorObject;
        public bool IsInsideScreen;
    }

    private void OnDrawGizmosSelected()
    {
        if (_screenCenter == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_screenCenter.position, _screenSize);
    }
}