using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text distanceText;
    public Text speedText;
    public Car car;

    private Rigidbody carRb;

    // Start is called before the first frame update
    void Start()
    {
        carRb = car.GetComponent<Rigidbody>();
    }

    void Update()
    {
        distanceText.text = "Distance: " + (int)car.transform.position.z;
        speedText.text = "Speed: " + (int)carRb.velocity.z;
    }
}
