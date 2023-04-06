using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehavior
{
    public Kinematic character;
    public GameObject target;

    float maxAcceleration = 100f;
    float maxSpeed = 10f;

    float targetRadius = 1.5f;

    float slowRadius = 3f;

    float timeToTarget = 0.1f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 direction = target.transform.position - character.transform.position;

        float distance = direction.magnitude;
        float targetSpeed = 0f;

        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }

        else
        {
            targetSpeed = maxSpeed * (distance - targetRadius) / targetRadius;
        }

        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        result.linear = targetVelocity - character.linearVelocity;
        result.linear /= timeToTarget;

        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }

        return result;
    }
}
