using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBall2D : MonoBehaviour
{
    Rigidbody2D rb;
    public MergeBallType mergeType;
    RigidbodyConstraints rbConstraints;
    bool cannotCollide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }



    public void ForceNoCollision()
    {
        cannotCollide = true;
    }

    public void MakeNotReactive()
    {
        //rbConstraints = rb.constraints;
        //rb.useGravity = false;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //rb.constraints -= RigidbodyConstraints.FreezePositionX;


    }

    public void MakeReactive()
    {
        //rb.velocity = Vector3.zero;
        //rb.useGravity = true;
        //rb.constraints = rbConstraints;

    }


    public bool CanMerge(int index)
    {
        return (int)mergeType == index;
    }
}
