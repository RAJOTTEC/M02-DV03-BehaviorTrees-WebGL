using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity;

    public float maxSpeed = 10.0f;
    public float maxAngularVelocity = 45.0f;

    public GameObject myTarget;

    protected SteeringOutput steeringUpdate;

    void Start()
    {
        steeringUpdate = new SteeringOutput();
    }

    protected virtual void Update()
    {
        if (float.IsNaN(angularVelocity))
        {
            angularVelocity = 0.0f;
        }

        this.transform.position += linearVelocity * Time.deltaTime;

        if (Mathf.Abs(angularVelocity) > 0.01f)
        {
            Vector3 v = new Vector3(0, angularVelocity, 0);
            this.transform.eulerAngles += v * Time.deltaTime;
        }

        if (steeringUpdate != null)
        {
            linearVelocity += steeringUpdate.linear * Time.deltaTime;
            angularVelocity += steeringUpdate.angular * Time.deltaTime;
        }

        if (linearVelocity.magnitude > maxSpeed)
        {
            linearVelocity.Normalize();
            linearVelocity *= maxSpeed;
        }

        if (Mathf.Abs(angularVelocity) > maxAngularVelocity)
        {
            angularVelocity = maxAngularVelocity * (angularVelocity / angularVelocity);
        }
    }

}
