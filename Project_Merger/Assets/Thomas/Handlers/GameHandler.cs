using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public BallHandler ballHandler {  get; private set; }


    private void Awake()
    {
        instance = this;

        ballHandler = GetComponent<BallHandler>();

    }

    public void CreateSFX(AudioClip clip)
    {

    }
}
