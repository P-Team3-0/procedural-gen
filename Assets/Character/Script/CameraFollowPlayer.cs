using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public Transform player;

    private Vector3 offset;

    void Awake()
    {
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        offset = new Vector3(0, 2, -4);
    }

    void Update()
    {
        cam.transform.position = player.position + offset;
        cam.transform.LookAt(player.position);
    }
}
