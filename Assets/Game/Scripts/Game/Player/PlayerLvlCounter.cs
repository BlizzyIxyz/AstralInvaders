using System;
using UnityEngine;

public class PlayerLvlCounter : MonoBehaviour
{
    [SerializeField] private EnemyAggregator _enemyAggregator;
    [SerializeField] private TutorialController _tutorialController;
    [SerializeField] private float[] _xpNeededToUpgrade;

    private float _xp;
    private int _lvl;

    public event Action<float> OnXpChange;
    public event Action<int> OnLvlChange;

    public int CurrentLevel => _lvl;
    public float CurrentXp => _xp;

    public float GetXpThreshold(int level)
    {
        if (level >= 0 && level < _xpNeededToUpgrade.Length)
            return _xpNeededToUpgrade[level];
        return 0;
    }

    private void Awake()
    {
        _enemyAggregator.OnEnemyRemoved += HandleEnemyKilled;
    }

    private void HandleEnemyKilled()
    {
#if UNITY_EDITOR
        Debug.Log("Handle kill");
#endif

        if (_lvl >= _xpNeededToUpgrade.Length || !_tutorialController.EnemyTutorialComplete)
            return;

        _xp += 0.1f;

        float neededXp = GetXpThreshold(_lvl);

        if (_xp >= neededXp)
        {
            _xp = 0;
            _lvl++;

            OnLvlChange?.Invoke(_lvl);
        }

#if UNITY_EDITOR
        Debug.Log("Handled kill");
#endif
        OnXpChange?.Invoke(_xp);
    }
}