using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provide a simple objcet pool script to use for this game
/// 
/// Author: Ben Hoffman
/// </summary>
public class ObjectPool : MonoBehaviour {

    [Tooltip("The person prefab object that will be spawned at random")]
    public GameObject pooledObj_Prefab;
    [Tooltip("How many of these objects to instantiate on start")]
    public int pooledAmount = 20;
    [Tooltip("If we need more then the inital pooled amount, then this will instanitate a new object")]
    public bool willGrow = true;

    private List<GameObject> objectList;

    /// <summary>
    /// Instantiate the necessary number of pooled objects
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    void Start ()
    {
        objectList = new List<GameObject>();
        
		for(int i = 0; i < pooledAmount; i++)
        {
            // Instantiate the pooled object and add it to our list
            objectList.Add(Instantiate(pooledObj_Prefab));
            // Set it as INACTIVE
            objectList[i].SetActive(false);
        }
	}

    /// <summary>
    /// Loop through our list until we find an inactive object,
    /// when we find an active one return it. If there are no active objects in the 
    /// pool, then instantiate a new one
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    /// <returns>One of the pooled objects</returns>
    public GameObject GetPooledObject()
    {
        // Loop through our known array
        for(int i = 0; i < objectList.Count; i++)
        {
            // If the object at this spot is INACTIVE
            if (!objectList[i].activeInHierarchy)
            {
                // Actiate it in the heirarchy
                objectList[i].SetActive(true);

                // Return it
                return objectList[i];
            }
        }

        // return an instantiate pooled object
        GameObject temp = Instantiate(pooledObj_Prefab);
        // Add it to our object pooled
        objectList.Add(temp);
        // Return it
        return Instantiate(pooledObj_Prefab);
    }
	
}
