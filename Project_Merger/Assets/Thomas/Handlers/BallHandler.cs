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

    MergeBall currentMergeBall;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform spawnPosition;
    [SerializeField] LineRenderer line;
    [SerializeField] List<MergeBall> mergeBallList = new();
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

    private void Awake()
    {
        GetMergeBallRef();

        


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
            CreateNextQueueItem();
        }
    }
    void CreateNextQueueItem()
    {
        int random = UnityEngine.Random.Range(0, 4);
        queueList.Add(mergeBallList[random]);
    }

    public void SpawnNewBall()
    {
        //we get a random selection of the lowers balls.
        isHolding = false;

        MergeBall newObject = Instantiate(queueList[0], spawnPosition.position, Quaternion.identity);
        ChangeScale(newObject.transform, (int)queueList[0].mergeType);
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

    public void SpawnNewAtPosition(int index, GameObject firstBall, GameObject secondBall)
    {
        

        if(index > mergeBallList.Count)
        {
            Debug.Log("then we just destroy this felçla");
            Destroy(firstBall.gameObject);
            Destroy(secondBall.gameObject);
            return;
        }



        MergeBall newObject = Instantiate(mergeBallList[index + 1], firstBall.transform.position, Quaternion.identity);
        ChangeScale(newObject.transform, index + 1);
        PlayerHandler.instance.AddPoints(5);


        Destroy(firstBall.gameObject);
        Destroy(secondBall.gameObject);

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

        Debug.Log("this was called");
        if(currentMergeBall == null) return;

        currentMergeBall.MakeReactive();
        currentMergeBall = null;
        isHolding = false;
        Invoke(nameof(SpawnNewBall), 0.5f);
    }

}

