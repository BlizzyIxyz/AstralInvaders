using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class WaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private EnemyAggregator _aggregator;
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Wave _tutorialWave;

    [SerializeField] private GameObject[] _bosses;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _bossSpawnPoint;
    [SerializeField] private GameObject _bossUpgradeWindow;
    [SerializeField] private PlayableDirector _winCutscene;

    private float _delayTimer;
    private float _currentDelay;

    private int _currentWaveIndex;
    private int _currentStepIndex;
    private bool _isWaitingForEnemiesClear;
    private bool _isBossSpawned;
    private Boss _currentBossInstance;

    public event Action OnLastStepComplete;

    public bool TutorialWaveComplete { get; private set; }

    private void Update()
    {
        if (!TutorialWaveComplete)
            return;

        if (_isWaitingForEnemiesClear)
        {
            HandleWaveTransition();
        }
        else
        {
            UpdateTimer();
            TryStartNextStep();
        }
    }

    private void HandleWaveTransition()
    {
        if (_isBossSpawned && _currentBossInstance != null)
        {
            return;
        }

        if (!_aggregator.HasEnemies)
        {
            if (!_isBossSpawned)
            {
                if (_bosses != null && _currentWaveIndex < _bosses.Length && _bosses[_currentWaveIndex] != null)
                {
                    SpawnBoss(_bosses[_currentWaveIndex]);
                    return;
                }
            }

            _currentWaveIndex++;
            _currentStepIndex = 0;
            _isWaitingForEnemiesClear = false;
            _isBossSpawned = false;
            _currentBossInstance = null;
            _delayTimer = 0f;

            if (_currentWaveIndex >= _waves.Length)
            {
                OnLastStepComplete?.Invoke();
                enabled = false;
                _winCutscene.Play();
            }
        }
    }

    private void SpawnBoss(GameObject bossPrefab)
    {
        Vector3 spawnPosition = _bossSpawnPoint != null ? _bossSpawnPoint.position : transform.position;

        GameObject bossObject = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        _currentBossInstance = bossObject.GetComponent<Boss>();

        if (_currentBossInstance != null)
        {
            _currentBossInstance.Activate(_playerTransform, _bossUpgradeWindow);
        }

        _isBossSpawned = true;
    }

    private void UpdateTimer()
    {
        _delayTimer += Time.deltaTime;
    }

    private void TryStartNextStep()
    {
        if (_delayTimer >= _currentDelay)
        {
            StartNextStep();
        }
    }

    private void StartNextStep()
    {
        if (_waves.Length == 0 || _currentWaveIndex >= _waves.Length) return;

        Wave currentWave = _waves[_currentWaveIndex];

        if (_currentStepIndex >= currentWave.Steps.Length)
        {
            _isWaitingForEnemiesClear = true;
            return;
        }

        WaveStep step = currentWave.Steps[_currentStepIndex];

        for (int i = 0; i < step.SpawnEnemyEntries.Length; i++)
        {
            SpawnEnemyEntry entry = step.SpawnEnemyEntries[i];
            _spawner.SpawnEnemy(entry.Prefab, entry.SpawnPosition);
        }

        _currentDelay = step.DelayAfter;
        _delayTimer = 0f;

        _currentStepIndex++;
    }

    public IEnumerator StartTutorialWave()
    {
        _spawner.SpawnEnemy(_tutorialWave.Steps[0].SpawnEnemyEntries[0].Prefab, _tutorialWave.Steps[0].SpawnEnemyEntries[0].SpawnPosition);

        yield return new WaitUntil(() => !_aggregator.HasEnemies);

        _spawner.SpawnEnemy(_tutorialWave.Steps[1].SpawnEnemyEntries[0].Prefab, _tutorialWave.Steps[1].SpawnEnemyEntries[0].SpawnPosition);

        yield return new WaitUntil(() => !_aggregator.HasEnemies);

        TutorialWaveComplete = true;

        _currentWaveIndex = 0;
        _currentStepIndex = 0;
        _currentDelay = 0;
    }

    public void SkipTutorial()
    {
        TutorialWaveComplete = true;

        _currentWaveIndex = 0;
        _currentStepIndex = 0;
        _currentDelay = 0;
    }
}