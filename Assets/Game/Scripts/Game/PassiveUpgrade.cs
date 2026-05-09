using UnityEngine;

public class PassiveUpgrade : Upgrade
{
    [SerializeField] private GameObject[] _passivesToEnable;

    public override void Execute()
    {
        foreach (var passive in _passivesToEnable)
        {
            passive.SetActive(true);
        }

        _upgradeController.SetNextUpgradeWindow(_nextUpgradeWindow);
    }
}
