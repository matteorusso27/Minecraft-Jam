using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, int> storedBlocks;
    public Inventory()
    {
        storedBlocks = new Dictionary<string, int>();
        storedBlocks.Add("GrassDrop", 0);
        storedBlocks.Add("CobbleStoneDrop", 0);
        storedBlocks.Add("WoodDrop", 0);
    }

    public void AddItem(string DropType)
    {
        storedBlocks[DropType] += 1;
    }

    public void RemoveItem(string DropType)
    {
        storedBlocks[DropType] -= 1;
    }

    public int GetQuantity(string DropType)
    {
        if (DropType !=null)
            return storedBlocks[DropType];
        return 0;
    }

    public Dictionary<string,int> GetStoredBlocks()
    {
        return storedBlocks;
    }
}
