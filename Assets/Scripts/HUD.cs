using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text distanceText;
    public Text speedText;

    public Rigidbody carRb;

    void Update()
    {
        distanceText.text = "Distance: " + (int)carRb.transform.position.z;
        speedText.text = "Speed: " + (int)carRb.velocity.z;
    }
}
