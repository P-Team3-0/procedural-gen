using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{

    public float destroyDelay;

    public AudioSource destroySound;

    private void Start()
    {
        if (destroySound != null)
        {
            destroySound.Play();
        }

        RemoveEffect();
    }

    private void RemoveEffect()
    {
        Destroy(gameObject, destroyDelay);
    }
}
