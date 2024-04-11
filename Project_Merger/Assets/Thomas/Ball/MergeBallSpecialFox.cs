using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBallSpecialFox : MergeBall
{

    //when it interacts it does something different 


    public override void SetUp(BallHandler handler)
    {
        this.handler = handler;
        isDropped = false;
        handler.eventBallOrder += ReceiveOrder;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MergeBall merge = collision.GetComponent<MergeBall>();

        if (merge == null) return;


        //we destroy the target if we are allowed to.]

        if ((int)merge.mergeType < 4)
        {
            //then we destroy it. otherwise we do nothing.
            //we destroy it.
            //but we need to give point for now we will simply destroy it.
            Destroy(merge.gameObject);

        }
    }

    protected override void HandleCollision(Collision2D collision)
    {
       //we do nothing about collision. but the problem is that we shoul
    }
}
