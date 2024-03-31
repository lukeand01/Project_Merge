using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    
    //this will actually just sign 

    ConfirmationUI confirmationUI;


    private void Start()
    {
        confirmationUI = UIHandler.instance.confirmationUI;
    }

    public void OrderPowerDestroy()
    {


        confirmationUI.StartConfirmationWindow("Buy Power", "Destroy a fella");

        confirmationUI.eventConfirm += CallPowerDestroy;
        confirmationUI.eventConfirm += ClearConfirmationMenu;
        confirmationUI.eventCancel += ClearConfirmationMenu;

    }

    void CallPowerDestroy()
    {
        GameHandler.instance.powerHandler.StartDestroyTarget();

    }

    public void OrderPowerWipe()
    {
        confirmationUI.StartConfirmationWindow("Buy Power", "Wipe all below chicken");

        confirmationUI.eventConfirm += CallPowerWipe;
        confirmationUI.eventConfirm += ClearConfirmationMenu;
        confirmationUI.eventCancel += ClearConfirmationMenu;
    }

    void CallPowerWipe()
    {
        GameHandler.instance.powerHandler.DestroyAllBelowChicken();
    }

    public void OrderPowerUpgrade()
    {
        confirmationUI.StartConfirmationWindow("Buy Power", "Upgrade target to the next tier");

        confirmationUI.eventConfirm += CallPowerUpgrade;
        confirmationUI.eventConfirm += ClearConfirmationMenu;
        confirmationUI.eventCancel += ClearConfirmationMenu;
    }
    void CallPowerUpgrade()
    {
        GameHandler.instance.powerHandler.StartUpgradePower();
    }

    void ClearConfirmationMenu()
    {
        confirmationUI.CloseConfirmationWindow();
    }


}
