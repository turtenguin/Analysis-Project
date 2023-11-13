using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public float collisionForceForMaxSound;

    private void OnCollisionEnter(Collision collision)
    {
        float volume = collision.impulse.magnitude / collisionForceForMaxSound;
        if (volume > 1) volume = 1;
        audioSource.volume = 1;
        audioSource.PlayOneShot(clip, volume);
    }
}
