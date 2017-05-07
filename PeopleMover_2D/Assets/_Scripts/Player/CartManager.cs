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

    public UnityEngine.UI.Text Text_currentPeopleCount;
    public UnityEngine.UI.Text Text_maxPeopleCount;


    [Tooltip("This is the layer that you can pick people up on, without angering them")]
    public LayerMask pickUpRangeMask;
    [Tooltip("This is the layer that people get hurt on")]
    public LayerMask hittingPersonMask;

    private AngerManagment angerManager;

    private Person personInRange;

    private int currentPeopleInCart;

    private Person[] peopleInCart;


    private void Start()
    {
        // Get the anger manager
        angerManager = GetComponent<AngerManagment>();

        // Instantiate the number of people that we have seats for
        peopleInCart = new Person[numberOfSeats];

        // Set up the UI
        Text_maxPeopleCount.text = "/ " + numberOfSeats.ToString();
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();
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
        else if (collision.gameObject.CompareTag("PersonInRange"))
        {
            // Keep track of that person 
            personInRange = collision.GetComponentInParent<Person>();
            // let the cart know that we are in range of the cart now
            personInRange.InRangeOfCart();
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
            personInRange.OutOfRangeCart();

            personInRange = null;
        }
    }

    private void PickUpPerson()
    {
        // Keep track of what people we are carrying
        peopleInCart[currentPeopleInCart] = personInRange;

        // Tell the person to get picked up
        personInRange.GetPickedUp();

        // Parent the transform of the person to our cart
        personInRange.transform.SetParent(transform);

        // Remove the person that we were in range with from our temp placeholder object
        personInRange = null;
        // Increase the number of people that are in our cart
        currentPeopleInCart++;

        // Update the UI 
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();

    }


    private void DropOffPerson()
    {
        currentPeopleInCart--;

        // Update the UI
        Text_currentPeopleCount.text = currentPeopleInCart.ToString();

    }


}
