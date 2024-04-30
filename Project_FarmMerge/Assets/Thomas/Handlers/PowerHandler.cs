using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHandler : MonoBehaviour
{
    //powers:
    //destroy target.
    //destroy all small ones.
    //advance target to next level.

    //can never be using more than one power at  a atime.


    PowerConfirmType confirmType = PowerConfirmType.None;
    enum PowerConfirmType 
    { 
        None,
        Destroy,
        Upgrade
    }

    


    public void StopPower()
    {
        UIHandler.instance.inputUI.StopButtonCancelPower();
        confirmType = PowerConfirmType.None;


        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Rabbit, OrderWhoType.All, OrderWhatType.StopTargetUI);
    }


    public void StartUpgradePower()
    {
        //we tell to target anyone.
        //

        Debug.Log("this was called");
        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Cat, OrderWhoType.AllBelow, OrderWhatType.StartTargetUI);
        confirmType = PowerConfirmType.Upgrade;

        UIHandler.instance.inputUI.StartButtonCancelPower();
    }

    public void StartDestroyTarget()
    {

        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Dog, OrderWhoType.AllBelow, OrderWhatType.StartTargetUI);

        //with this order
        confirmType = PowerConfirmType.Destroy;

        UIHandler.instance.inputUI.StartButtonCancelPower();
    }


    public void UseCurrentPower(MergeBall merge)
    {
        //we wioll give the information back here to decide what fella i started.



        if (confirmType == PowerConfirmType.None) return;
        if (merge == null) return;

        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Rabbit, OrderWhoType.All, OrderWhatType.StopTargetUI);

        if(confirmType == PowerConfirmType.Destroy)
        {
            //need to call it destruction so award points
            UIHandler.instance.inputUI.ChangeEspecificPowerAmmo(PowerType.DestroyTarget, -1);
            UIHandler.instance.inputUI.StopButtonCancelPower();
            GameHandler.instance.ballHandler.AwardPoint(merge);
            merge.DestructionEffect();
            
        }

        if(confirmType == PowerConfirmType.Upgrade)
        {
            //
            UIHandler.instance.inputUI.ChangeEspecificPowerAmmo(PowerType.Upgrade, -1);
            UIHandler.instance.inputUI.StopButtonCancelPower();
            merge.OrderUpgrade();
        }

        Invoke(nameof(ResetConfirm), 0.3f);
        
    }


    void ResetConfirm()
    {
        confirmType = PowerConfirmType.None;
    }

    public void DestroyAllBelowChicken()
    {
        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Chicken, OrderWhoType.AllBelow, OrderWhatType.Destroy);
        Invoke(nameof(ResetConfirm), 0.3f);

        UIHandler.instance.inputUI.ChangeEspecificPowerAmmo(PowerType.Wipe, -1);
    }

    public bool IsPowerActive()
    {
        return confirmType != PowerConfirmType.None;
    }

}
