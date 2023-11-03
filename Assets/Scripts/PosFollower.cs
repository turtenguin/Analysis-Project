using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosFollower : MonoBehaviour
{
    public Transform parent;

    public bool followX;
    public bool followY;
    public bool followZ;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - parent.position;
    }

    private void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 parentPos = parent.position;

        if (followX)
        {
            currPos = new Vector3(parentPos.x + offset.x, currPos.y, currPos.z);
        }
        if (followY)
        {
            currPos = new Vector3(currPos.x, parentPos.y + offset.y, currPos.z);
        }
        if (followZ)
        {
            currPos = new Vector3(currPos.x, currPos.y, parentPos.z + offset.z);
        }

        transform.position = currPos;
    }
}
