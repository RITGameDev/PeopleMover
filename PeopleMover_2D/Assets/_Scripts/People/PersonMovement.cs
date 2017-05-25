using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Give the people a wander-like movement pattern when they spawn
/// 
/// Author: Ben Hoffman
/// </summary>
public class PersonMovement : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}

    private void OnEnable()
    {
        // Add a constant force in the forward direction   
    }

    private void OnDisable()
    {
        // Set the velocity to zero and remove the constant force   
    }

    // Update is called once per frame
    void Update ()
    {
		// Check if there is something in front of us

        // If there is, then turn like 10 degrees
	}
}
