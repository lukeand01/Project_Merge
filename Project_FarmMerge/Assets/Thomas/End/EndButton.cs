using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndButton : ButtonBase
{
    [SerializeField] Animator heart;
    [SerializeField] GameObject notUsableImage;


    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        heart.enabled = false;
    }




    DeathLine deathHandler;

    public void SetUp(DeathLine deathLine)
    {
        deathHandler = deathLine;
    }
    public void StartEndButton()
    {

        StopAllCoroutines();

        heart.transform.localScale = new Vector3(0.6f, 0.6f, 0);

        bool hasUsedHealth = GameHandler.instance.hasUsedHealth;

        heart.enabled = !hasUsedHealth;

        if (!hasUsedHealth)
        {
            
        }



        notUsableImage.SetActive(hasUsedHealth);

    }

    //cannot ever click on anything 



    //while the thing is thinkiung about it we cannot send another input.

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (notUsableImage.activeInHierarchy) return;
        base.OnPointerClick(eventData);
        deathHandler.ReceiveEndInputResumeGame();
    }
}
