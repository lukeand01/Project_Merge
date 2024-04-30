using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationUI : MonoBehaviour
{
    //this receives informations and links itself to whatever sent the request for confirmation. then it send the choice back.

    GameObject holder;

    //they will always be about watching ad.


    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject buttonHolder;

    [SerializeField] TextMeshProUGUI confirmText;

    bool inProcess = false;

    public Action eventConfirm;
    public Action eventCancel;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }


    void ClearEvents()
    {
        eventCancel = delegate { };
        eventConfirm = delegate { };
    }

    public void StartConfirmationWindow(string title, string description, bool hasScreenButton = false)
    {
        //you assign something and then send the information to show here.
        if (holder.activeInHierarchy)
        {
            Debug.Log("there is holder already");
            return;
        }

        //screenButton.SetActive(hasScreenButton);

        //i also need to know the currency.



        titleText.text = title;
        descriptionText.text = description;
        StopAllCoroutines();
        StartCoroutine(OpenProcess());

    }

    public void UpdateButton(bool hasAmmo)
    {
        //i will decide if it should show ""

        if (hasAmmo)
        {
            confirmText.text = "Use";
        }
        else
        {
            confirmText.text = "Watch an ad";
        }


    }



    public void CloseConfirmationWindow()
    {
        ClearEvents();

        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

    IEnumerator OpenProcess()
    {
        holder.SetActive(true);
        holder.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);


        float timeForAnimation = 0.5f;
        holder.transform.DOScale(1, timeForAnimation);

        yield return new WaitForSecondsRealtime(timeForAnimation);
    }

    IEnumerator CloseProcess()
    {
        ClearEvents();

        float timeForAnimation = 0.5f;
        holder.transform.DOScale(0.1f, timeForAnimation);
        yield return new WaitForSecondsRealtime(timeForAnimation);
        holder.SetActive(false);

    }


    public void Confirm()
    {

        if (eventConfirm != null)
        {
            eventConfirm.Invoke();
        }
    }
    public void Cancel()
    {
        if (eventCancel != null)
        {
            eventCancel.Invoke();
        }
    }

    public void ForceClose()
    {
        Debug.Log("force close was clicked");
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

    public bool IsActive()
    {
        return holder.activeInHierarchy;
    }
}
