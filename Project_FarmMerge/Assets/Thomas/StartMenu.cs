using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    //we must show the loading
    //right at the start 
    //we make the name 
    [SerializeField] Image backgroundImage;


    private void Start()
    {
        StartCoroutine(StartProcess());
    }
    IEnumerator StartProcess()
    {
        float timer = 0.5f;

        yield return new WaitForSeconds(0.2f);

        GameHandler.instance.ballHandler.StartBallHandler();
        backgroundImage.DOFade(0, timer);
        yield return new WaitForSecondsRealtime(timer);
        gameObject.SetActive(false);

    }


}

