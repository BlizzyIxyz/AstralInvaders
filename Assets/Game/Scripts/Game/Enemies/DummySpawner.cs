using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _maxY = 10f;

    [Header("Timing Settings")]
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private bool _spawnOnStart = true;

    private void Start()
    {
        Time.timeScale = 1f;

        if (_spawnOnStart)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (_prefabs.Length > 0)
            {
                SpawnRandomPrefab();
            }
            else
            {
                Debug.LogWarning("No prefabs assigned in RandomPrefabSpawner", this);
                yield break;
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnRandomPrefab()
    {
        GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];

        Vector3 position = new Vector3(
            0,
            Random.Range(_minY, _maxY),
            0
        );

        Instantiate(prefab, transform.position + position, Quaternion.identity);
    }

    public void StartSpawning()
    {
        if (_spawnOnStart) return;
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}