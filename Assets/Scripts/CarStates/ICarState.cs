using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarState
{
    public ControlMode ControlMode { get; }
    public void FixedUpdate();
    public void EnterDrift();
    public void ExitDrift();
}
