using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameStartCutscene _gameStartCutscene;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private PlayableDirector _showHud;
    [SerializeField] private Tutorial _movementTutorial;
    [SerializeField] private Tutorial _enemyTutorial;

    public bool MovementTutorialComplete { get; private set; }
    public bool EnemyTutorialComplete { get; private set; }

    public event Action OnMovementTutorialStart;
    public event Action OnMovementTutorialCompleate;
    public event Action TutorialComplete;

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
        _movementTutorial.Show();
        OnMovementTutorialStart?.Invoke();
        yield return new WaitUntil(() => _playerInput.MoveInput.sqrMagnitude > 0.01f);
        MovementTutorialComplete = true;
        OnMovementTutorialCompleate?.Invoke();
        _movementTutorial.Hide();

        _enemyTutorial.Show();
        StartCoroutine(_waveController.StartTutorialWave());
        yield return new WaitUntil(() => _waveController.TutorialWaveComplete);
        TutorialComplete?.Invoke();
        EnemyTutorialComplete = true;
        _enemyTutorial.Hide();

        _showHud.Play();

        PlayerPrefs.SetInt("tu_co", 1);
        PlayerPrefs.Save();
    }

    private void SkipTutorial()
    {
        MovementTutorialComplete = true;
        EnemyTutorialComplete = true;

        _waveController.SkipTutorial();
        TutorialComplete?.Invoke();

        _showHud.Play();
    }
}