using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Vector3 InputKey;
    float Myfloat;
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        ////////////Move With WASD
        InputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {

        bool pressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);


        if (pressed)
        {
            rb.MovePosition((Vector3)transform.position + InputKey * 4 * Time.deltaTime);
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg; //=========================================== LookAt
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f); //=================== Smooth Rotation
            transform.rotation = Quaternion.Euler(0, Smooth, 0); //============================================================ Change Angle
            animator.SetBool(isWalkingHash, true);          
        }
        if (!pressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (pressed && runPressed)
        {
            rb.MovePosition((Vector3)transform.position + InputKey * 8 * Time.deltaTime);
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg; //=========================================== LookAt
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f); //=================== Smooth Rotation
            transform.rotation = Quaternion.Euler(0, Smooth, 0); //============================================================ Change Angle
            animator.SetBool(isRunningHash, true);
        }
        if (!pressed || !runPressed)
        {
            animator.SetBool(isRunningHash, false);
        }



    }



}