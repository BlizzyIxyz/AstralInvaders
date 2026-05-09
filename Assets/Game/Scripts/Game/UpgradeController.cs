using UnityEngine;
using UnityEngine.Playables;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private PlayerLvlCounter _counter;
    [SerializeField] private GameObject _nextUpgradeWindow;

    private void Awake()
    {
        _counter.OnLvlChange += HandleLvlChange;
    }

    private void HandleLvlChange(int lvl)
    {
        _nextUpgradeWindow.SetActive(true);
    }

    public void SetNextUpgradeWindow(GameObject playableWindow)
    {
        _nextUpgradeWindow = playableWindow;
    }
}