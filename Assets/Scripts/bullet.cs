using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage;
    public GameObject player;

    public AudioSource bulletSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        bulletSound.Play();
        Invoke(nameof(stopSound), 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<LifeManager>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void stopSound()
    {
        bulletSound.Stop();
    }
}
