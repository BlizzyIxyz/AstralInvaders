using UnityEngine;

public class ProjUpgrade : Upgrade
{
    [SerializeField] private PlayerAttack _newAttackStrategy;

    public override void Execute()
    {
        _playerInertial.SetAttackStrategy(_newAttackStrategy);

        _upgradeController.SetNextUpgradeWindow(_nextUpgradeWindow);
    }
}