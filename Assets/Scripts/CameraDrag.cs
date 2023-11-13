using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public Rigidbody carRb;
    public float ZPullStrength;
    public float YPullStrength;

    public float rotPullStrength;
    public float XRotMax;
    public float nonlinearity;
    public float Ynonlinearity;

    public float movementDamp;

    private Vector3 basePos;
    private Quaternion baseRot;
    private Quaternion maxRot;

    private void Start()
    {
        basePos = transform.localPosition;
        baseRot = transform.rotation;
        maxRot = new Quaternion();
        maxRot.eulerAngles = baseRot.eulerAngles + new Vector3(XRotMax, 0, 0);
    }

    //Camera is in car Z center track frame, so it is pulling from local 0, 0, 0
    private void Update()
    {
        float zStretch = ZPullStrength * Mathf.Log(carRb.velocity.magnitude * nonlinearity + 1, 2);
        float yStretch = YPullStrength * Mathf.Log(carRb.velocity.magnitude * Ynonlinearity + 1, 2);
        Vector3 targetPos = new Vector3(basePos.x, basePos.y * (1 + yStretch), basePos.z * (1 + zStretch));
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, movementDamp * Time.deltaTime);

        float rotLerp = 1 - (1 / (nonlinearity * carRb.velocity.magnitude * rotPullStrength + 1));
        Quaternion targetRot = Quaternion.Lerp(baseRot, maxRot, rotLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, movementDamp * Time.deltaTime);
    }
}
