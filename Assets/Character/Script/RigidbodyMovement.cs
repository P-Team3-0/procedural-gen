using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RigidbodyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Vector3 InputKey;
    float Myfloat;
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int AttackHash;
    private CinemachineFreeLook activeCamera;
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        AttackHash = Animator.StringToHash("Attack");
    }

    void Update()
    {
        activeCamera = GameObject.FindWithTag("FreeLookCamera").GetComponent<CinemachineFreeLook>();

        // Get the direction that the camera is looking
        Vector3 cameraForward = activeCamera.transform.forward;
        cameraForward.y = 0; // keep only the horizontal direction
        cameraForward.Normalize(); // Normalize it so that it's a unit vector

        Vector3 cameraRight = activeCamera.transform.right;
        cameraRight.y = 0; // keep only the horizontal direction
        cameraRight.Normalize(); // Normalize it so that it's a unit vector

        // Get the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Transform the input vector into world space, oriented relative to the camera's look direction
        InputKey = (cameraRight * input.x + cameraForward * input.z);
    }

    void FixedUpdate()
    {

        bool pressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            animator.SetBool(AttackHash, true);
        }
        else
        {
            animator.SetBool(AttackHash, false);
        }

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