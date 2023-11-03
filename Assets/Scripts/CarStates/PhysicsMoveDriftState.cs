using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMoveDriftState : ICarState
{
    protected CarStateDriver carStateDriver;
    private Timer timer;
    
    public ControlMode ControlMode
    {
        get { return ControlMode.physics; }
    }

    protected Direction direction;
    protected Rigidbody carRb;

    private const float driftForce = 10;
    private const float driftAdjustForce = 15;
    private const float driftImpulseFactor = .75f;
    private const float chargeTime = 1;

    protected Vector2 targetUp;

    public PhysicsMoveDriftState(Direction direction)
    {
        timer = Timer.Instance;
        carStateDriver = CarStateDriver.Instance;
        this.direction = direction;
        this.carRb = carStateDriver.CarRigidbody;

        targetUp = new Vector3((int)direction, 1, -1);

        InitDrift();
    }

    protected virtual void InitDrift()
    {
        if((direction == Direction.left && carRb.velocity.x > 0) || (direction == Direction.right && carRb.velocity.x < 0))
        {
            Vector3 driftImpulseVelocityChange = new Vector3(-carRb.velocity.x * driftImpulseFactor, 0, 0);
            carRb.AddForce(driftImpulseVelocityChange, ForceMode.VelocityChange);
        }
        

        timer.TimerInvoke(OnCharged, chargeTime);
    }

    private void OnCharged()
    {
        if(carStateDriver.CarState == this)
        {
            carStateDriver.CarState = new PhysicsMoveChargedDriftState(direction);
        }
    }

    public void EnterDrift()
    {
        //Do nothing
    }

    public virtual void ExitDrift()
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
