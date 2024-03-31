using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    //everytime it requires.
    GameHandler handler;

    MergeBall currentMergeBall;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform spawnPosition;
    [SerializeField] LineRenderer line;
    [SerializeField] List<MergeBall> mergeBallList = new();
    [SerializeField] List<MergeBall> specialMergeBallList = new(); //
    Dictionary<MergeBallType, MergeBall> mergeBallDictionary = new();

    [SerializeField]List<MergeBall> queueList = new();

    [SerializeField] Transform leftLimitation;
    [SerializeField] Transform rightLimitation;
    [SerializeField] Transform lineEnd;

    Vector3 lastPos;

    bool hasStarted;

    [Separator("DEBUG")]
    [SerializeField] bool debugStart;

    bool isHolding;


    //things i might want to call
    //destroy the fellas
    //activate ui on them.

    public Action<MergeBallType, OrderWhoType, OrderWhatType> eventBallOrder;
    public void OnBallOrder(MergeBallType mergeBallType, OrderWhoType whoOrder, OrderWhatType whatOrder) => eventBallOrder?.Invoke(mergeBallType, whoOrder, whatOrder);



    



    private void Awake()
    {
        GetMergeBallRef();
        handler = GetComponent<GameHandler>();  
        


    }

    private void Start()
    {     
        CreateQueueList();
        SpawnNewBall();
    }


    void GetMergeBallRef()
    {
        foreach (var item in mergeBallList)
        {
            mergeBallDictionary.Add(item.mergeType, item);            
        }
    }

    void CreateQueueList()
    {
        int queue = 4;

        for (int i = 0; i < queue; i++)
        {
            CreateNextQueueItem(true);
        }
    }
    void CreateNextQueueItem(bool firstBatch = false)
    {
        int specialChance = UnityEngine.Random.Range(0, 100);

        if(specialChance > 80 && !firstBatch)
        {
            //then we take it from the especial list.
            int randomSpecial = UnityEngine.Random.Range(0, specialMergeBallList.Count);
            queueList.Add(mergeBallList[randomSpecial]);
            UIHandler.instance.queueUI.AddToQueue(specialMergeBallList[randomSpecial]);
            return;
        }

        int random = UnityEngine.Random.Range(0, 4);
        queueList.Add(mergeBallList[random]);
        UIHandler.instance.queueUI.AddToQueue(mergeBallList[random]);
    }




    public void SpawnNewBall()
    {
        //we get a random selection of the lowers balls.


        isHolding = false;

        UIHandler.instance.queueUI.RemoveFromQueue();
        
        MergeBall newObject = Instantiate(queueList[0], spawnPosition.position, Quaternion.identity);
        newObject.SetUp(this);
        newObject.transform.Rotate(new Vector3(0, 180, 0));
        //ChangeScale(newObject.transform, (int)queueList[0].mergeType);
        newObject.MakeNotReactive();
        currentMergeBall = newObject;

        lastPos = spawnPosition.position;


        queueList.RemoveAt(0);
        CreateNextQueueItem();
    }

    //now we inform that we have someone waiting.
    private void Update()
    {
        //it keep trying to follow the touch lastposition

        line.gameObject.SetActive(currentMergeBall != null && isHolding);

        if(currentMergeBall == null) return;
        Vector3 newPosition = Vector3.Lerp(currentMergeBall.transform.position, lastPos, moveSpeed * Time.deltaTime);
        currentMergeBall.transform.position = newPosition;
        line.transform.position = newPosition;

        newPosition.y = lineEnd.transform.position.y;
        lineEnd.transform.position = newPosition;

        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, lineEnd.position);

    }

    public void SpawnNewAtPosition(int index, MergeBall firstBall, MergeBall secondBall)
    {
        

        if(index > mergeBallList.Count)
        {
            Debug.Log("then we just destroy this felçla");
            Destroy(firstBall.gameObject);
            if (secondBall != null) secondBall.OrderDestruction();
            return;
        }

        float turnValueModifier = GetTurnModifier(firstBall.transform.position);

        MergeBall newObject = Instantiate(mergeBallList[index + 1], firstBall.transform.position, Quaternion.identity);
        newObject.SetUp(this);
        newObject.transform.Rotate(new Vector3(0, 180, 0));
        //ChangeScale(newObject.transform, index + 1);

        float score = GetMergeTypeScore(newObject.mergeType);
        PlayerHandler.instance.AddPoints((int)score * (int)turnValueModifier);

        firstBall.OrderDestruction();

        if (secondBall != null) secondBall.OrderDestruction();

    }


    float GetMergeTypeScore(MergeBallType mergeType)
    {
        return ((float)mergeType * (float)mergeType) + 1 * 1.5f;
    }
    float GetSpawnTypeScore(MergeBallType mergeType)
    {
        return ((float)mergeType + 1) * 1.5f;
    }

    void ChangeScale(Transform target, int index)
    {
        float sizeValue = GetSizeForBallType(index);
        Vector3 size = new Vector3(sizeValue, sizeValue, sizeValue);
        target.transform.localScale = size;
    }

    float GetSizeForBallType(int index)
    {
        float baseSize = 0.8f;
        float increase = 0.2f * index;
        return baseSize + increase;
    }


    public void ReceiveMoveInput(Vector3 pos)
    {

        if (handler.powerHandler.IsPowerActive()) return;
        if (currentMergeBall == null) return;
        float clampedValueX = pos.x;
        clampedValueX = Mathf.Clamp(clampedValueX, leftLimitation.transform.position.x, rightLimitation.transform.position.x);
        lastPos = new Vector3(clampedValueX, currentMergeBall.transform.position.y, currentMergeBall.transform.position.z);
        isHolding = true;
        //and there are limits.

    }
    public void ReceiveDecisionInput()
    {
        //if you cancel or act.
        //this is called on 

        if (handler.powerHandler.IsPowerActive()) return;
        if (currentMergeBall == null) return;

        int score = (int)GetSpawnTypeScore(currentMergeBall.mergeType);
        PlayerHandler.instance.AddPoints(score);

        currentMergeBall.MakeReactive();
        currentMergeBall.DropIt();
        currentMergeBall = null;
        isHolding = false;
        Invoke(nameof(SpawnNewBall), 0.5f);

        

        ResetTurn();
    }


    #region ROUND
    //everytime 
    [Separator("ROUND")]
    [SerializeField] FadeUI fadeTemplate;

    int mergeSpree;

    void ResetTurn()
    {
        //this happens when you throw another ball

        mergeSpree = 0;
    }

    float GetTurnModifier(Vector3 mergePos)
    {
        //we call this everytime we merge two fellas.
        //we create the commemoration here and give especial bonus based in the mergespree
        mergeSpree += 1;
        float scoreModifier = 1;


        if(mergeSpree > 1)
        {
            //thebn we start doing this stuff.

            FadeUI newObject = Instantiate(fadeTemplate, mergePos, Quaternion.identity);
            newObject.transform.SetParent(UIHandler.instance.transform);
            newObject.SetUp(scoreModifier.ToString() + "X", Utils.GetRandomColor());
            newObject.ChangeScaleModifier(1.2f);
            newObject.ChangeColorModifier(0.6f);

            Debug.Log("called fade ui");
        }


        return scoreModifier;
    }

    public void DebugSpawnFade(Vector3 pos)
    {
        FadeUI newObject = Instantiate(fadeTemplate, pos, Quaternion.identity);
        newObject.transform.SetParent(UIHandler.instance.transform);
        newObject.SetUp("teste", Utils.GetRandomColor());
    }

    #endregion

}

public enum OrderWhoType
{
    All,
    AllBelow,
    AllAbove,
    Only

}
public enum OrderWhatType
{
    Destroy,
    Upgrade,
    StartTargetUI,
    StopTargetUI

}