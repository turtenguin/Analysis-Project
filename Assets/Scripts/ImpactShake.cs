using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactShake : MonoBehaviour
{
    public float shakeMultiplier;
    public Transform transformToShake;
    public float damp;

    private float intensity = 0;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transformToShake.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        intensity = collision.impulse.magnitude * shakeMultiplier;
    }

    private void Update()
    {
        Vector3 offset = Random.insideUnitSphere * intensity;
        transformToShake.localPosition = originalPos + offset;
        intensity -= intensity * damp * Time.deltaTime;
    }
}
