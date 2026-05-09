using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [field: SerializeField] public bool HasLaserBeamLUpgrade {  get; private set; }

    public void EnableLaserUpgrade()
    {
        HasLaserBeamLUpgrade = true;
    }
}