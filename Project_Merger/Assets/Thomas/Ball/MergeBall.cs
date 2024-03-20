using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBall : MonoBehaviour
{
    Rigidbody rb;
    public MergeBallType mergeType;

    bool cannotCollide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //i hvae to desactive the other one.
    private void OnCollisionEnter(Collision collision)
    {
        //then we need to ask if the other fella that we collide is the same.

        MergeBall merge = collision.collider.GetComponent<MergeBall>();

        if (merge == null) return;

        if (!merge.CanMerge((int)mergeType)) return;

        if (cannotCollide) return;

        merge.ForceNoCollision();
        ForceNoCollision();

        GameHandler.instance.ballHandler.SpawnNewAtPosition((int)mergeType, gameObject, collision.collider.gameObject);

        Destroy(gameObject);
    }

    RigidbodyConstraints rbConstraints;

    public void ForceNoCollision()
    {
        cannotCollide = true;
    }

    public void MakeNotReactive()
    {
        rbConstraints = rb.constraints;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints -= RigidbodyConstraints.FreezePositionX;
    }

    public void MakeReactive()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        rb.constraints = rbConstraints;

    }


    public bool CanMerge(int index)
    {
        return (int)mergeType == index;
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