using UnityEngine;

public class UpgradeSlower : MonoBehaviour
{
    [SerializeField] private GameSlower _gameSlower;

    public void Slow()
    {
        _gameSlower.SlowDown(0.02f);
    }

    public void Reset()
    {
        _gameSlower.SpeedUp();
    }
}
