using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NOatt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ATTNO"))
        {
            Destroy(gameObject);
        }
    }
}