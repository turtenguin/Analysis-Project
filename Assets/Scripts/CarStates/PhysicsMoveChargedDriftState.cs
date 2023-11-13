using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.GraphicsBuffer;

public class PhysicsMoveChargedDriftState : PhysicsMoveDriftState
{
    private static float boostImpulse = 60;

    public PhysicsMoveChargedDriftState(Direction direction) : base(direction)
    {
        ActivateDriftBoosters();
        carStateDriver.audioSource.PlayOneShot(carStateDriver.charge);
    }

    private void ActivateDriftBoosters()
    {
        carStateDriver.leftBooster.Stop();
        carStateDriver.rightBooster.Stop();
        carStateDriver.leftChargedBooster.Play();
        carStateDriver.rightChargedBooster.Play();
    }

    private void ActivateNormalBoosters()
    {
        carStateDriver.leftChargedBooster.Stop();  
        carStateDriver.rightChargedBooster.Stop();
        carStateDriver.leftBooster.Play();
        carStateDriver.rightBooster.Play();
    }

    public override void ExitDrift()
    {
        Vector3 boostForce = new Vector3((int)direction * boostImpulse, 0, 0);
        carRb.AddForce(boostForce, ForceMode.Impulse);
        ActivateNormalBoosters();
        carStateDriver.audioSource.PlayOneShot(carStateDriver.boost);
        base.ExitDrift();
    }

    protected override void InitDrift() { }
}
