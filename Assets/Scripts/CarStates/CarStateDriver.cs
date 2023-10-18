using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarStateDriver : MonoBehaviour
{
    public static CarStateDriver Instance { get; private set; }
    public Rigidbody CarRigidbody { get; private set; }
    public BoxCollider Collider { get; private set; }

    private InputActions inputActionMap;
    private InputAction moveAxis;
    private InputAction driftAction;

    public ICarState CarState { get; set; }
    public ControlMode CurrentControlMode
    {
        get
        {
            return CarState.ControlMode;
        }
    }

    void Awake()
    {
        Instance = this;

        CarRigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();

        inputActionMap = new InputActions();
        inputActionMap.Enable();
        moveAxis = inputActionMap.FindAction("move");
        driftAction = inputActionMap.FindAction("drift");

        driftAction.started += EnterDrift;
        driftAction.canceled += ExitDrift;

        CarState = new PhysicsMoveNormalState();
    }

    private void FixedUpdate()
    {
        CarState.FixedUpdate();
    }

    private void EnterDrift(InputAction.CallbackContext callback)
    {
        CarState.EnterDrift();
    }

    private void ExitDrift(InputAction.CallbackContext callback)
    {
        CarState.ExitDrift();
    }

    public Vector2 GetInputAxis()
    {
        return moveAxis.ReadValue<Vector2>();
    }
}
