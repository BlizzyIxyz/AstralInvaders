using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggregator : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();

    public event Action OnLastEnemyRemoved;
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
        _enemies.Remove(enemy);
        enemy.OnDeath -= RemoveEnemy;

        HasEnemies = _enemies.Count != 0;

        if (!HasEnemies)
            OnLastEnemyRemoved?.Invoke();
    }
}
