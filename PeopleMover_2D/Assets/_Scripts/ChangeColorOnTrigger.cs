using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the color of this object's sprite when
/// it enters a given trigger;
/// 
/// Author: Ben Hoffman
/// </summary>
public class ChangeColorOnTrigger : MonoBehaviour {

    public Color ColorToChangeTo;
    public string triggerZone = "Player";

    private Color startColor;
    private SpriteRenderer spRend;

    private void Start()
    {
        spRend = GetComponentInChildren<SpriteRenderer>();
        startColor = spRend.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerZone))
        {
            spRend.color = ColorToChangeTo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerZone))
        {
            spRend.color = startColor;
        }
    }
}
