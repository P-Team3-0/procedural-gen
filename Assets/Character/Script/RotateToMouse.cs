using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.PlayerSettings;

public class RotateToMouse : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public float maximumLength;

    private Ray rayMouse;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            RaycastHit hit;
            var mousePos = Input.mousePosition;

            // Converti le coordinate del mouse direttamente in coordinate del mondo
            pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            // Crea un ray dal personaggio al punto del mouse
            rayMouse = new Ray(transform.position, (pos - transform.position).normalized);

            if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumLength))
            {
                // Modifica la direzione in modo che punti dal personaggio al punto colpito
                direction = hit.point - transform.position;
                rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                direction = rayMouse.direction;
                rotation = Quaternion.LookRotation(direction);
            }

            if (Input.GetMouseButtonDown(0))
            {
                // Applica la rotazione all'oggetto
                transform.rotation = rotation;
            }
        }
        else
        {
            Debug.Log("No camera");
        }
    }

    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }
}