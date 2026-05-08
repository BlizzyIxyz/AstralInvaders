using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _palyerTransform;
    [SerializeField] private EnemyAggregator _aggregator;

    public void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        var instance = Object.Instantiate(prefab, position, Quaternion.identity);
        var enemy = instance.GetComponent<Enemy>();

        enemy.Construct(_palyerTransform);

        _aggregator.AddEnemy(enemy);
    }
}
