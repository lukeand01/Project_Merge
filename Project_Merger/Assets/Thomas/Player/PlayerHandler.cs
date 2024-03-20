using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;

    private void Awake()
    {
        instance = this;
    }


    public int points {  get; private set; }

    public void AddPoints(int value)
    {
        points += value;
        UIHandler.instance.playerUI.UpdatePointText(points);
    }
    public void RemovePoints(int value)
    {
        points -= value;
        UIHandler.instance.playerUI.UpdatePointText(points);
    }
}
