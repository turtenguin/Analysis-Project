using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public Rigidbody carRb;
    public float ZPullStrength;
    public float YPullStrength;
    public float nonlinearity;

    public float movementDamp;

    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }

    //Camera is in car Z center track frame, so it is pulling from local 0, 0, 0
    private void Update()
    {
        float zStretch = ZPullStrength * Mathf.Log(carRb.velocity.magnitude * nonlinearity + 1, 2);
        float yStretch = YPullStrength * Mathf.Log(carRb.velocity.magnitude * nonlinearity + 1, 2);
        Vector3 targetPos = new Vector3(basePos.x, basePos.y * (1 + yStretch), basePos.z * (1 + zStretch));
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, movementDamp * Time.deltaTime);
    }
}
