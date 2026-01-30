using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    //CinmeachineFreeLookObj
    public Transform cam;
    public CinemachineFreeLook cinemachine;
    public float o_YAxis;
    public float o_XAxis;

    float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    //Player Movement
    Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    float trueSpeed;
    public bool Walking = false;

    //Jump
    public float jumpHeight;
    public float gravity;
    public float fallgravity;
    public float jumpGravity;
    bool isGround;
    [SerializeField]Vector3 velocity;

    public Animator anim;

    //Test
    public Transform Frontobj;

    [Header("Draw Sword")]
    public KeyCode swordDrawKeyCode;
    public bool SwordDrawed = false;
    [Header("Draw Sword Animation Config")]
    public GameObject SwordTarget;
    public GameObject BackPlacesTarget;
    public GameObject HandTarget;
    // Start is called before the first frame update
    void Start()
    {
        trueSpeed = sprintSpeed;

        //Stop cam moving 
        o_XAxis = cinemachine.m_XAxis.m_MaxSpeed;
        o_YAxis = cinemachine.m_YAxis.m_MaxSpeed;
        cinemachine.m_XAxis.m_MaxSpeed = 0f;
        cinemachine.m_YAxis.m_MaxSpeed = 0f;

        anim = transform.Find("Model").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Character Idle on the ground    
        //isGround = Physics.CheckSphere(transform.position, 0.01f);
        
        if(!isGround && velocity.y < 0.1)
        {
            velocity.y = -1;
            //anim.SetBool("isGround", true);
        }
        

        HoldingCursor();

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        //Switch Sprint/walk
        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            if (Walking)
            {
                trueSpeed = sprintSpeed;
                Walking = false;
            }
            else
            {
                trueSpeed = walkSpeed;
                Walking = true;
            }
        }

        if (Input.GetKeyDown(swordDrawKeyCode))
        {
            if (isGround)
            {
                if (!SwordDrawed)
                {
                    SwordDrawed = true;
                    anim.SetBool("DrawedSword", true);

                }
                else
                {
                    SwordDrawed = false;
                    anim.SetBool("DrawedSword", false);
                    anim.SetTrigger("PlaceBackSword");
                    Debug.Log("PlaceSword");
                }
            }
        }

        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
            controller.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);

            if (!Walking)
            {
                anim.SetFloat("Speed", 2);
            }
            else
            {
                anim.SetFloat("Speed", 1);
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

            //jump 
            if (Input.GetButtonDown("Jump") && isGround)
            {
            //anim.SetBool("Jumping", true);
            anim.SetBool("isGround", false);
            isGround = false;
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * jumpGravity);
            //velocity.y = Mathf.Sqrt(jumpHeight * -2 * jumpGravity);
            }
   
        //Fall Gravity
        if(velocity.y > -5)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "floor")
        {
            isGround = true;
            anim.SetBool("isGround", true);
            //Debug.Log("isGround! " + col.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    
    void HoldingCursor()
    {
        //lock the cursor when holding right click
        if (Input.GetMouseButton(1))
        {
            cinemachine.m_XAxis.m_MaxSpeed = o_XAxis;
            cinemachine.m_YAxis.m_MaxSpeed = o_YAxis;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            /*
            if (Input.GetMouseButton(1) )
            {
                var lookPos = Frontobj.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            }
            */
        }
        if (Input.GetMouseButtonUp(1))
        {
            cinemachine.m_XAxis.m_MaxSpeed = 0f;
            cinemachine.m_YAxis.m_MaxSpeed = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Mouse Scroll
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(cinemachine.m_Lens.FieldOfView < 80)
            {
                cinemachine.m_Lens.FieldOfView++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (cinemachine.m_Lens.FieldOfView > 15)
            {
                cinemachine.m_Lens.FieldOfView--;
            }
        }
    }

    //For AnimationEventHolder
    public void Anim_SwordToBackPlacesTarget()
    {
        SwordTarget.transform.SetParent(BackPlacesTarget.transform);
    }

    public void Anim_SwordToBackHandTarget()
    {
        SwordTarget.transform.SetParent(HandTarget.transform);
    }
}
