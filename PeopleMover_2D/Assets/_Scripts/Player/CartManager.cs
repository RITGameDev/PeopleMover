using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will handle what we do when we want to pick up a person
/// 
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(AngerManagment))]
public class CartManager : MonoBehaviour {

    [Tooltip("A particle system that will be played when we drop people off")]
    public ParticleSystem dropOffParticles;
    public int numberOfSeats;
    [Tooltip("One of these objects will be placed at every person's destination")]
    public GameObject showDestination_Prefab;

    [Space]

    [Header("Collision Masks")]
    [Tooltip("This is the layer that you can pick people up on, without angering them")]
    public LayerMask pickUpRangeMask;
    [Tooltip("This is the layer that people get hurt on")]
    public LayerMask hittingPersonMask;

    [Space]
  
    [Header("User Interface")]
    public UnityEngine.UI.Text Text_currentPeopleCount;
    public UnityEngine.UI.Text Text_maxPeopleCount;
    public UnityEngine.UI.Text Text_HappyPeopleCount;

    //private AngerManagment angerManager;
    private Person personInRange;
    private int currentPeopleInCart;

    private Queue<Person> peopleInCart;
    private Transform destinationObject;

    private float happyCount = 0;

    private void Start()
    {
        // Instantiate the number of people that we have seats for
        peopleInCart = new Queue<Person>();

        // Set up the UI
        Text_maxPeopleCount.text = "/ " + numberOfSeats.ToString();
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();

        // Set up the destination markers
        //destinationObjects = new Transform[numberOfSeats];

        destinationObject = Instantiate(showDestination_Prefab).transform;
        destinationObject.gameObject.SetActive(false);

        Text_HappyPeopleCount.text = happyCount.ToString();
    }

    /// <summary>
    /// Listen to input from the player to see if we want to pick up
    /// a person or not
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && personInRange != null)
        {
            // Pick up this person
            PickUpPerson();
        }      

    }


    /// <summary>
    /// If we enter a trigger that is a person, then 
    /// handle angering anoother person
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <param name="collision">The object that we have entered the trigger zone with</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Person"))
        {
            // Anger that person
            collision.GetComponent<Person>().GetAngry();
        }*/
        if (collision.gameObject.CompareTag("PersonInRange") && personInRange == null)
        {
            // Keep track of that person 
            personInRange = collision.GetComponentInParent<Person>();

            // If this person is angry then get rid of them
            if (personInRange.IsAngry)
            {
                // Get rid of them
                personInRange = null;
                return;
            }
            else
            {
                // let the cart know that we are in range of the cart now
                personInRange.InRangeOfCart();
            }

        }
        // If we are colliding with a bus stop...
        else if (collision.gameObject.CompareTag("Destination"))
        {
            // Try and drop someone off
            DropOffPerson();
        }

        #region Using Layers (Not working)
        // If we are hitting a person
        /*   if (collision.gameObject.layer == hittingPersonMask)
           {
               Debug.Log("YOU HIT A PERSON!!");
               // Handle angering a person
               angerManager.AngeredPerson();
           }
           // If we are in the pickup range of a person
           else if (collision.IsTouchingLayers(pickUpRangeMask))
           {
               Debug.Log("You can pick up a person now");

               // Keep track of that person 
               personInRange = collision.GetComponent<Person>();

               //TODO: Change the color this person so that the user knows that they are within range
           }*/
        #endregion
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we leave a bus stop then we can no longer pick people up[
        if (collision.gameObject.CompareTag("PersonInRange") && personInRange != null)
        {
            personInRange = null;
        }
    }

    private void PickUpPerson()
    {
        if(peopleInCart.Count >= numberOfSeats)
        {
            //TODO: Tell the player somehow that their cart is full

            return;
        }

        // Add this person to the queue
        peopleInCart.Enqueue(personInRange);

        // Tell the person to get picked up
        personInRange.GetPickedUp();

        // Parent the transform of the person to our cart
        personInRange.transform.SetParent(transform);

        personInRange.transform.localPosition = Vector3.zero;

        destinationObject.gameObject.SetActive(true);
        destinationObject.position = personInRange.destination;
        
        // Remove the person that we were in range with from our temp placeholder object
        personInRange = null;

        // Increase the number of people that are in our cart
        currentPeopleInCart++;

        // Update the UI 
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();
        
    }

    private void DropOffPerson()
    {
        // If we have nobody, then return
        if (peopleInCart.Count == 0)
        {
            //TODO: Add some sort of bad sound to make sure that the player knows
            return;
        }
        // If we have the wrong destination then return
        if(destinationObject.transform.position != peopleInCart.Peek().destination)
        {
            return;
        }
        // Turn off this destination
        destinationObject.gameObject.SetActive(false);

        // Tell the person to be dropped off
        peopleInCart.Dequeue().DropOff();
        // Decrement the amount of people in the cart
        currentPeopleInCart--;

        // Play the drop off effect and sound
        dropOffParticles.Play();

        // Update the UI
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();
        
        // Increate the number of people made happy
        happyCount++;
        Text_HappyPeopleCount.text = happyCount.ToString();

        // If we have nobody left now, then return
        if(peopleInCart.Count <= 0)
        {
            return;
        }

        // Move the destination shower to the next destination
        if (peopleInCart.Peek().destination == destinationObject.position)
        {
            // Call drop off person again
            DropOffPerson();
        }
        // Otherwise...
        else
        {
            // Move the destination to the next person's destination
            destinationObject.position = peopleInCart.Peek().destination;
        }


    }


}
