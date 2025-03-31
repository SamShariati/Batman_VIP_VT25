using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust movement speed in the Inspector

    private void Update()
    {
        // Get horizontal and vertical input (WASD keys or Arrow keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down arrow keys

        // Create a direction vector based on input and normalize it to ensure consistent speed
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the player based on direction, speed, and time
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}
