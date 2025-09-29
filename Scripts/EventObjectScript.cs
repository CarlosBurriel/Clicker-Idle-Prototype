using UnityEngine;
using DG.Tweening;

public class EventObjectScript : MonoBehaviour
{
    private ClickerDemoMainObject mainObjectScript;
    public void EventObjectPressed()
    {
        mainObjectScript = FindAnyObjectByType<ClickerDemoMainObject>();
        mainObjectScript.currentPoints += (mainObjectScript.currentPoints / 3);
        transform.DOScale(new Vector3(0, 0, 0), 0.2f).OnComplete(() => Destroy(transform.parent.gameObject));
    }
}
