using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMoveDriftState : ICarState
{
    private CarStateDriver carStateDriver;
    
    public ControlMode ControlMode
    {
        get { return ControlMode.physics; }
    }

    private Direction direction;
    private Rigidbody carRb;

    private static float driftForce = 10;
    private static float driftAdjustForce = 10;
    private static float driftImpulse = 3;

    private Vector2 targetUp;

    public PhysicsMoveDriftState(Direction direction)
    {
        carStateDriver = CarStateDriver.Instance;
        this.direction = direction;
        this.carRb = carStateDriver.CarRigidbody;

        targetUp = new Vector3((int)direction, 1, -1);

        Vector3 driftImpulseForce = new Vector3((int)direction * driftImpulse, 0, 0);
        carRb.AddForce(driftImpulseForce, ForceMode.Impulse);
    }

    public void EnterDrift()
    {
        //Do nothing
    }

    public void ExitDrift()
    {
        carStateDriver.CarState = new PhysicsMoveNormalState();
    }

    public void FixedUpdate()
    {
        Vector3 driftForceVec = new Vector3((int)direction * driftForce, 0, 0);
        Vector3 pullPosition = carRb.transform.TransformPoint(carRb.centerOfMass + PhysicsModeUtils.PullPoint);
        carRb.AddForceAtPosition(driftForceVec, pullPosition, ForceMode.Acceleration);

        Vector3 driftForceAdjustVec = carStateDriver.GetInputAxis();
        driftForceAdjustVec = new Vector3(driftForceAdjustVec.x * driftAdjustForce, 0, 0);
        carRb.AddForce(driftForceAdjustVec, ForceMode.Acceleration);

        PhysicsModeUtils.AddPassiveForces(carRb, targetUp);
    }
}
