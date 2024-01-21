using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utils;

public class Inventory
{
    private Dictionary<InventoryItem, int> storedBlocks;
    public Inventory()
    {
        storedBlocks = new Dictionary<InventoryItem, int>();
        
        var allInventoryItem = Enum.GetValues(typeof(InventoryItem));

        foreach (InventoryItem i in allInventoryItem)
        {
            if (i == InventoryItem.None) continue;
            storedBlocks.Add(i, 2);
        }
    }

    public int GetQuantity(InventoryItem item) => storedBlocks.ContainsKey(item) ? storedBlocks[item] : 0;
    public Dictionary<InventoryItem, int> GetStoredBlocks() => storedBlocks;

    public void IncrementItem(InventoryItem item)
    {
        storedBlocks[item] += 1;
    }

    public void DecrementItem(InventoryItem item, int quantity = 1)
    {
        storedBlocks[item] -= quantity;
    }

    public void RemoveItem(InventoryItem item) => storedBlocks.Remove(item);
}
