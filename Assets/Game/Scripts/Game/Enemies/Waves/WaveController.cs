using System;
using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private EnemyAggregator _aggregator;
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Wave _tutorialWave;

    private float _delayTimer;
    private float _currentDelay;

    private int _currentWaveIndex;
    private int _currentStepIndex;
    private bool _isWaitingForEnemiesClear;

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
        if (!_aggregator.HasEnemies)
        {
            _currentWaveIndex++;
            _currentStepIndex = 0;
            _isWaitingForEnemiesClear = false;

            _delayTimer = 0f;

            if (_currentWaveIndex >= _waves.Length)
            {
                OnLastStepComplete?.Invoke();
                enabled = false;
            }
        }
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