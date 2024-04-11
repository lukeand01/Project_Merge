using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    
    //this will actually just sign 

    ConfirmationUI confirmationUI;

    [SerializeField] GameObject buttonCancelPower;
    [SerializeField] List<PowerUnit> powerUnitList = new();


    private void Start()
    {
        confirmationUI = UIHandler.instance.confirmationUI;
    }


    PowerUnit currentPowerUnit;


    public void ChangeEspecificPowerAmmo(PowerType power, int value)
    {
        foreach (var item in powerUnitList)
        {
            if(item.PowerType == power)
            {
                item.ChangeAmmo(value);
                return;
            }
        }
    }

    

    public void StartPower(PowerUnit powerUnit)
    {
        //this will take any of the arguemtn. but then i will use the information inside to decide on what to do

        if(currentPowerUnit != null)
        {
            Debug.Log("there is already a power here");
            return;
        }

        currentPowerUnit = powerUnit;


        bool hasEnoughAmmo = powerUnit.powerAmmo > 0;
        confirmationUI.UpdateButton(hasEnoughAmmo);


        if (hasEnoughAmmo)
        {
            confirmationUI.eventConfirm += CallCurrentPower;
            confirmationUI.eventConfirm += ClearConfirmationMenu;
        }
        else
        {
            confirmationUI.eventConfirm += CallAd;
        }



        confirmationUI.eventCancel += ClearConfirmationMenu;


        string title = "";
        string description = "";

        if (currentPowerUnit.PowerType == PowerType.Wipe)
        {
            title = "Wipe";
            description = "Destroy all below chicken (but not the chickens)";
        }
        if (currentPowerUnit.PowerType == PowerType.DestroyTarget)
        {
            title = "Destroy one target";
            description = "Select any target you want. it will be destroyed.";
        }
        if (currentPowerUnit.PowerType == PowerType.Upgrade)
        {
            title = "Upgrade";
            description = "Select any target you want. it will be upgraded to the next tier. Only cows are immune to this";
        }


        confirmationUI.StartConfirmationWindow(title, description);
    }

    void CallAd()
    {
        //this will call the ad instead. informatin

        if (currentPowerUnit.PowerType == PowerType.Wipe)
        {
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.UsePowerWipe);
        }
        if (currentPowerUnit.PowerType == PowerType.DestroyTarget)
        {
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.UsePowerDestroy);
        }
        if (currentPowerUnit.PowerType == PowerType.Upgrade)
        {
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.UsePowerUpgrade);
        }

    }
    public void CallPowerFromAd(PowerType power)
    {

        ChangeEspecificPowerAmmo(currentPowerUnit.PowerType, 1);

        if (power == PowerType.Wipe)
        {
            GameHandler.instance.powerHandler.DestroyAllBelowChicken();
        }
        if (power == PowerType.DestroyTarget)
        {
            GameHandler.instance.powerHandler.StartDestroyTarget();
        }
        if (power == PowerType.Upgrade)
        {
            GameHandler.instance.powerHandler.StartUpgradePower();
        }

        ClearConfirmationMenu();
    }

    public void CallCurrentPower()
    {
        //this will check what to do.

        //it will watch then call this.

        if(currentPowerUnit == null)
        {
            Debug.Log("there was no cuyrrent power here");
            return;
        }

        Debug.Log("this is the power " + currentPowerUnit.PowerType);

        if(currentPowerUnit.PowerType == PowerType.Wipe)
        {
            GameHandler.instance.powerHandler.DestroyAllBelowChicken();
        }
        if (currentPowerUnit.PowerType == PowerType.DestroyTarget)
        {
            GameHandler.instance.powerHandler.StartDestroyTarget();
        }
        if (currentPowerUnit.PowerType == PowerType.Upgrade)
        {
            GameHandler.instance.powerHandler.StartUpgradePower();
        }

        //currentPowerUnit.ChangeAmmo(-1);

        currentPowerUnit = null;
    }

    


    void ClearConfirmationMenu()
    {
        confirmationUI.CloseConfirmationWindow();
        currentPowerUnit = null;
    }


    
    
    public void StartButtonCancelPower()
    {
        buttonCancelPower.gameObject.SetActive(true);

        //buttonCancelPower.transform.rotation = new Vector3();
        buttonCancelPower.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        StopAllCoroutines();
        StartCoroutine(MoveButtonCancelPowerProcess());
    }
    public void StopButtonCancelPower()
    {
        buttonCancelPower.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator MoveButtonCancelPowerProcess()
    {
        float timer = 0.8f;
        buttonCancelPower.transform.DORotate(new Vector3(0, 0, 4), timer);

        yield return new WaitForSecondsRealtime(timer);

        buttonCancelPower.transform.DORotate(new Vector3(0, 0, -4), timer);

        yield return new WaitForSecondsRealtime(timer);

        StartCoroutine(MoveButtonCancelPowerProcess());
    }


    public void OrderToStopPower()
    {
        GameHandler.instance.powerHandler.StopPower();
    }


    public void ResetPowerAmmo()
    {
        foreach (var item in powerUnitList)
        {
            item.UpdateAmmo(0);
        }
    }
}
