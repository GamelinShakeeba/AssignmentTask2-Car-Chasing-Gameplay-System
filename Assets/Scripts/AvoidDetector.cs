using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 1;

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag != "car" || col.gameObject.tag != "Player") return;
        avoidTime = 0;
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag != "car" || col.gameObject.tag != "Player") return;

        Rigidbody otherCar = col.rigidbody;
        avoidTime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
        avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
    }
}
