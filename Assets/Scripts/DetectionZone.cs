using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List <Collider2D> DetectedColliders = new List <Collider2D> ();
    Collider2D col;

    public UnityEvent noCollidersRemain;

    public void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectedColliders.Add (collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DetectedColliders.Remove (collision);

        if(DetectedColliders.Count <= 0)
        {
            noCollidersRemain.Invoke ();
        }
    }
}
