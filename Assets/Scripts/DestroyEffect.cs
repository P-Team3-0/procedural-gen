using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{

    public float destroyDelay;

    private void Start()
    {
        RemoveEffect();
    }

    private void RemoveEffect()
    {
        Destroy(gameObject, destroyDelay);
    }
}
