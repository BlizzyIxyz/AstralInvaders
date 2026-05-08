using System;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public event Action OnGameStart;

    private bool _gameStarted;

    public void StartGame()
    {
        if (_gameStarted)
            return;

        _gameStarted = true;

        OnGameStart?.Invoke();
    }
}