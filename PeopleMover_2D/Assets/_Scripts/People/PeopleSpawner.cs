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
    private Vector3[] peopleSpawnPoints;
    private int lastIndex = -1;

	// Use this for initializationg
	void Start ()
    {
        // Find all objects with the bustop tag
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("Bus Stop");
        // Instantiate the people spawn points array with the proper length
        peopleSpawnPoints = new Vector3[tempArr.Length]; 

        // Set our people positions to the given positions of the bus stops
        for (int i = 0; i < tempArr.Length; i++)
        {
            // Set the element to the transform position
            peopleSpawnPoints[i] = tempArr[i].transform.position;
        }

        // Get our object pool componenet
        personObjectPool = GetComponent<ObjectPool>();

        // Start the spawning coroutine
        StartCoroutine(SpawnPeopleRandomly());
	}

    /// <summary>
    /// Randomly spawn people at doors within the given intervals
    /// </summary>
    /// <returns>Null</returns>
    private IEnumerator SpawnPeopleRandomly()
    {
        while (SpawningEnemies)
        {
            // If it has been long enough in between waves
            if(timeSinceLastWave >= timeBetweenWaves)
            {
                // Spawn perople
                for(int i = 0; i < numberOfEnemiesPerWave; i++)
                {
                    // Grab an object from the ojbect pool
                    GameObject temp = personObjectPool.GetPooledObject();
                    // Set the position of that object to one of the bus stops
                    temp.transform.position = GetRandomBusStop();
                    yield return null;
                }

                // Reset the time since last wave
                timeSinceLastWave = 0f;
            }
            else
            {
                // Increase the time since the last wave
                timeSinceLastWave += Time.deltaTime;
            }

            yield return null;
        }

        
    }

    private void OnEnable()
    {
        // Set the game manager's people spawner to this
        GameManager.Instance.PeopleSpawner = this;
    }


    /// <summary>
    /// Return a bus stop at a random index that 
    /// has not been chosen the last time
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <returns>A random position from our bus stop array</returns>
    public Vector3 GetRandomBusStop()
    {
        // If we have no array, then return 0
        if(peopleSpawnPoints == null)
        {
            return Vector3.zero;
        }
        // If there is only 1 thing in our array, then just return that
        else if(peopleSpawnPoints.Length == 0)
        {
            return peopleSpawnPoints[0];
        }

        // Otherwise generate a random integer
        int randomIndex = Random.Range(0, peopleSpawnPoints.Length);

        while(randomIndex == lastIndex)
        {
            randomIndex = Random.Range(0, peopleSpawnPoints.Length);
        }

        return peopleSpawnPoints[randomIndex];
    }
	
}
