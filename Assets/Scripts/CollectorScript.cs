using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    GameObject toDestroy;
    bool proceed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drop"))
        {

            other.gameObject.GetComponent<Renderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(other.gameObject);
        }
    }
}
