using UnityEngine;

public class DestroyFlame : MonoBehaviour
{
    public float destroyDelay;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            (other.name);
        }
    }
}
