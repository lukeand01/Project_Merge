using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        InputInteractWithBall();
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (ClickedOverUI(mousePos)) return;

        if(Input.mousePosition.y > Screen.height /1.24f)
        {
            return;
        }

        


        if (Input.GetMouseButtonUp(0))
        {


            ballHandler.ReceiveDecisionInput();
            return;
        }

        if (!Input.GetMouseButton(0)) return;




        ballHandler.ReceiveMoveInput(mousePos);

    }

    void InputInteractWithBall()
    {
        //this will detect if you touched a ball but only ever if there is a power that requires it.

        //assing this as an event.

        bool canCheckThis = Input.touchCount > 0 || Input.GetMouseButtonDown(0);

        if (!canCheckThis)
        {

            return;
        }



        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


        //if the touch is in the toop

        if (hit.collider == null) return;


        if (hit.collider.gameObject.tag != "Ball") return;

        MergeBall merge = hit.collider.GetComponent<MergeBall>();
        
        if (merge == null) return;

        //so its always a power if you are interact.
        GameHandler.instance.powerHandler.UseCurrentPower(merge);

    }


    bool ClickedOverUI(Vector3 pos)
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
