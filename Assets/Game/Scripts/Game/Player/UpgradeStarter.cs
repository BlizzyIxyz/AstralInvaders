using UnityEngine;

public class UpgradeStarter : MonoBehaviour
{
    [SerializeField] private UpgradeSlower _upgradeSlower;

    private void Start()
    {
        _upgradeSlower.Slow();
    }
}
