using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUnit : ButtonBase
{
    public PowerType PowerType;
    public int powerAmmo; //how many times you can use without watching an ad.

    [SerializeField] InputUI inputHandler;
    [SerializeField] GameObject graphicalHolder;
    [SerializeField] TextMeshProUGUI powerAmmoText;
    [SerializeField] GameObject powerAmmoHolder;
    [SerializeField] GameObject powerAdIcon;
    [SerializeField] TextMeshProUGUI effectText;


    Vector3 effectTextOriginalPos;
    
    private void Awake()
    {
        UpdateAmmo(powerAmmo);

        effectTextOriginalPos = effectText.transform.localPosition;
    }


    public void UpdateAmmo(int ammo)
    {
        powerAmmo = ammo;
        powerAmmoText.text = powerAmmo.ToString();

        powerAmmoHolder.gameObject.SetActive(powerAmmo > 0);
        powerAdIcon.SetActive(powerAmmo <= 0);

    }


    public void ChangeAmmo(int ammo)
    {


        powerAmmo += ammo;
        powerAmmo = Mathf.Clamp(powerAmmo, 0, 10);
        UpdateAmmo(powerAmmo);

        if(ammo > 0)
        {
            Debug.Log("this was called");
            StopCoroutine(nameof(DanceEffectProcess));
            StartCoroutine(DanceEffectProcess());
        }


        if(ammo <= -1)
        {
            Debug.Log("ammo should never go this low");
        }

        if(effectCourotine != null)
        {
            StopCoroutine(effectCourotine);
        }
        
      effectCourotine = StartCoroutine(EffectTextProcess(ammo));

    }


    Coroutine effectCourotine;

    IEnumerator EffectTextProcess(int value)
    {

        float timer = 1f;

        effectText.gameObject.SetActive(true);

        effectText.transform.localPosition = effectTextOriginalPos;
        effectText.transform.DOLocalMove(effectTextOriginalPos + new Vector3(0, 15, 0), timer);

        effectText.DOFade(0, 0);
        effectText.DOFade(1, timer * 0.4f);

        effectText.text = "";

        if(value > 0)
        {
            effectText.color = Color.green;
            effectText.text += "+";
        }
        if(value < 0)
        {
            effectText.color = Color.red;

        }

        effectText.text += value.ToString();

        yield return new WaitForSeconds(timer);

        effectText.DOFade(0, timer);



    }


    IEnumerator DanceEffectProcess()
    {
        //it shakes to better show that this is the target.

        float offsetValue = 3.5f;
        for (int i = 0; i < 25; i++)
        {
            float x = Random.Range(-offsetValue, offsetValue);
            float y = Random.Range(-offsetValue, offsetValue);
            graphicalHolder.transform.localPosition = Vector3.zero + new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.02f);
        }


        graphicalHolder.transform.localPosition = Vector3.zero;



    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        UIHandler.instance.inputUI.StartPower(this);


    }

    [ContextMenu("DEBUG REDUCE AMMO")]
    public void DebugReduceAmmo()
    {
        ChangeAmmo(-1);
    }

    [ContextMenu("DEBUG INCREASE AMMO")]
    public void DebugIncreaseAmmo()
    {
        ChangeAmmo(1);
    }


    

}


public enum PowerType
{
    Wipe,
    Upgrade,
    DestroyTarget
}