using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the temper of people. The longer they wait before being picked up,
/// the angrier they will get.
/// 
/// Author: Ben Hoffan
/// </summary>
public class Person : MonoBehaviour {

    public AngerManagment angerManager;

    public float startTemper;

    public Transform destination;   // Where this 'person' wants to go

    public Sprite angryPersonSprite;
    public Sprite inRangePersonSprite;

    private float currentTemper;    // This person's current temper

    private bool pickedUp;
    private bool reportedAngry;

    private Sprite startingSprite;

    public bool PickedUp { get { return pickedUp; } }

	// Use this for initialization
	void Start ()
    {
        // Set the current temper of this player
        currentTemper = startTemper;
        startingSprite = GetComponentInChildren<SpriteRenderer>().sprite;
        //TODO:  Pick a destination point
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!pickedUp && !reportedAngry)
        {
            // Decrememnet the temper of this person
            currentTemper -= Time.deltaTime;

            // Check if our temper is 0
            if (currentTemper <= 0f)
            {
                // Get angry 
                GetAngry();
            }
        }

	}

    public void GetAngry()
    {
        // Report to the player that they have angered someone
        angerManager.AngeredPerson();

        // Set our sprite to angry
        GetComponentInChildren<SpriteRenderer>().sprite = angryPersonSprite;

        reportedAngry = true;
    }

    /// <summary>
    /// Call this method from the cart manager to let the player know that
    /// they are in range of some people
    /// </summary>
    public void InRangeOfCart()
    {
        if (!reportedAngry)
        {
            // Change our sprite
            GetComponentInChildren<SpriteRenderer>().sprite = inRangePersonSprite;
        }
    }

    /// <summary>
    /// Call this method from the cart class to let the player know that they are out
    /// of range of the cart now
    /// </summary>
    public void OutOfRangeCart()
    {
        if (!reportedAngry)
        {
            // Change our sprite
            GetComponentInChildren<SpriteRenderer>().sprite = startingSprite;
        }
    }

    public void GetPickedUp()
    {
        if (!reportedAngry)
        {
            pickedUp = true;

            // Set our sprite to angry
            GetComponentInChildren<SpriteRenderer>().sprite = angryPersonSprite;

        }
    }
}
