using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class CollectorScript : MonoBehaviour
{
    public Inventory inventory;
    private AudioSource pickupSoundManager;
    private void Awake()
    {
        pickupSoundManager = FindGameObjectWithTag(Tags.PickObjectManager).GetComponent<AudioSource>();
        inventory = new Inventory();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enum.TryParse(other.gameObject.tag, out Tags tag);
        switch (tag)
        {
            case Tags.CobbleStoneDrop:
                HandleDrop(other.gameObject);
                inventory.IncrementItem(InventoryItem.CobbleStone);
                break;
            case Tags.WoodDrop:
                HandleDrop(other.gameObject);
                inventory.IncrementItem(InventoryItem.Wood);
                break;
            case Tags.GrassDrop:
                HandleDrop(other.gameObject);
                inventory.IncrementItem(InventoryItem.Grass);
                break; 
            case Tags.CoalDrop:
                HandleDrop(other.gameObject);
                inventory.IncrementItem(InventoryItem.Coal);
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
        pickupSoundManager.Play();
    }
}
