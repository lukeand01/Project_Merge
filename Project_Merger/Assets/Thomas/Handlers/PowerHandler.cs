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
        
    }


    public void StartUpgradePower()
    {
        //we tell to target anyone.
        //
        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Rabbit, OrderWhoType.All, OrderWhatType.StartTargetUI);
        confirmType = PowerConfirmType.Upgrade;
    }

    public void StartDestroyTarget()
    {

        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Rabbit, OrderWhoType.All, OrderWhatType.StartTargetUI);

        //with this order
        confirmType = PowerConfirmType.Destroy;
    }



    public void UseCurrentPower(MergeBall merge)
    {
        //we wioll give the information back here to decide what fella i started.

        Debug.Log("use current power");

        if (confirmType == PowerConfirmType.None) return;
        if (merge == null) return;

        BallHandler ballHandler = GameHandler.instance.ballHandler;
        ballHandler.OnBallOrder(MergeBallType.Rabbit, OrderWhoType.All, OrderWhatType.StopTargetUI);

        if(confirmType == PowerConfirmType.Destroy)
        {
            //need to call it destruction so award points
            merge.OrderDestruction();
            
        }

        if(confirmType == PowerConfirmType.Upgrade)
        {
            //
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
    }

    public bool IsPowerActive()
    {
        return confirmType != PowerConfirmType.None;
    }

}
