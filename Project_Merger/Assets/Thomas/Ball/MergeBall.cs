using DG.Tweening;
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

    public bool isDropped;

    protected BallHandler handler;


    public AudioClip mergeClip;
    
    

    public virtual void SetUp(BallHandler handler)
    {



        this.handler = handler;

        isDropped = false;

        circleCollider.isTrigger = false;
        //rb2.constraints = RigidbodyConstraints2D.FreezeRotation;

        handler.eventBallOrder += ReceiveOrder;

        

    }
    private void OnDestroy()
    {
        if(handler != null)
        {
            handler.eventBallOrder -= ReceiveOrder;
        }

        if (wasOutside)
        {
            //we say to inform that this thing is no longer outside.
            GameHandler.instance.ballHandler.RemoveToOutside();
            wasOutside = false;
        }
        
    }

    #region EVENT ORDERS
    public void ReceiveOrder(MergeBallType ballType, OrderWhoType orderWho, OrderWhatType orderWhat)
    {


        if (!isDropped)
        {
            Debug.Log("is not dropped");
            return;
        }



        if (orderWho == OrderWhoType.All)
        {
            //then we do this.
            ReceiveWhatOrder(orderWhat);
            return;
        }
        if(orderWho == OrderWhoType.AllBelow && (int)ballType >= (int)mergeType)
        {
            Debug.Log("this is below " + mergeType.ToString());
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
        if(orderWhat == OrderWhatType.Destroy && mergeType != MergeBallType.Cow)
        {
            Destroy(gameObject);
            return;
        }
        if(orderWhat == OrderWhatType.Upgrade && (int)mergeType > 4)
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


        total = 0.4f;
        current = 0;

        outsideTotal = 2;
    }


    float current;
    float total;


    float outsideCurrent;
    float outsideTotal;

    bool wasOutside;

    void CallOutside()
    {
        Debug.Log("calling this as an error");

        if (!wasOutside)
        {
            wasOutside = true;
            GameHandler.instance.ballHandler.AddToOutside();
        }

    }

    

    private void Update()
    {
        
        if(transform.localPosition.y > 650 && isDropped)
        {

            
            if(outsideCurrent >= outsideTotal)
            {
                CallOutside();
            }
            else
            {
                outsideCurrent += Time.deltaTime;
            }

        }
        else if(wasOutside)
        {
            //call this if none of them are affect
            wasOutside = false;
            GameHandler.instance.ballHandler.RemoveToOutside();
        }

        if (!mergeProcess)
        {

            if(tag == "Ball")
            {
                if (circleCollider.isTrigger)
                {
                    circleCollider.isTrigger = false;
                    rb2.gravityScale = 1;
                }



            }

            return;
        }


        if(mergeProcess && currentMergeTarget == null)
        {
            Debug.Log("there is nothing to follow");
            mergeProcess = false;
            circleCollider.isTrigger = false;
            rb2.gravityScale = 1;
        }

        if (mergeTarget == null) return;

        transform.position = Vector2.MoveTowards(transform.position, mergeTarget.transform.position, Time.deltaTime * mergeSpeed);

        float distance = Vector2.Distance(transform.position, mergeTarget.transform.position);

        if(distance < 0.1f)
        {
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

    public bool alreadyPushedToTheSide = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mergeProcess) return;

        if(!alreadyPushedToTheSide && collision.gameObject.tag == "Ball")
        {
            int randomSide = Random.Range(0, 2);

            if (randomSide == 0)
            {
                rb2.AddForce(Vector3.right * 0.3f, ForceMode2D.Impulse);
            }

            if (randomSide == 1)
            {
                rb2.AddForce(Vector3.left * 0.3f, ForceMode2D.Impulse);
            }

            alreadyPushedToTheSide = true;
        }

        if (!isDropped) return;



        HandleCollision(collision);
    }

    protected virtual void HandleCollision(Collision2D collision)
    {
        MergeBall merge = collision.collider.GetComponent<MergeBall>();

        if (merge == null) return;

        if (!merge.CanMerge((int)mergeType)) return;

        if (cannotCollide) return;

        if (!isDropped) return;

        //CallMerge(merge);
        //merge.StartMergeProcess(this);
        StartMergeProcess(merge);
    }

    public void MakeReactive2()
    {
       
    }


    #endregion


    #region SPRITE
    public Sprite sprite;


    #endregion

    MergeBall currentMergeTarget;

    protected void StartMergeProcess(MergeBall mergeTarget)
    {

        currentMergeTarget = mergeTarget;

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



    public void DestructionEffect()
    {
        rb2.gravityScale = 0;
        StopAllCoroutines();
        StartCoroutine(DestructionEffectProcess());
    }
    IEnumerator DestructionEffectProcess()
    {
        //it disappears and then is destroyed.
        transform.DOScale(0, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(gameObject);
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