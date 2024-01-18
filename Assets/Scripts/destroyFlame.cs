using UnityEngine;

public class DestroyFlame : MonoBehaviour
{
    public float destroyDelay;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
