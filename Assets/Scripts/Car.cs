using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    public Vector3 moveForce = new Vector3(5, 0, 10);
    public Vector3 pullPoint = new Vector3(0, 0, 5f);
    public Vector3 drag = new Vector3(0, 0, -1f);
    public float reverseDrag = 5f;
    public float hoverForce = 2f;
    public float hoverHeight = 2f;
    public float driftImpulse = 2f;

    public float driftForce = 5;
    public float driftRotTorque = 2;
    public bool autoAccel = true;
    
    public Rigidbody Rb { get; private set; }
    public BoxCollider Collider { get; private set; }
    private InputActions inputActions;
    private InputAction move;

    private InputAction drift;
    private bool drifting = false;

    public float slowRotationBy = .8f;
    public float correctRotationBy = .8f;

    private Vector3 forceVec;
    private Vector3 targetUp = Vector3.up;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();

        inputActions = new InputActions();
        inputActions.Enable();
        move = inputActions.FindAction("move");
        drift = inputActions.FindAction("drift");

        drift.started += Drift;
        drift.canceled += ExitDrift;
    }

    public void FixedUpdate()
    {
        Vector2 inputVec = move.ReadValue<Vector2>();
        if (autoAccel) inputVec = new Vector2(inputVec.x, 1);

        if (!drifting)
        {
            forceVec = Vec2ToVec3(inputVec);
            forceVec.Scale(moveForce);
        } else
        {
            Vector3 driftVec = Vec2ToVec3(inputVec);
            driftVec = new Vector3(driftVec.x * driftForce, 0, 0);
            Rb.AddForce(driftVec, ForceMode.Acceleration);
        }
        
        Rb.AddForceAtPosition(forceVec, transform.TransformPoint(Rb.centerOfMass + pullPoint), ForceMode.Acceleration);

        AddUpwardsStabilizingTorque();
        AddDrag();
        AddHoverForce();
    }

    void AddHoverForce()
    {
        Rb.AddForce(Vector3.up * hoverForce * (hoverHeight - transform.position.y), ForceMode.Acceleration);
    }
    void AddUpwardsStabilizingTorque()
    {
        //https://stackoverflow.com/questions/58419942/stabilize-hovercraft-rigidbody-upright-using-torque/58420316#58420316

        Quaternion rotateDelta = Quaternion.FromToRotation(transform.up, targetUp);
        rotateDelta.ToAngleAxis(out float goalAngle, out Vector3 goalAxis);

        Rb.AddTorque(-Rb.angularVelocity * slowRotationBy, ForceMode.Acceleration);
        Rb.AddTorque(goalAxis.normalized * goalAngle * goalAngle * correctRotationBy, ForceMode.Acceleration);
    }

    private void AddDrag()
    {
        Vector3 dragForce = drag;
        dragForce.Scale(Rb.velocity);
        Rb.AddForce(dragForce, ForceMode.Acceleration);

        if(Rb.velocity.z < 0)
        {
            Rb.AddForce(new Vector3(0, 0, reverseDrag), ForceMode.Acceleration);
        }
    }

    private void Drift(InputAction.CallbackContext context)
    {
        drifting = true;
        forceVec = new Vector3(forceVec.x, 0, 0);

        if (forceVec.x > 0 && Rb.velocity.x < 0 || forceVec.x < 0 && Rb.velocity.x > 0)
        {
            Rb.AddForce(new Vector3(forceVec.x, 0, 0) * driftImpulse, ForceMode.Impulse);
        }

        if(forceVec.x > 0)
        {
            targetUp = new Vector3(1, 1, -.5f);
        }
        else
        {
            targetUp = new Vector3(-1, 1, -.5f);
        }
        
    }

    private void ExitDrift(InputAction.CallbackContext context)
    {
        drifting = false;

        targetUp = Vector3.up;
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
