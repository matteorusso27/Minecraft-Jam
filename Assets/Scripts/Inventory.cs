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
        
        var allInventoryItem = Enumerable.Range(0, Enum.GetNames(typeof(InventoryItem)).Length).ToList();
        
        foreach (var i in allInventoryItem)
        {
            if (i == -1) continue;
            storedBlocks.Add((InventoryItem)i, 5);
        }
    }

    public int GetQuantity(InventoryItem DropType) => storedBlocks.ContainsKey(DropType)? storedBlocks[DropType] : 0;
    public Dictionary<InventoryItem, int> GetStoredBlocks() => storedBlocks;

    public void IncrementItem(InventoryItem DropType)
    {
        storedBlocks[DropType] += 1;
    }

    public void DecrementItem(InventoryItem DropType)
    {
        storedBlocks[DropType] -= 1;
    }
    
    public void DecrementItem(InventoryItem DropType,int quantity)
    {
        storedBlocks[DropType] -= quantity;
    }
}
