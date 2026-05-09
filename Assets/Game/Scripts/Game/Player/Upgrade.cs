using UnityEngine;
using UnityEngine.Playables;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] protected GameObject _nextUpgradeWindow;
    [SerializeField] protected UpgradeController _upgradeController;
    [SerializeField] protected PlayerInertial _playerInertial;

    public abstract void Execute();
}