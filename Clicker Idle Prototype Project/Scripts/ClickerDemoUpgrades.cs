using TMPro;
using UnityEngine;
using static ClickerDemoUpgradeSO;

public class ClickerDemoUpgrades : MonoBehaviour
{
    public ClickerDemoUpgradeSO upgradeData;
    
    [HideInInspector]public int pointsToUnlock;
    public int upgradeCost;
    public ClickerDemoMainObject mainObject; // Convert To Singleton

    private TextMeshProUGUI CostText;

    private void Start()
    {
        CostText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        CostText.text = upgradeCost.ToString();

        pointsToUnlock = upgradeCost / 3;

        gameObject.SetActive(false);
    }

    public void BuyUpgrade() // ERROR TO ACCESS TO upgradeData
    {
        if(mainObject.currentPoints >= upgradeCost)
        {
            mainObject.currentPoints -= upgradeCost; //Spend Points
        }
        else
        {
            //FEEDBACK
            print("Not Enough Points to Buy this Upgrade");
            return;
        }

        
        switch (upgradeData.UpgrType)
        {
            case UpgradeType.ClickValueIncrease:
                mainObject.clickValueMultiplier += upgradeData.ValueIncrease;
            break;
               
            case UpgradeType.AutoClickerNumberIncrease:
                mainObject.autoClickNum++;
            break;
                
            case UpgradeType.AutoClickerMultiplierIncrease:
                mainObject.autoClickValueMultiplier += upgradeData.ValueIncrease;
            break;
               
            case UpgradeType.AutoClickerIntervalTime:
                if (mainObject.autoClickIntervalTime <= 0) { mainObject.autoClickIntervalTime = 0; }
                else { mainObject.autoClickIntervalTime -= upgradeData.ValueIncrease; }
            break;

            default:
                Debug.LogWarning("Warninig: INVALID UPGRADE TYPE");
            break;

        }
        

        gameObject.SetActive(false);
    }
}
