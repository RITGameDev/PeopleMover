using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the player movement for the game.
/// WASD to move the golf cart up and down on the screen, 
/// no gravity
/// 
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    // Input axes
    public string horiz_input_axis = "Horizontal";
    public string vert_input_axis = "Vertical";

    //Speeds
    public float speed = 1f;

    public float MAX_SPEED = 5f;

    // The movement vector for this object
    private Vector2 movementVector;

    // The rigidbody of this object for movement
    private Rigidbody2D rb_2d;  

	// Use this for initialization
	void Start ()
    {
        // Get the 2d rigidbody componenet
        rb_2d = GetComponent<Rigidbody2D>();
        // Don't use gravity
        rb_2d.gravityScale = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Reset the movement vector
        movementVector.x = 0f;
        movementVector.y = 0f;

        // Get the player input
        movementVector.x = Input.GetAxis(horiz_input_axis);
        movementVector.y = Input.GetAxis(vert_input_axis);

        // If we actually need to do the calculations of movement...
        if(movementVector.magnitude != 0f)
        {
            // Add a force the player Rigidbody
            rb_2d.AddForce(movementVector * speed, ForceMode2D.Impulse);
        }

        // If we are going faster then the max speed that we want...
        if(rb_2d.velocity.magnitude > MAX_SPEED)
        {
            // Add a force in the opposite direction, scaled by how much faster we are going
            rb_2d.AddForce(-rb_2d.velocity.normalized * (rb_2d.velocity.magnitude - MAX_SPEED), ForceMode2D.Impulse);

            //float angle = Mathf.Atan2(-rb_2d.velocity.x, rb_2d.velocity.y) * Mathf.Rad2Deg;
            float angle = Mathf.Atan2(-movementVector.x, movementVector.y) * Mathf.Rad2Deg;
            // Rotate to look in the direction that we are moving
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


	}


}
