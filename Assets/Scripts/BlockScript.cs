using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField]
    public int health = 1000;

    public void DoDamage()
    {
        health--;
        if (health == 0) Destroy(gameObject);
        if (health == 2) GetComponent<Renderer>().material.color = Color.blue;
        if (health == 1) GetComponent<Renderer>().material.color = Color.red;
    }
   
}
