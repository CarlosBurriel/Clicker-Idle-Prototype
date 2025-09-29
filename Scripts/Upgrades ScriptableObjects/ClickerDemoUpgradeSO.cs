using UnityEngine;

[CreateAssetMenu(fileName = "ClickerDemoUpgradeSO", menuName = "Scriptable Objects/ClickerDemoUpgradeSO")]
public class ClickerDemoUpgradeSO : ScriptableObject
{
    public enum UpgradeType{ ClickValueIncrease, AutoClickerNumberIncrease, AutoClickerIntervalTime, AutoClickerMultiplierIncrease };
    public UpgradeType UpgrType;

    public float ValueIncrease;
}
