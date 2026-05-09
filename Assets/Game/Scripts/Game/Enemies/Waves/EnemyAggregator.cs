using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggregator : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();

    public List<Enemy> GetEnemies() => _enemies;

    public event Action OnLastEnemyRemoved;
    public event Action OnEnemyRemoved;
    public event Action<Enemy> OnEnemyAdded;
    public bool HasEnemies { get; private set; }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
        enemy.OnDeath += RemoveEnemy;

        OnEnemyAdded?.Invoke(enemy);

        HasEnemies = true;
    }

    private void RemoveEnemy(Enemy enemy)
    {
#if UNITY_EDITOR
        Debug.Log("Removeing emeny");
#endif

        _enemies.Remove(enemy);
        enemy.OnDeath -= RemoveEnemy;

        HasEnemies = _enemies.Count != 0;

        OnEnemyRemoved?.Invoke();

        if (!HasEnemies)
            OnLastEnemyRemoved?.Invoke();
    }
}
