using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will handle what we do when we 
/// </summary>
public class PeopleCollisions : MonoBehaviour {

    [Tooltip("The number of people that the player is allowed to hit before they get fired")]
    public int allowedPeopleAngered = 10;

    private int currentAngeredPeople = 0;   // The current number of people hit

    /// <summary>
    /// If we enter a trigger that is a person, then 
    /// handle angering anoother person
    /// </summary>
    /// <param name="other">The object that we have entered the trigger zone with</param>
    private void OnTriggerEnter(Collider other)
    {
        // If we are hitting a person
        if (other.gameObject.CompareTag("Person"))
        {
            AngeredPerson();
        }
    }


    /// <summary>
    /// Handle the player angering someone
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void AngeredPerson()
    {
        // Increment the amonut of angererd people
        currentAngeredPeople++;

        // If we have exceeded the max number of people angered...
        if(currentAngeredPeople >= allowedPeopleAngered)
        {
            // Game Over
            Debug.Log("Game Over!");
        }
    }
}
