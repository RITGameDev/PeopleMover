using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps track of how many people the player has angered so far
/// 
/// Author: Ben Hoffman
/// </summary>
public class AngerManagment : MonoBehaviour {

    [Tooltip("The number of people that the player is allowed to hit before they get fired")]
    public int allowedPeopleAngered = 10;

    public Text Text_maxPeopleAngered;
    public Text Text_currentAngryPeople;

    private int currentAngeredPeople = 0;   // The current number of people hit


    private void Start()
    {
        Text_maxPeopleAngered.text = "/ " + allowedPeopleAngered.ToString();
        Text_currentAngryPeople.text = currentAngeredPeople.ToString();
    }

    /// <summary>
    /// Handle the player angering someone
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void AngeredPerson()
    {
        // Increment the amonut of angererd people
        currentAngeredPeople++;

        Text_currentAngryPeople.text = currentAngeredPeople.ToString();

        // If we have exceeded the max number of people angered...
        if (currentAngeredPeople >= allowedPeopleAngered)
        {
            //Tell the game manager that the game is over
            GameManager.Instance.GameOver();
        }
    }
}
