using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ClickerDemoMainObject : MonoBehaviour
{

    #region Public Variables
    public GameObject meowText;
    #endregion

    #region Private / Hidden Variables
    private RectTransform mainSprite;
    private ParticleSystem clickParticle;
    private TextMeshProUGUI canvasPoints;
    private TextMeshProUGUI comboMark;
    private TextMeshProUGUI AutoclickMark;
    private TextMeshProUGUI ClickMark;


    #region Logical Values
    private bool mainObjectClickEffect;
    private bool startedCoroutine = true;
    private bool hastheComboStated = true;
    private bool shakeComboMarkActive = true;

    private int comboIndex = 0;
    #endregion

    #region Stats
    [HideInInspector]public int currentPoints = 0;
    public int clickedPoints = 0;

    private float clickValue = 1; //Value for each Click 
    public float clickValueMultiplier = 1; //Increaseable with Upgrade [Click Multiplier]

    public int autoClickNum = 0; //Increaseable with Upgrade [Number of Autoclickers]
    public float autoClickValueMultiplier = 1; //Increaseable with Upgrade [Autoclicker Multiplier]
    [Range(1, 0)] public float autoClickIntervalTime = 1;//Decreaseable with Upgrade [Autoclicker Interval Time]

    private float clickComboMultiplayer = 1; //Increaseble with Combo System (Only for Normal Clicks)
    #endregion

    private ClickerDemoUpgrades[] UpgradeList;
    private int UpgradeListIndex = 0;
    #endregion

    private void OnEnable()
    {
        mainSprite = transform.GetChild(0).GetComponent<RectTransform>();
        mainSprite.transform.localScale = new Vector3(.2f, .33f, .33f);
        clickParticle = transform.parent.GetChild(5).GetComponent<ParticleSystem>();

        canvasPoints = transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        comboMark = transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>();
        AutoclickMark = transform.parent.GetChild(6).GetComponent<TextMeshProUGUI>(); AutoclickMark.text = null;
        ClickMark = transform.parent.GetChild(7).GetComponent<TextMeshProUGUI>(); ClickMark.text = null;
        canvasPoints.text = 0.ToString();

        UpgradeList = transform.parent.GetChild(3).GetChild(1).GetComponentsInChildren<ClickerDemoUpgrades>(true);
    }

    public void ClickedObject()
    {
        int clickPointsValue = 0;
        if (!mainObjectClickEffect) 
        { 
            mainObjectClickEffect = true;
            clickParticle.Play();
            mainSprite.DOPunchScale(new Vector3(.25f, .36f, .36f), .2f).OnComplete(() => { mainObjectClickEffect = false; clickParticle.Stop();});
            canvasPoints.gameObject.transform.DOPunchScale(new Vector3(-.25f, -.36f, -.36f), .15f);
        }

        clickedPoints++;

        currentPoints += (int)((clickValue * clickComboMultiplayer) * clickValueMultiplier);
        canvasPoints.text = currentPoints.ToString(); //Update Canvas Text

        StartCoroutine(MeowTextEffect());

        #region Combo Interaction
        ShakeComboMark();
        if (hastheComboStated)
            StartCoroutine(ComboCounter());
        #endregion

        clickPointsValue = (int)((clickValue * clickComboMultiplayer) * clickValueMultiplier);
        ClickMark.text = "+ " + clickPointsValue.ToString();
        ClickMark.GetComponent<Animator>().enabled = true;
        ClickMark.GetComponent<Animator>().Play("ClickPointText");

        if (currentPoints >= UpgradeList[UpgradeListIndex].pointsToUnlock) 
        {
            if (UpgradeListIndex >= UpgradeList.Length) { return; }
            transform.parent.GetChild(3).GetChild(1).GetChild(UpgradeListIndex).gameObject.SetActive(true); 
            UpgradeListIndex++; 
        }
    }

    #region Meow Text Effect
    IEnumerator MeowTextEffect()
    {
        GameObject meowTextInstance;
        meowTextInstance = Instantiate(meowText, new Vector3(transform.position.x + Random.Range(-40, 40), transform.position.y + Random.Range(-40, 40), transform.position.z), Quaternion.Euler(0, 0, 0), transform.parent);
        yield return new WaitForSecondsRealtime(1f);
        Destroy(meowTextInstance);
        ClickMark.color = new Color(0, 0, 0, 0);
        ClickMark.GetComponent<Animator>().enabled = false;
    }
    #endregion

    #region Combo System
    IEnumerator ComboCounter()
    {
        hastheComboStated = false;
        int clickComboGoal = clickedPoints + 15;

        yield return new WaitForSecondsRealtime(4);

        if (clickedPoints >= clickComboGoal) { comboIndex++; if (comboIndex <= 0) { comboIndex = 1; } }
        else { comboIndex--; if(comboIndex >= 4) { comboIndex = 3; } }

        switch (comboIndex)
        {
            case 0:
                comboMark.text = "D";
                canvasPoints.color = new Color(255, 255, 255, 255);
                clickComboMultiplayer = 1;
                break;

            case 1:
                comboMark.text = "C";
                canvasPoints.color = new Color(255, 185, 185, 255);
                clickComboMultiplayer = 1.5f;
                break;

            case 2:
                comboMark.text = "B";
                canvasPoints.color = new Color(255, 127, 127, 255);
                clickComboMultiplayer = 2f;
                break;

            case 3:
                comboMark.text = "A";
                canvasPoints.color = new Color(255, 64, 64, 255);
                clickComboMultiplayer = 2.5f;
                break;

            case >= 4:
                comboMark.text = "S";
                canvasPoints.color = new Color(255, 0, 0, 255);
                clickComboMultiplayer = 3;
                break;
        }

        clickedPoints = 0;
        hastheComboStated = true;
    }

    private void ShakeComboMark()
    {
        if (shakeComboMarkActive) //Assure that the Tween has finished to avoid position recolocations
        {
            shakeComboMarkActive = false;
            Sequence ShakeSequence = DOTween.Sequence();
            ShakeSequence.Join(comboMark.gameObject.transform.DOShakePosition(0.1f, 5f, 15, 90, false, false)).Join(comboMark.gameObject.transform.parent.GetChild(1).transform.DOShakePosition(0.1f, 0.1f, 10, 90, false, false)).OnComplete(() => shakeComboMarkActive = true);
        }
    }
    #endregion

    #region Autoclick
    private void Update()
    {
        if (startedCoroutine) {StartCoroutine(Autoclick());}
    }
    IEnumerator Autoclick()
    {
        startedCoroutine = false;
        int autoclickPointsValue = 0;
        yield return new WaitForSecondsRealtime(1 * autoClickIntervalTime);

        currentPoints += (int)(clickValue * autoClickNum * autoClickValueMultiplier) ;
        canvasPoints.text = currentPoints.ToString(); //Update Canvas Text

        if (autoClickNum != 0)
        {
            autoclickPointsValue = (int)(clickValue * autoClickNum * autoClickValueMultiplier);
            AutoclickMark.text = "+ " + autoclickPointsValue.ToString();
            AutoclickMark.GetComponent<Animator>().Play("AutoClickPointText");
        }
        else { AutoclickMark.text = null; }

        startedCoroutine = true;
    }
    #endregion
}
