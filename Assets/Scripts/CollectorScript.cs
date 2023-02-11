using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    public Dictionary<string, int> storedBlocks;
    private void Awake()
    {
        storedBlocks = new Dictionary<string, int>();
        storedBlocks.Add("GrassDrop", 0);
        storedBlocks.Add("CobbleStoneDrop", 0);
        storedBlocks.Add("WoodDrop", 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CobbleStoneDrop":
                HandleDrop(other.gameObject);
                UpdateValues("CobbleStoneDrop");
                break;
            case "WoodDrop":
                HandleDrop(other.gameObject);
                UpdateValues("WoodDrop");
                break;
            case "GrassDrop":
                HandleDrop(other.gameObject);
                UpdateValues("GrassDrop");
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

    private void UpdateValues(string dropType)
    {
        storedBlocks[dropType] += 1;
        foreach (KeyValuePair<string, int> item in storedBlocks)
        {
            Debug.Log("Key: "+item.Key+ " Value: "+item.Value);
        }
    }
}
