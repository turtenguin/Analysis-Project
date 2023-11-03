using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PhysicsMoveChargedDriftState : PhysicsMoveDriftState
{
    private static float boostImpulse = 20;

    public PhysicsMoveChargedDriftState(Direction direction) : base(direction) { }

    public override void ExitDrift()
    {
        Vector3 boostForce = new Vector3((int)direction * boostImpulse, 0, 0);
        carRb.AddForce(boostForce, ForceMode.Impulse);
        base.ExitDrift();
    }

    protected override void InitDrift() { }
}
