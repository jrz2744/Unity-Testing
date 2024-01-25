using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player Movement Speed Variables
    [Header("Movement")]
    // Player MoveSpeed
    [SerializeField] float moveSpeed;
    // Player Drag Variable
    [SerializeField] float groundDrag;
    // Jump Force
    [SerializeField] float jumpForce;
    // Air Speed Variable
    [SerializeField] float airMultiplier;
    // Jump Cooldown
    [SerializeField] float jumpCooldown;
    // Ready To Jump
    bool readyToJump;

    // Keybinds
    [Header("Keybinds")]
    // Jump Key
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    // Player Ground Check Variables
    [Header("Ground Check")]
    // Player Height
    [SerializeField] float playerHeight;
    // Ground Mask
    [SerializeField] LayerMask whatIsGround;
    // Grounded Varaible
    bool grounded;

    // Other Variables
    [Header("Other")]

    // Player Orientation
    [SerializeField] Transform orientation;

    // Horizontal Input
    float horizontalInput;
    // Vertical Input
    float verticalInput;

    // Current Player Movement Variable
    Vector3 moveDirection;

    // Player Rigidbody
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Set Rigidbody
        this.rb = GetComponent<Rigidbody>();
        // Freeze Player Rotation
        rb.freezeRotation = true;

        // Set Jump Ready
        this.readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        this.grounded = Physics.Raycast(this.transform.position, Vector3.down, 
            this.playerHeight * 0.5f + 0.2f, this.whatIsGround);

        // Get Inputs
        MyInput();
        // Check Speed
        SpeedControl();

        // Set Drag
        if (this.grounded) { this.rb.drag = this.groundDrag; }
        else { this.rb.drag = 0; }
    }

    // Fixed Update
    void FixedUpdate()
    {
        // Move Player
        MovePlayer();
    }

    // Get Input
    void MyInput()
    {
        // Get Horizontal Input
        this.horizontalInput = Input.GetAxisRaw("Horizontal");
        // Get Vertical Input
        this.verticalInput = Input.GetAxisRaw("Vertical");

        // Check Jump
        if (Input.GetKey(this.jumpKey) && this.readyToJump && this.grounded)
        {
            // Set Jump False
            this.readyToJump = false;
            // Jump!
            Jump();
            // Set Jump Delay
            Invoke(nameof(ResetJump), this.jumpCooldown);
        }
    }

    // Move Player Function
    void MovePlayer()
    {
        // Calcualate Movement Direction
        this.moveDirection = this.orientation.forward * this.verticalInput 
            + this.orientation.right * this.horizontalInput;

        // Add Force To Player
        // On Ground
        if (grounded) { this.rb.AddForce(this.moveDirection.normalized 
            * this.moveSpeed * 10f, ForceMode.Force); }
        // In Air
        else if (!grounded) { this.rb.AddForce(this.moveDirection.normalized 
            * this.moveSpeed * 10f * this.airMultiplier, ForceMode.Force); }

    }

    // Limit Player Speed
    void SpeedControl()
    {
        // Calculate Velocity Using X/Z Vectors
        Vector3 flatVelocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);

        // Check Speed Limit
        if (flatVelocity.magnitude > this.moveSpeed)
        {
            // Calculate Max Velocity
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            // Apply Velocity
            this.rb.velocity = new Vector3(limitedVelocity.x, this.rb.velocity.y, limitedVelocity.z);
        }
    }

    // Jump Function
    void Jump()
    {
        // Reset Y Velocity
        this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);

        // Add Jump Force
        this.rb.AddForce(this.transform.up * this.jumpForce, ForceMode.Impulse);
    }

    // Jump Reset Bool Function
    void ResetJump()
    {
        this.readyToJump = true;
    }
}