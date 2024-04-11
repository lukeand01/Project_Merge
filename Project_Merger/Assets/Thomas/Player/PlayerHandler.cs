using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;


    public PlayerController controller {  get; private set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        controller = GetComponent<PlayerController>();

        DontDestroyOnLoad(gameObject);
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

    public void ResetPlayer()
    {
        ClearPoints();
    }

     void ClearPoints()
    {
        points = 0;
        UIHandler.instance.playerUI.UpdatePointText(points);
    }
    
}
