using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI pointText;

    public void UpdatePointText(int value)
    {
        pointText.text = "Points: " + value.ToString();
    }

    [SerializeField] TextMeshProUGUI giftTitleText;

    [ContextMenu("DEBUG GAIN GIFT TEXT")]
    public void GainTextEffect()
    {
        StopAllCoroutines();
        giftTitleText.transform.DOKill();
        StartCoroutine(GiftTextProcess());
    }
    IEnumerator GiftTextProcess()
    {
        float timer = 0.5f;
        giftTitleText.transform.DOScale(1, timer);
       yield return new WaitForSecondsRealtime(timer);
        giftTitleText.transform.DOScale(0, timer);
    }



}
