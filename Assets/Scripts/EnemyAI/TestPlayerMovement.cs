using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    // Movement speed in units per second
    [SerializeField] private float moveSpeed = 5f;
     public int soundIntensity;
    public bool currentlyMakingSound = false;

    private float currentTimer = 0;
    private float totalTime = 3;

    // Reference to the character's Rigidbody component
    private Rigidbody rb;

    // Vector to store movement input
    private Vector3 movement;

    // Called before the first frame update
    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();

        // If there's no Rigidbody component, add one
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            // Freeze rotation to prevent the character from tipping over
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from arrow keys
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Left/Right arrow keys
        float verticalInput = Input.GetAxisRaw("Vertical");     // Up/Down arrow keys

        // Create movement vector (we're moving on the XZ plane in 3D)
        movement = new Vector3(horizontalInput, 0, verticalInput).normalized;

        GetSoundStrength();
    }

    // FixedUpdate is called at a fixed interval and is used for physics calculations
    void FixedUpdate()
    {
        // Move the character based on input
        MoveCharacter();
    }

    // Function to handle character movement
    void MoveCharacter()
    {
        // Apply movement using Rigidbody
        rb.linearVelocity = movement * moveSpeed;
    }

    private void GetSoundStrength()
    {
        // For regular number keys (1-5 at the top of keyboard)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            soundIntensity = 1;
            currentlyMakingSound = true;
            currentTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            soundIntensity = 2;
            currentlyMakingSound = true;
            currentTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            soundIntensity = 3;
            currentlyMakingSound = true;
            currentTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            soundIntensity = 4;
            currentlyMakingSound = true;
            currentTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            soundIntensity = 5;
            currentlyMakingSound = true;
            currentTimer = 0;
        }

        if (currentlyMakingSound)
        {
            MakeSound();
        }

    }
    private void MakeSound()
    {

        currentTimer += Time.deltaTime;

        if (currentTimer > totalTime)
        {
            currentlyMakingSound = false;
            currentTimer = 0;
        }

    }

}
