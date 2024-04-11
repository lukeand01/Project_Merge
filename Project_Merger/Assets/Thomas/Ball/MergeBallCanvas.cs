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

    [SerializeField] GameObject target;

    float originalScale = 1;
    float alteredScale = 1.2f;

    private void Awake()
    {
        originalScale = 0.8f;
        alteredScale = 1.1f;
    }

    private void Update()
    {
        if (target.activeInHierarchy)
        {
            target.transform.Rotate(new Vector3(0, 0, 45 * Time.deltaTime));
        }
    }


    public void StartTarget()
    {
        target.SetActive(true);
    }
    public void StopTarget()
    {
        target.SetActive(false);
    }

    

}

//we need to lock when we are using an ability.
//we also need to ask for confirmation before using the ability.