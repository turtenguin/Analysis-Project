using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMoveNormalState : ICarState
{
    private CarStateDriver carStateDriver;
    private Rigidbody carRb;

    private static Vector3 moveForce;

    static PhysicsMoveNormalState()
    {
        moveForce = new Vector3(5, 0, 5);
    }

    public PhysicsMoveNormalState()
    {
        carStateDriver = CarStateDriver.Instance;
        carRb = carStateDriver.CarRigidbody;
    }

    public ControlMode ControlMode
    {
        get { return ControlMode.physics; }
    }

    public void EnterDrift()
    {
        Direction direction;
        if(carStateDriver.GetInputAxis().x < 0)
        {
            direction = Direction.left;
        }
        else
        {
            direction = Direction.right;
        }
        carStateDriver.CarState = new PhysicsMoveDriftState(direction);
    }

    public void ExitDrift()
    {
        //Do nothing
    }

    public void FixedUpdate()
    {
        Vector2 inputVec = carStateDriver.GetInputAxis();
        Vector2 moveVec = new Vector2(inputVec.x, 1);
        Vector3 forceVec = PhysicsModeUtils.Vec2ToVec3(moveVec);
        forceVec.Scale(moveForce);

        Vector3 pullPosition = carRb.transform.TransformPoint(carRb.centerOfMass + PhysicsModeUtils.PullPoint);

        carRb.AddForceAtPosition(forceVec, pullPosition, ForceMode.Acceleration);

        PhysicsModeUtils.AddPassiveForces(carRb, Vector3.up);
    }
}
