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


    private AngerManagment angerManager;
    private Person personInRange;
    private int currentPeopleInCart;

    private Queue<Person> peopleInCart;

    private Transform[] destinationObjects; 


    private void Start()
    {
        // Get the anger manager
        angerManager = GetComponent<AngerManagment>();

        // Instantiate the number of people that we have seats for
        peopleInCart = new Queue<Person>();

        // Set up the UI
        Text_maxPeopleCount.text = "/ " + numberOfSeats.ToString();
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();

        // Set up the destination markers
        destinationObjects = new Transform[numberOfSeats];

        for (int i = 0; i < numberOfSeats; i++)
        {
            // Instantiate one of these objects
            GameObject temp = Instantiate(showDestination_Prefab);
            // Set it as inactive
            temp.gameObject.SetActive(false);
            // Keep track of this objects position for later
            destinationObjects[i] = temp.transform;
        }
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
        if (collision.gameObject.CompareTag("Person"))
        {
            // Anger that person
            collision.GetComponent<Person>().GetAngry();
        }
        else if (collision.gameObject.CompareTag("PersonInRange") && personInRange == null)
        {
            // Keep track of that person 
            personInRange = collision.GetComponentInParent<Person>();

            // If this person is angry then get rid of them
            if (personInRange.IsAngry)
            {
                // Get rid of them

            }
            else
            {

                // let the cart know that we are in range of the cart now
                personInRange.InRangeOfCart();
            }

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

        // Set the destination object to the destination of the person
        destinationObjects[currentPeopleInCart].gameObject.SetActive(true);
        destinationObjects[currentPeopleInCart].transform.position = personInRange.destination.position;

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
        if (!peopleInCart.Peek())
        {
            //TODO: Add some sort of bad sound to make sure that the player knows
            return;
        }

        currentPeopleInCart--;
        
        // Update the UI
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();

    }


}
