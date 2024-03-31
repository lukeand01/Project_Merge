using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MergeBall : MonoBehaviour
{
    
    
    //this instructs the other to come to it.
    //when ti detects that the other is close enough we destroy them
    

    Rigidbody rb;
    Rigidbody2D rb2;
    CircleCollider2D circleCollider;
    public MergeBallType mergeType;

    bool cannotCollide;
    bool mergeProcess;
    MergeBall mergeTarget;

     float mergeSpeed = 25;


    [SerializeField] MergeBallCanvas canvas;

    bool isDropped;

    BallHandler handler;

    

    public void SetUp(BallHandler handler)
    {



        this.handler = handler;

        isDropped = false;

        handler.eventBallOrder += ReceiveOrder;


    }
    private void OnDestroy()
    {
        if(handler != null)
        {
            handler.eventBallOrder -= ReceiveOrder;
        }
        
    }

    #region EVENT ORDERS
    public void ReceiveOrder(MergeBallType ballType, OrderWhoType orderWho, OrderWhatType orderWhat)
    {


        if (!isDropped) return;



        if (orderWho == OrderWhoType.All)
        {
            //then we do this.
            ReceiveWhatOrder(orderWhat);
            return;
        }
        if(orderWho == OrderWhoType.AllBelow && (int)ballType > (int)mergeType)
        {


            ReceiveWhatOrder(orderWhat);
            return;
        }
        if(orderWho == OrderWhoType.AllAbove && (int)ballType < (int)mergeType)
        {
            ReceiveWhatOrder(orderWhat);
            return; 
        }
        if (orderWho == OrderWhoType.Only && (int)ballType == (int)mergeType)
        {
            ReceiveWhatOrder(orderWhat);
            return;
        }

        Debug.Log("none found this fella ");
    }

    void ReceiveWhatOrder(OrderWhatType orderWhat)
    {
        if(orderWhat == OrderWhatType.Destroy)
        {
            Destroy(gameObject);
            return;
        }
        if(orderWhat == OrderWhatType.Upgrade)
        {
            //then we call for someone in this place.

            return;
        }

        if(orderWhat == OrderWhatType.StartTargetUI)
        {
            canvas.StartTarget();
            return;
        }
        if(orderWhat == OrderWhatType.StopTargetUI)
        {
            canvas.StopTarget();
            return;
        }
    }

    #endregion

    public void DropIt()
    {
        isDropped = true;
    }




    private void Awake()
    {

        mergeSpeed  = 3.5f;
        rb = GetComponent<Rigidbody>();
        rb2 = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();


    }


    private void Update()
    {
        if (!mergeProcess) return;
        if (mergeTarget == null) return;

        transform.position = Vector2.MoveTowards(transform.position, mergeTarget.transform.position, Time.deltaTime * mergeSpeed);

        float distance = Vector2.Distance(transform.position, mergeTarget.transform.position);

        if(distance < 0.1f)
        {
            Debug.Log("time to merge");
            CallMerge(mergeTarget);
        }

        mergeProcess = true;
        cannotCollide = true;
    }

    #region UNIVERSAL
    public bool CanMerge(int index)
    {
        return (int)mergeType == index;
    }
    public void ForceNoCollision()
    {
        cannotCollide = true;
    }
    void CallMerge(MergeBall merge)
    {
        merge.ForceNoCollision();
        ForceNoCollision();
        GameHandler.instance.ballHandler.SpawnNewAtPosition((int)mergeType, this, merge);
        OrderDestruction();
    }
    #endregion

    #region 3D
    RigidbodyConstraints rbConstraints;

    private void OnCollisionEnter(Collision collision)
    {
        if (mergeProcess) return;

        MergeBall merge = collision.collider.GetComponent<MergeBall>();

        if (merge == null) return;

        if (!merge.CanMerge((int)mergeType)) return;

        if (cannotCollide) return;

        CallMerge(merge);
    }

    public void MakeNotReactive()
    {
        if(rb != null)
        {
            rbConstraints = rb.constraints;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints -= RigidbodyConstraints.FreezePositionX;

        }

        if(rb2 != null)
        {
            rb2.gravityScale = 0;
        }


    }

    public void MakeReactive()
    {
        if(rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rb.constraints = rbConstraints;
        }

        if(rb2 != null)
        {
            rb2.gravityScale = 1;
        }



    }


    #endregion

    #region 2D

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mergeProcess) return;
        MergeBall merge = collision.collider.GetComponent<MergeBall>();

        if (merge == null) return;

        if (!merge.CanMerge((int)mergeType)) return;

        if (cannotCollide) return;

        //CallMerge(merge);
        //merge.StartMergeProcess(this);
        StartMergeProcess(merge);
    }

    public void MakeReactive2()
    {
       
    }


    #endregion


    void StartMergeProcess(MergeBall mergeTarget)
    {
        mergeProcess = true;
        this.mergeTarget = mergeTarget;

        rb2.velocity = Vector3.zero;

        rb2.gravityScale = 0;

        circleCollider.isTrigger = true;

    }
  
    public void OrderDestruction()
    {
        //award points for destruction as well as creation.
        PlayerHandler.instance.AddPoints(5);
        Destroy(gameObject);
    }
    public void OrderUpgrade()
    {
        handler.SpawnNewAtPosition((int)mergeType, this, null);
    }


    [ContextMenu("DEBUG CALL MENU")]
    public void DebugCallFade()
    {
        GameHandler.instance.ballHandler.DebugSpawnFade(transform.position);
    }

}

public enum MergeBallType
{
    Rat = 0,
    Bird = 1,
    Rabbit = 2,
    Chicken = 3,
    Cat = 4,
    Pig = 5,
    Sheep = 6,
    Dog = 7,
    Horse = 8,
    Cow = 9


}