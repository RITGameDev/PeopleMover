using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A simple smooth camera follow script for the game
/// 
/// AUthor: Ben Hoffman
/// </summary>
public class SmoothCameraFollow : MonoBehaviour {

    [Tooltip("The object that we want to follow.")]
    public Transform followTarget;
    
    // How smooth of camera movement we want
    public float smoothing = 1f;

    // The offset from the object
    public Vector3 offset;

    // Use this vector for calculations in LateUpdate
    private Vector3 desiredPosition;


    private void Start()
    {
        // Set our position to where we want to be
        transform.position = followTarget.position + offset;
    }

    /// <summary>
    /// Lerp betweeen the current position of this object and the 
    /// target object
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    void LateUpdate ()
    {
        // Calculate the desired position
        desiredPosition = followTarget.position + offset;
        // Set our position based on a quick lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing);
	}
}
