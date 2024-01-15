using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public float maximumLenght;

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
            rayMouse = cam.ScreenPointToRay(mousePos);

            // Calculate the position of the mouse in camera space
            pos = cam.ScreenToWorldPoint(mousePos);

            if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumLenght))
            {
                // Calculate the rotation using Quaternion.LookAt()
                rotation = Quaternion.LookRotation(hit.point, transform.position);
            }
            else
            {
                // Use the direction of the mouse ray
                direction = rayMouse.direction;
                rotation = Quaternion.LookRotation(direction);
            }

            // Apply the rotation to the object
            if (Input.GetMouseButtonDown(0))
            {
                transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, 1);

            }
        }
        else
        {
            Debug.Log("no camera");
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
