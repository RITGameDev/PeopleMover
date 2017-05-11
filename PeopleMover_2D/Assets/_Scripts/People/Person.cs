using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the temper of people. The longer they wait before being picked up,
/// the angrier they will get.
/// 
/// Author: Ben Hoffan
/// </summary>
public class Person : MonoBehaviour
{

    #region Fields

    public float lifeAfterAnger = 1f;
    [Tooltip("How long it will take for this person to get angry when they haven't been picked up")]
    public float startLifetime = 10;

    public Vector3 destination;   // Where this 'person' wants to go

    [Space]
    [Header("Colors!")]
    public Color angryColor;
    public Color happyColor;
    //public PeopleSpawner peopleSpawner;

    private SpriteRenderer spRend;
    private float currentTemper;    // This person's current temper
    private bool pickedUp;
    private bool reportedAngry;
    private float _timeSinceAngry;

    #endregion

    public bool PickedUp { get { return pickedUp; } }
    public bool IsAngry { get { return reportedAngry; } }

	/// <summary>
    /// Get the sprite rend componene,t and set the current temper variable
    /// 
    /// Author: Ben Hoffman
    /// </summary>
	void Start ()
    {
        // Set the current temper of this player
        currentTemper = startLifetime;

        // Get our sprite renderer component
        spRend = GetComponentInChildren<SpriteRenderer>();
    }
	
	/// <summary>
    /// Keep track of the amount of time it takes us to get angry
    /// 
    /// Author: Ben Hoffman
    /// </summary>
	void Update ()
    {
        // As long as we are not picked up already or we have not already reported as angry...
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

            // Lerp our color based on the percentage of how angry we are
            spRend.color = Color.Lerp(angryColor, happyColor, currentTemper / startLifetime);
        }

        // Only stay alive for a certain amount of time after we get angry
        if (reportedAngry)
        {
            // Increase the amount of time since we have been angry
            _timeSinceAngry += Time.deltaTime;

            // If we exceed that amount of time, then...
            if(_timeSinceAngry >= lifeAfterAnger)
            {
                // Set ourselves as inactve
                gameObject.SetActive(false);
            }
        }

	}

    /// <summary>
    /// Get angry when we enter a trigger that is a person
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we collide with the player, then get angry
        if (collision.CompareTag("Player"))
        {
            // Tell this person to get angry
            GetAngry();
        }
    }

    /// <summary>
    /// Set the color to to the happy color, unparent this object,
    /// and start the lifecycle countdown
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void DropOff()
    {
        // Make the person happy
        spRend.color = happyColor;
        // De parent the person
        transform.parent = null;
        // Disappear after a second
        reportedAngry = true;
    }

    /// <summary>
    /// Handle this person getting angry
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void GetAngry()
    {
        // If we are not playing, then return
        if(GameManager.Instance.CurrentState != GameStates.Playing)
        {
            return;
        }
        // TODO: Show little exclamation points

        // Report to the player that they have angered someone
        GameManager.Instance.AngerManager.AngeredPerson();

        // Set our sprite to angry
        spRend.color = angryColor;
        // Report that we have been angry
        reportedAngry = true;
    }

    /// <summary>
    /// Call this method from the cart manager to let the player know that
    /// they are in range of some people
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void InRangeOfCart()
    {
        // If we are not reported as angry...
        if (!reportedAngry)
        {
            // Change our sprite
            spRend.color = happyColor;
        }
    }

    /// <summary>
    /// Use this to handle things that individual people will do 
    /// when they get picked up, like set their object as inactive, 
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void GetPickedUp()
    {
        if (!reportedAngry)
        {
            pickedUp = true;

            // Set our sprite to angry
            spRend.color = Color.black;

        }
    }

    /// <summary>
    /// Ensure that this person does not have a parent object
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void OnEnable()
    {
        // Unparent the object
        transform.parent = null;
    }

    /// <summary>
    /// Ensure that when this person has been reset and
    /// is ready to be put back in the object pool
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void OnDisable()
    {
        // Reset the amount of time since we have reported as angry
        _timeSinceAngry = 0f;
        // Reset the start temper of the object
        currentTemper = startLifetime;
        // Reset the flags of if we reported as angry
        reportedAngry = false;
        // We have not been picked up yet
        pickedUp = false;
    }

}
