using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold a reference to all the bus stops in scene
/// 
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(ObjectPool))]
public class PeopleSpawner : MonoBehaviour
{
    [Tooltip("The number of enemies to spawn per wave")]
    public int numberOfEnemiesPerWave = 2;
    [Tooltip("The amount of time between waves")]
    public float timeBetweenWaves = 3f;
    [Tooltip("If true, then enemies will spawn")]
    public bool SpawningEnemies = true;

    private float timeSinceLastWave;

    private ObjectPool personObjectPool;

    public Vector3[] peopleSpawnPoints;

    private int lastIndex = -1;

	// Use this for initializationg
	void Start ()
    {
        // Find all objects with the bustop tag
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("Bus Stop");

        // Instantiate the people spawn points array with the proper length
        peopleSpawnPoints = new Vector3[tempArr.Length];

        // Store the positions of the bus stops
        for(int i = 0; i < tempArr.Length;i++)
        {
            peopleSpawnPoints[i] = tempArr[i].transform.position;
        }

        // Get the object pool componenet
        personObjectPool = GetComponent<ObjectPool>();

        // Get our object pool componenet
        personObjectPool = GetComponent<ObjectPool>();

        // Start spawning people
        StartCoroutine(SpawnPeopleRandomly());
    }

    /// <summary>
    /// Randomly spawn people at doors within the given intervals
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <returns>Null</returns>
    private IEnumerator SpawnPeopleRandomly()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        while (SpawningEnemies && GameManager.Instance.CurrentState != GameStates.GameOver)
        {
            Person temp;
            // Spawn people
            for (int i = 0; i < numberOfEnemiesPerWave; i++)
            {
                // Grab an object from the ojbect pool
                temp = personObjectPool.GetPooledObject().GetComponent<Person>();

                // Set the position of the person to a random destination 
                temp.transform.position = peopleSpawnPoints[GetRandomIndex()];

                // Set the element to the transform position
                temp.destination = peopleSpawnPoints[GetRandomIndex()];
            }

            // Wait time between waves of people
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void OnEnable()
    {
        if(GameManager.Instance == null)
        {
            return;
        }

        // Set the game manager's people spawner to this
        GameManager.Instance.PeopleSpawner = this;

        // Start the spawning coroutine
        //StartCoroutine(SpawnPeopleRandomly()); 
    }


    /// <summary>
    /// Return a bus stop at a random index that 
    /// has not been chosen the last time
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <returns>A random position from our bus stop array</returns>
    public int GetRandomIndex()
    {
        // If we have no array, then return 0
        if(peopleSpawnPoints == null)
        {
            return -1;
        }

        // If there is only 1 thing in our array, then just return that
        else if(peopleSpawnPoints.Length == 0)
        {
            return 0;
        }

        // Otherwise generate a random integer
        int randomIndex = Random.Range(0, peopleSpawnPoints.Length - 1);

        while(randomIndex == lastIndex)
        {
            randomIndex = Random.Range(0, peopleSpawnPoints.Length);
        }

        // keep track of the last index that we used
        lastIndex = randomIndex;
        // Return this random index
        return randomIndex;
    }
	
}
