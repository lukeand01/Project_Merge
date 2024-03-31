using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueUI : MonoBehaviour
{
    GameObject holder;
    List<Vector3> localPositions = new();

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        localPositions = new()
        {
            new Vector3(50, -50, 0),
            new Vector3(126, -50, 0),
            new Vector3(202, -50, 0),
            new Vector3(278, -50, 0)
        };

    }

    private void Update()
    {
        //we can do that each part should be consantly moving to the end.



    }

    //i might be cool to show more fellas. so lets no throw the idea out just yet.
    List<Image> queueImageList = new();

    public void AddToQueue(MergeBall merge)
    {
        //we wil extract the sprite from this
        //always add to the end of the list


        //it cannot be an ui. i need to do it with gameobjects.
    }


    public void RemoveFromQueue()
    {
        



    }


    //we also need to make them smoothly move when one is removed.



}
