using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFollower : MonoBehaviour
{
    public Transform car;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, car.position.z);
    }
}
