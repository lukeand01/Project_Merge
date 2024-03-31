using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;


    public PlayerUI playerUI;
    public ConfirmationUI confirmationUI;
    public QueueUI queueUI;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }
}
