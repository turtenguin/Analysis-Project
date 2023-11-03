using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void TimerInvoke(Action action, float seconds)
    {
        StartCoroutine(Callback(action,seconds));
    }
    private IEnumerator Callback(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
