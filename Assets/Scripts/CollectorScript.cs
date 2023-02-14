using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    public Inventory inventory;
    private void Awake()
    {
        inventory = new Inventory();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CobbleStoneDrop":
                HandleDrop(other.gameObject);
                inventory.IncrementItem("CobbleStoneDrop");
                break;
            case "WoodDrop":
                HandleDrop(other.gameObject);
                inventory.IncrementItem("WoodDrop");
                break;
            case "GrassDrop":
                HandleDrop(other.gameObject);
                inventory.IncrementItem("GrassDrop");
                break;
            default:
                break;
        }
    }

    private void HandleDrop(GameObject drop)
    {
        drop.gameObject.GetComponent<Renderer>().enabled = false;
        drop.gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(drop.gameObject);
    }
}
