using System;
using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameStartCutscene _gameStartCutscene;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private WaveController _waveController;

    public bool MovementTutorialComplete { get; private set; }
    public bool EnemyTutorialComplete { get; private set; }

    public event Action OnMovementTutorialStart;
    public event Action OnMovementTutorialCompleate;
    public event Action OnWaveTutorialCompleate;

    private void Awake()
    {
        _gameStartCutscene.OnCutsceneEnd += StartTutorial;
    }

    private void StartTutorial()
    {
        if (PlayerPrefs.GetInt("tu_co") == 1)
        {
            SkipTutorial();
            return;
        }

        StartCoroutine(TutorialCoroutine());
    }

    private IEnumerator TutorialCoroutine()
    {
        OnMovementTutorialStart?.Invoke();
        yield return new WaitUntil(() => _playerInput.MoveInput.sqrMagnitude > 0.01f);
        MovementTutorialComplete = true;
        OnMovementTutorialCompleate?.Invoke();

        StartCoroutine(_waveController.StartTutorialWave());
        yield return new WaitUntil(() => _waveController.TutorialWaveComplete);
        OnWaveTutorialCompleate?.Invoke();

        PlayerPrefs.SetInt("tu_co", 1);
        PlayerPrefs.Save();
    }

    private void SkipTutorial()
    {
        MovementTutorialComplete = true;
        EnemyTutorialComplete = true;

        _waveController.SkipTutorial();
        OnWaveTutorialCompleate?.Invoke();
    }
}