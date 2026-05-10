using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _palyerTransform;
    [SerializeField] private Transform _parent;
    [SerializeField] private EnemyAggregator _aggregator;

    public void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        var instance = Object.Instantiate(prefab, position, Quaternion.identity, _parent);
        var enemy = instance.GetComponent<Enemy>();

        enemy.Construct(_palyerTransform);

        _aggregator.AddEnemy(enemy);
    }
}
