using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Physics : MonoBehaviour
{
    #region Player Variables
    [Header("Player Variables")]
    // Player Vars
    public Rigidbody rb;
    private Vector3 lastRotation = Vector3.back;
    public float movement_speed = 8.0f;
    public float jump_force = 900.0f;
    public float fallingDirection = 0.0f;
    public float fallingDirectionReducer = 0.03f;
    #endregion

    #region Animation Variables
    // Animation vars
    private Animator animator;
    AnimatorStateInfo stateInfo;
    public const string IDLE = "Sword_F_BattleIdle_01";
    public const string RUN = "Sword_F_Run_01";
    public const string ATT_0 = "Sword_F_Attack_01";
    public const string ATT_1 = "Sword_F_Attack_02";
    public const string COMBO_1 = "Sword_F_Skill_02";
    public const string JUMP = "Sword_F_Jump_01";
    public const string SPECIAL_1 = "Sword_F_Skill_03";
    #endregion

    #region Animation Hashes
    // Animation hashses - more efficient than passing in a string as the animator will have to evaluate the string everytime it makes the call
    private int movingHash = Animator.StringToHash("Player Moving");
    private int jumpingHash = Animator.StringToHash("Player Jumping");
    private int onGroundHash = Animator.StringToHash("Player On Ground");
    #endregion

    #region Jumping Variables
    [Header("Jump Variables")]
    // For variable jump
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    public float distToGround = 1.0f;
    public float fallHeight = 0.0f;
    public ParticleSystem dust;
    #endregion

    #region Dash Variables
    [Header("Dash Variables")]
    // Dash Variables
    public float dashSpeed = 1.0f;
    public float dashInputBufferTime = 0.5f;
    private float dashInputTimer;
    private int inputCountLeft = 0;
    private int inputCountRight = 0;
    #endregion

    #region Grounded Fine Tuning
    [Header("Grounded Fine Tuning Variables")]
    // Vars for falling through platform
    public Vector3 raycastOrigin;
    public float rayDownDist = 1.0f;
    // Vars for IsGrounded()
    public LayerMask groundLayers;
    public CapsuleCollider playerCapsuleCollider;
    #endregion

    #region Unity's Rendering Loop (Start/Update/Etc)
    // To initalize when player instantiates
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        dashInputTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        //float vertical_input = Input.GetAxis("Vertical");     // Not used in side scroller
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(fallingDirection);
        if (!stateInfo.IsName(RUN) && !stateInfo.IsName(IDLE) && !stateInfo.IsName(JUMP))
        {
            if(IsGrounded())
            {
                fallingDirection = 0;   // Change this out to lose momentum rather than losing input direction (Both work, but input direction shouldnt be altered without player input)
            }
            if (fallingDirection > 0)
            {
                // Instad of direction should I be changing the speed? Either that or use lerp? Or Coroutine?
                this.gameObject.transform.Translate(new Vector3(Mathf.Clamp(fallingDirection -= fallingDirectionReducer, 0.0f, 1.0f), 0, 0) * movement_speed * dashSpeed * Time.deltaTime, Space.World); // Standard horizontal movement
            }
            else if (fallingDirection < 0)
            {
                this.gameObject.transform.Translate(new Vector3(Mathf.Clamp(fallingDirection += fallingDirectionReducer, -1.0f, 0.0f), 0, 0) * movement_speed * dashSpeed * Time.deltaTime, Space.World); // Standard horizontal movement
            }
            //this.gameObject.transform.Translate(new Vector3(0, 0, 0) * movement_speed * dashSpeed * Time.deltaTime, Space.World); // Standard horizontal movement
        }
        else
        {
            this.gameObject.transform.Translate(new Vector3(horizontal_input, 0, 0) * movement_speed * dashSpeed * Time.deltaTime, Space.World); // Standard horizontal movement
        }
        AnimationTriggers();
        RotatePlayer(horizontal_input);
        Dash(horizontal_input);
    }

    private void FixedUpdate()
    {
        JumpPhysics();
    }

    // Per frame physics that updates last
    private void LateUpdate()
    {
        // Fall through platform logic
        RaycastHit hit;
        int layerMask = 1 << 8;
        raycastOrigin = gameObject.transform.GetChild(2).transform.position;
        layerMask = ~layerMask;

        if (Physics.Raycast(raycastOrigin, transform.TransformDirection(-Vector3.up), out hit, rayDownDist, layerMask))
        {
            //Debug.DrawRay(raycastOrigin, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            if (Input.GetButtonDown("Down") && hit.transform.gameObject.tag == "Platform")
            {
                Physics.IgnoreCollision(hit.transform.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                //Debug.Log(hit.transform.gameObject.name);
            }
        }
        //else
        //{
        //    //Debug.DrawRay(raycastOrigin, transform.TransformDirection(-Vector3.up) * rayDownDist, Color.white);
        //    //Debug.Log("No Hit");
        //}
    }
    #endregion

    #region Helper Methods
    private void AnimationTriggers()
    {
        // Movement Animation
        if (Input.GetButton("Horizontal") && IsGrounded() && !stateInfo.IsName(ATT_0) && !stateInfo.IsName(ATT_1) && !stateInfo.IsName(COMBO_1)) // If moving and not doing anything but running
        {
            animator.SetBool(movingHash, true);
            //animator.SetBool("Player Jumping", false);
        }
        else if (!Input.GetButtonDown("Horizontal") && IsGrounded() && !stateInfo.IsName(ATT_0) && !stateInfo.IsName(ATT_1) && !stateInfo.IsName(COMBO_1))   // If moving and not doing anything but idling
        {
            animator.SetBool(movingHash, false);
            //animator.SetBool("Player Jumping", false);
        }

        if (!IsGrounded() && rb.velocity.y > Mathf.Epsilon)   // Place up and down animations here with respect to velocity // To stop premature animation changes when moving up through two platforms and landing on ground
        {
            animator.SetBool(onGroundHash, false); 
        }
        if (IsGrounded() && rb.velocity.y < Mathf.Epsilon)  // To stop premature animation changes when moving up through two platforms and landing on ground
        {
            //Object.Instantiate(dust, transform.position, Quaternion.identity).Play();
            //Debug.Log("WE HAVE LANDED");
            animator.SetBool(jumpingHash, false);
            animator.SetBool(onGroundHash, true);
            //landingParticles.Play();
        }
    }

    private void RotatePlayer(float horizontal_input)
    {
        // Rotation of the Player - Might need to be fixed
        if (horizontal_input > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.right);
            lastRotation = Vector3.right;
        }
        else if (horizontal_input < 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
            lastRotation = Vector3.left;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(lastRotation);
        }
    }

    private void Dash(float horizontal_input)
    {
        //Debug.Log("LEFT COUNT: " + inputCountLeft);
        //Debug.Log("RIGHT COUNT: " + inputCountRight);
        //Debug.Log(Time.time - dashInputTimer);
        if(Time.time - dashInputTimer > dashInputBufferTime)
        {
            //Debug.Log("RESET TIMER");
            inputCountLeft = 0;
            inputCountRight = 0;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // I think this is when the button has been pressed?
        {
            //Debug.Log("LEFT");
            if (inputCountLeft == 0)    // Starting the dash timer if first key registered
            {
                dashInputTimer = Time.time;
            }
            inputCountLeft++;
            inputCountRight = 0;

            if (inputCountLeft >= 2 && Time.time - dashInputTimer < dashInputBufferTime)
            {
                dashSpeed = 2.0f;
            }
            else
            {
                dashSpeed = 1.0f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))    // if right pressed
        {
            //Debug.Log("RIGHT");
            if (inputCountRight == 0)   // Starting the dash timer if first key registered
            {
                dashInputTimer = Time.time;
            }
            inputCountRight++;
            inputCountLeft = 0;

            if (inputCountRight >= 2 && dashInputTimer - Time.time < dashInputBufferTime)
            {
                dashSpeed = 2.0f;
            }
            else
            {
                dashSpeed = 1.0f;
            }
        }
    }

    private void JumpPhysics()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))    // Standard Jump
        {
            fallingDirection = Input.GetAxisRaw("Horizontal");
            rb.velocity = Vector3.zero;
            animator.SetBool(jumpingHash, true);
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jump_force * Time.fixedDeltaTime, ForceMode.VelocityChange);
            ParticleSystem particlefx = Instantiate(dust, transform.position, Quaternion.identity);
            //particlefx.GetComponent<DissolveControl>().dissolveMaterial.SetFloat("_AlphaClipThreshold", 0.0f);
            particlefx.Play();
            Destroy(particlefx.gameObject, particlefx.main.duration);
        }
        if (rb.velocity.y < fallHeight)  // Faster fall
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        //else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))   // Small Hop
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //}
    }

    public bool IsGrounded()
    {
        Debug.DrawRay(raycastOrigin, transform.TransformDirection(-Vector3.up) * (distToGround + 0.1f), Color.white);
        return Physics.Raycast(raycastOrigin, -Vector3.up, distToGround + 0.1f);
    }
    #endregion
}
