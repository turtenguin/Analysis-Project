using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class PhysicsModeUtils
{
    public static Vector3 PullPoint { get; private set; }

    private static Vector3 drag;
    private static float reverseOnlyDrag = 10f;

    private static float hoverForce = 2f;
    private static float hoverHeight = 2f;

    private static float slowRotationBy = .8f;
    private static float correctRotationBy = .5f;
    
    static PhysicsModeUtils()
    {
        PullPoint = new Vector3(0, 0, .5f);
        drag = new Vector3(0, -.1f, -.005f);
    }

    public static void AddHoverForce(Rigidbody rb)
    {
        rb.AddForce(Vector3.up * hoverForce * (hoverHeight - rb.transform.position.y), ForceMode.Acceleration);
    }

    public static void AddUpwardsStabilizingTorque(Rigidbody rb, Vector3 moveUpVectorTo)
    {
        //https://stackoverflow.com/questions/58419942/stabilize-hovercraft-rigidbody-upright-using-torque/58420316#58420316

        Quaternion rotateDelta = Quaternion.FromToRotation(rb.transform.up, moveUpVectorTo);
        rotateDelta.ToAngleAxis(out float goalAngle, out Vector3 goalAxis);

        rb.AddTorque(-rb.angularVelocity * slowRotationBy, ForceMode.Acceleration);
        rb.AddTorque(goalAxis.normalized * goalAngle * goalAngle * correctRotationBy, ForceMode.Acceleration);
    }

    public static void AddDrag(Rigidbody rb)
    {
        Vector3 dragForce = drag;
        dragForce.Scale(rb.velocity);
        rb.AddForce(dragForce, ForceMode.Acceleration);

        if (rb.velocity.z < 0)
        {
            rb.AddForce(new Vector3(0, 0, reverseOnlyDrag), ForceMode.Acceleration);
        }
    }

    public static void AddPassiveForces(Rigidbody rb, Vector3 moveUpVectorTo)
    {
        AddUpwardsStabilizingTorque(rb, moveUpVectorTo);
        AddDrag(rb);
        AddHoverForce(rb);
    }

    public static Vector3 Vec2ToVec3(Vector2 vec2)
    {
        return new Vector3(vec2.x, 0, vec2.y);
    }

    public static Vector3 RemoveY(Vector3 vec3)
    {
        return new Vector3(vec3.x, 0, vec3.z);
    }
}
