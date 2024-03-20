using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //this will control the input.

    BallHandler ballHandler;

    //what it will try will be to always follow the finger.

    private void Start()
    {
        ballHandler = GameHandler.instance.ballHandler;
    }


    private void Update()
    {
        //InputControlBall();
        InputControlBallForMouse();
    }


    void InputControlBall()
    {
        //it works as following: you can control from one side to another
        //if you release the touch while below a certain line then it lets the thing fall.
        //otherwise it cancels it.

        if(Input.touchCount <= 0)
        {
            //then we allow it.
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        ballHandler.ReceiveMoveInput(touchPos);


        //this will simply deal with 

    }

    void InputControlBallForMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("called");
            ballHandler.ReceiveDecisionInput();
            return;
        }

        if (!Input.GetMouseButton(0)) return;

   

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ballHandler.ReceiveMoveInput(mousePos);

    }

}
