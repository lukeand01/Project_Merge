using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;


    public PlayerController controller {  get; private set; }
    private void Awake()
    {
        instance = this;

        controller = GetComponent<PlayerController>();
    }

    //need to create effect for the points going up.
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
