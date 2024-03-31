using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeBallCanvas : MonoBehaviour
{
    //everytime they spawn they create one of these fellas.
    //

    //these thigns must be derived from an event system. makes everything simpler rather than calling everyone.

    [SerializeField] Image targetImage;

    float originalScale = 1;
    float alteredScale = 1.2f;

    private void Awake()
    {
        originalScale = 0.8f;
        alteredScale = 1.1f;
    }

    public void StartTarget()
    {
        StopAllCoroutines();
        StartCoroutine(StartTargetProcess());
    }
    public void StopTarget()
    {
        StopAllCoroutines();
        StopTargetProcess();
    }

    IEnumerator StartTargetProcess()
    {
        targetImage.DOKill();
        targetImage.transform.DOScale(0, 0);
        targetImage.gameObject.SetActive(true);
        targetImage.transform.DOScale(originalScale, 1);
        yield return new WaitForSecondsRealtime(1);

        StartCoroutine(TargetProcess());
    }
    IEnumerator StopTargetProcess()
    {
        targetImage.DOKill();
        targetImage.transform.DOScale(0, 1);
        yield return new WaitForSecondsRealtime(1);
        targetImage.gameObject.SetActive(true);

    }



    IEnumerator TargetProcess()
    {
        targetImage.DOKill();
        targetImage.transform.DOScale(originalScale, 0);

        float timer = 1f;

        targetImage.transform.DOScale(alteredScale, timer);

        yield return new WaitForSecondsRealtime(timer);

        targetImage.transform.DOScale(originalScale, timer);

        yield return new WaitForSecondsRealtime(timer);

        StartCoroutine (TargetProcess());
    }

}

//we need to lock when we are using an ability.
//we also need to ask for confirmation before using the ability.