using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class QueueUI : MonoBehaviour
{
    GameObject holder;
    List<Vector3> localPositions = new();
    [SerializeField] int showLimit;
    [SerializeField] Image imageTemplate;
    [SerializeField] MyGrid containerGrid;
    float xoffset;
    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
        xoffset = 70;

        localPositions = new()
        {
            new Vector3(50, -50, 0),
            new Vector3(126, -50, 0),
            new Vector3(202, -50, 0),
            new Vector3(278, -50, 0)
        };

    }

    public void SetLimit(int showLimit)
    {
        this.showLimit = showLimit;
    }


    


    private void Update()
    {
        //we can do that each part should be consantly moving to the end.

    }

    //i might be cool to show more fellas. so lets no throw the idea out just yet.
    List<Image> queueImageList = new();

    
    public void MoveQueue(MergeBall newBall)
    {
        StopAllCoroutines();
        StartCoroutine(MoveQueueProcess(newBall));
    }

    IEnumerator MoveQueueProcess(MergeBall newBall)
    {
        //we tell teh thing the first to disappear.

        //we disable the grid

        containerGrid.enabled = false;

        yield return null;

        queueImageList[0].transform.DOScale(0, 0.2f);


        Image newObject = Instantiate(imageTemplate);
        newObject.sprite = newBall.sprite;
        newObject.transform.SetParent(containerGrid.transform);
        Vector3 pos = queueImageList[queueImageList.Count - 1].transform.localPosition;
        newObject.transform.localPosition = pos + new Vector3(76, 0, 0);
        queueImageList.Add(newObject);

        newObject.transform.localScale = Vector3.zero;
        newObject.gameObject.SetActive(true);   

        yield return new WaitForSeconds(0.05f);

        for (int i = 1; i < queueImageList.Count; i++)
        {
            //each fella moves to the side at the same time.
            float newXPos = queueImageList[i].transform.localPosition.x - 76;
            queueImageList[i].transform.DOLocalMoveX(newXPos, 0.2f);
        }

        yield return new WaitForSeconds(0.15f);


        Destroy(queueImageList[0].gameObject);
        queueImageList.RemoveAt(0);

        queueImageList[queueImageList.Count - 1].transform.DOScale(0.65f, 0.2f);


    }


    public void SetQueue(List<MergeBall> mergeBallList)
    {
        //this sets the first queue without having to move the fellas.
        queueImageList.Clear();

        for (int i = 0; i < containerGrid.transform.childCount; i++)
        {
            Destroy(containerGrid.transform.GetChild(i).gameObject);
        }


        foreach (var item in mergeBallList)
        {
            Image newObject = Instantiate(imageTemplate, new Vector3(500,500), Quaternion.identity);
            newObject.sprite = item.sprite;
            newObject.transform.SetParent(containerGrid.transform);
            queueImageList.Add(newObject);
            newObject.gameObject.SetActive(true);
        }

        containerGrid.enabled = false;
        containerGrid.enabled = true;
        //Invoke(nameof(ResetContainerGrid), 0.1f);


    }


    void ResetContainerGrid()
    {
        containerGrid.enabled = true;
    }

    //maybe we can disable it. move it. then enabled it.



    //we also need to make them smoothly move when one is removed.


    //i want the new items to appear from the right.
    //so basically i will first create the thing with no effect.
    //


}


//we can set the queue just to not have any problem. we just put the fellas there.
//then we everytime a new one is created, the first one is rediced away, we move the line and then in the end we increase the last.
//this process will be started only by the spawn of the ball you can control.