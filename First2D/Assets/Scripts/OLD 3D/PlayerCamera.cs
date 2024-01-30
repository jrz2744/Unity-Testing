using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // X Mouse Sens
    [SerializeField] float sensX;
    // Y Mouse Sense
    [SerializeField] float sensY;
    // Player Orientation
    [SerializeField] Transform orientation;
    // Player X Rotation
    [SerializeField] float xRotation;
    // Player Y Rotation
    [SerializeField] float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Lock Mouse To Game
        Cursor.lockState = CursorLockMode.Locked;
        // Set Cursor Invisable
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse X Input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * this.sensX;
        // Get Mouse Y Input
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * this.sensY;

        // Update Player Rotation
        this.yRotation += mouseX;
        this.xRotation -= mouseY;

        // Clamp Rotation - Stops player from looking to far up and down
        this.xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Update Player Model
        transform.rotation = Quaternion.Euler(this.xRotation, this.yRotation, 0);
        this.orientation.rotation = Quaternion.Euler(0, this.yRotation, 0);
    }
}
