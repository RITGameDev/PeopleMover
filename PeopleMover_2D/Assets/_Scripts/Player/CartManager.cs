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

    #region Fields

    [Tooltip("A particle system that will be played when we drop people off")]
    public ParticleSystem dropOffParticles;

    [Tooltip("How many seats that we have in the cart")]
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

    // A temp object to keep track of the person that is in the range right now
    private Person personInRange;
    // A queue to hold the people that we have picked up in the cart
    private Queue<Person> peopleInCart;
    // A reference to destination object's transform
    private Transform destinationObject;

    private AudioSource audioSource;

    private bool isDestination;
    #endregion

    private void Start()
    {
        // Instantiate the number of people that we have seats for
        peopleInCart = new Queue<Person>();

        // Set up the UI
        Text_maxPeopleCount.text = "/ " + numberOfSeats.ToString();
        Text_currentPeopleCount.text = peopleInCart.Count.ToString();

        // Instantiate the destination object prefab
        destinationObject = Instantiate(showDestination_Prefab).transform;
        destinationObject.gameObject.SetActive(false);

        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Listen to input from the player to see if we want to pick up
    /// a person or not
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void Update()
    {
        // Make sure that we know that we are not colliding with destination
        isDestination = false;
        // If you press E and you have a person in range
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
        // If we are in the pickup range of a person...
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
        else if (collision.gameObject.CompareTag("Destination") && !isDestination)
        {
            isDestination = true;
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

    /// <summary>
    /// If we are out of the range of a person now, set the personInRange
    /// to null.
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we leave a bus stop then we can no longer pick people up[
        if (collision.gameObject.CompareTag("PersonInRange") && personInRange != null)
        {
            personInRange = null;
        }
    }

    /// <summary>
    /// Enque the person in the range to the queue that is our cart
    /// Set the transform of that person to the cart
    /// </summary>
    private void PickUpPerson()
    {
        // If our cart is already full...
        if(peopleInCart.Count >= numberOfSeats)
        {
            //TODO: Tell the player somehow that their cart is full
            // Don't pick anyone up
            return;
        }


        // Add this person to the queue
        peopleInCart.Enqueue(personInRange);

        // Tell the person to get picked up
        personInRange.GetPickedUp();

        // Parent the transform of the person to our cart
        personInRange.transform.SetParent(transform);

        // Set the local positoin of that person to 0 in our cart
        personInRange.transform.localPosition = Vector3.zero;

        // If this is the first person in the queue...
        if(peopleInCart.Count == 1)
        {
            // Activate the destinaion object
            destinationObject.gameObject.SetActive(true);
            destinationObject.position = personInRange.destination;
        }

        
        // Remove the person that we were in range with from our temp placeholder object
        personInRange = null;

        // Increase the number of people that are in our cart
        //currentPeopleInCart++;

        // Update the UI 
        Text_currentPeopleCount.text = peopleInCart.Count.ToString();
        
    }

    /// <summary>
    /// Dequeue a person and drop them off, play the nice little particle
    /// effect and a sound 
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void DropOffPerson()
    {
        // If we have nobody, then return
        if (peopleInCart.Count == 0)
        {
            return;
        }

        // If we have the wrong destination then return
      /*  if(destinationObject.transform.position != peopleInCart.Peek().destination)
        {
            return;
        }*/

        // Turn off this destination

        // Tell the person to be dropped off
        peopleInCart.Dequeue().DropOff();

        // Play the drop off effect and sound
        dropOffParticles.Play();

        // Play the simple ping audio
        audioSource.Play();

        // Update the UI
        Text_currentPeopleCount.text = peopleInCart.Count.ToString();
        
        // Increate the number of people made happy
        GameManager.Instance.AngerManager.CurrentHappyPeople++;

        // If we have nobody left now, then return
        if (peopleInCart.Count <= 0)
        {
            // Turn off the destination 
            destinationObject.gameObject.SetActive(false);
            return;
        }

        // Show the next destination
        destinationObject.position = peopleInCart.Peek().destination;
        // Move the destination shower to the next destination

    }


}
