using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RigidbodyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Vector3 InputKey;
    public AudioSource witchWalk;
    public AudioSource witchRun;
    public AudioSource witchWin;
    public AudioSource witchDeath;
    public AudioSource witchUpgrade;
    float Myfloat;
    int controlWalk;
    int controlRun;
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int AttackHash;
    GameObject winEffect;
    private CinemachineFreeLook activeCamera;
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        AttackHash = Animator.StringToHash("Attack");
        controlWalk = 0;
        controlRun = 0;
        winEffect = GameObject.FindWithTag("Win");

        if (winEffect != null)
            winEffect.SetActive(false);
    }

    void Update()
    {
        Win();
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
            if (controlWalk == 0)
            {
                witchWalk.Play();
                controlWalk = 1;
            }
            animator.SetBool(isWalkingHash, true);
        }
        if (!pressed)
        {
            if (controlWalk == 1)
            {
                witchWalk.Stop();
                controlWalk = 0;
            }
            animator.SetBool(isWalkingHash, false);
        }
        if (pressed && runPressed)
        {
            rb.MovePosition((Vector3)transform.position + InputKey * 8 * Time.deltaTime);
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg; //=========================================== LookAt
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f); //=================== Smooth Rotation
            transform.rotation = Quaternion.Euler(0, Smooth, 0); //============================================================ Change Angle
            if (controlRun == 0)
            {
                witchRun.Play();
                controlRun = 1;
            }
            animator.SetBool(isRunningHash, true);
        }
        if (!pressed || !runPressed)
        {
            if (controlRun == 1)
            {
                witchRun.Stop();
                controlRun = 0;
            }
            animator.SetBool(isRunningHash, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy" || collision.gameObject.tag == "Bullet")
        {
            StopForce();
        }
        {
            StopForce();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        GetComponent<LifeManager>().TakeDamage(other.GetComponent<bullet>().damage);
        Destroy(other.gameObject);
    }

    private void StopForce()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void Win()
    {
        GameObject boss = GameObject.FindWithTag("Boss");
        LifeManager lifeManager = boss.GetComponent<LifeManager>();
        if (lifeManager != null && lifeManager.health <= 0)
        {
            winEffect.SetActive(true);
            animator.SetTrigger("win");
            witchWin.Play();
            GetComponent<RigidbodyMovement>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            GameObject firePoint = gameObject.transform.Find("Sphere").gameObject;
            firePoint.GetComponent<WitchAttack>().enabled = false;
        }
    }

    private void playSoundDeath()
    {
        witchDeath.Play();
    }
    private void playSoundUpgrade()
    {
        witchUpgrade.Play();
    }
}



