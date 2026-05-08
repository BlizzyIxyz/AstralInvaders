using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameStartCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _cutscene;
    [SerializeField] private GameStarter _gameStarter;
    [SerializeField] private PlayerInertial _player;

    public event Action OnCutsceneEnd;

    private void Awake()
    {
        _gameStarter.OnGameStart += StartCutscene;
    }

    private void StartCutscene()
    {
        _cutscene.Play();
    }

    public void HandleCutsceneEnd()
    {
        _player.Enable();
        OnCutsceneEnd?.Invoke();
    }
}