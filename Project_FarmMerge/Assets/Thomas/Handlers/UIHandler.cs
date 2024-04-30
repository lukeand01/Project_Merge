using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;


    public PlayerUI playerUI;
    public ConfirmationUI confirmationUI;
    public QueueUI queueUI;
    public InputUI inputUI;
    public DeathLine deathLineUI;

    [Separator("POSITION REFERENCES")]
    public Transform spawnPosRef;
    public Transform leftWallPosRef;
    public Transform rightWallPosRef;
    public Transform groundPosRef;
    public Transform topLinePosRef;

    [Separator("Down Below")]
    [SerializeField] Image mergeBonusBar;
    [SerializeField] Image mergeBonusGiftBar;
    [SerializeField] TextMeshProUGUI mergeBonusText;
    [SerializeField] Transform mergeBonusHolder;
    [SerializeField] GameObject mergeBonusHightlight;
    List<ParticleSystem> bonusParticleList = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        speed = 50;
        DontDestroyOnLoad(gameObject);
    }

    //the fox consuming the thing also cause 


    float speed;

    private void Update()
    {
        HandleBonusValue();
        HandleBonusGiftValue();

    }
    float bonusCurrent;
    float bonusUsingValue;
    float bonusTotal;

    void HandleBonusValue()
    {
        //Debug.Log("current is: " + bonusCurrent + " total is: " + bonusTotal);


        if (bonusUsingValue > bonusCurrent)
        {
            bonusUsingValue -= Time.deltaTime * speed;
        }

        if (bonusUsingValue < bonusCurrent)
        {
            bonusUsingValue += Time.deltaTime * speed;
        }


        bonusUsingValue = Mathf.Clamp(bonusUsingValue, 0, bonusCurrent);

        //Debug.Log("this is the using value " + bonusUsingValue + " bohnuscurrent " + bonusCurrent + " bonus total " + bonusTotal) ;
        mergeBonusBar.fillAmount = bonusUsingValue / bonusTotal;
    }

    float bonusGiftCurrent;
    float bonusGiftUsingValue;
    float bonusGiftTotal;


    void HandleBonusGiftValue()
    {


        if (bonusGiftUsingValue > bonusGiftCurrent)
        {
            bonusGiftUsingValue -= Time.deltaTime * speed;
        }

        if (bonusGiftUsingValue < bonusGiftCurrent)
        {
            bonusGiftUsingValue += Time.deltaTime * speed;
        }



        mergeBonusGiftBar.fillAmount = bonusGiftUsingValue / bonusGiftTotal;
    }

    public void UpdateMergeBonusFill(float current, float total)
    {

        bonusCurrent = current;
        bonusTotal = total;
    }

    public void HardUpdateMergeBonusFill(float current, float total)
    {
        UpdateMergeBonusFill(current, total);
        bonusUsingValue = current;
    }

    public void UpdateMergeBonusGiftFill(float current, float total)
    {
        bonusGiftCurrent = current;
        bonusGiftTotal = total;
    }

    public void HardUpdateMergeBonusGiftFill(float current, float total)
    {
        UpdateMergeBonusGiftFill(current, total);
        bonusGiftUsingValue = current;
    }

    public void UpdateMergeText(string text)
    {
        mergeBonusText.text = text;
    }

    public void StopAllOperations()
    {
        StopAllCoroutines();
    }


    public void TriggerBonus()
    {
        mergeBonusHolder.transform.DOScale(1.08f, 0.3f);
        StartCoroutine(BonusIsHappeningProcess());
    }
    public void StopBonus()
    {
        mergeBonusHolder.transform.DOScale(1f, 0.3f);
        StopAllCoroutines();
        mergeBonusHightlight.SetActive(false);
    }

    IEnumerator BonusIsHappeningProcess()
    {
        mergeBonusHightlight.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        mergeBonusHightlight.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BonusIsHappeningProcess());
    }

  
    



}
