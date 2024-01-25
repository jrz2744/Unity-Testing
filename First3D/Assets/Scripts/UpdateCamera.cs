using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCamera : MonoBehaviour
{
    // Camera Position
    [SerializeField] Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        // Move Camera to Player
        transform.position = cameraPosition.position;
    }
}
