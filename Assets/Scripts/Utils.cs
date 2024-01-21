using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public enum Tags
    {
        None = -1,
        Player,
        Block,
        Enemy,
        DamagePlayer,
        Left_Crafting_Slot,
        Right_Crafting_Slot,
        CobbleStoneDrop,
        WoodDrop,
        GrassDrop,
        CoalDrop,
        LowBar,
        CraftManager,
        CraftingPanel,
        ResultCrafting_Slot,
        PickObjectManager,
        BigEnvironment,
        Environment,
        GameStateHandler,
        Raycaster
    }

    public enum InventoryItem
    {
        None = -1,
        Grass,
        Wood,
        Coal,
        CobbleStone,
        Pike
    }

    public enum GameState
    {
        PlayMode,
        CraftingMode
    }

    public static GameObject FindGameObjectWithTag(Tags tag) => GameObject.FindGameObjectWithTag(tag.ToString());
    public static GameObject[] FindGameObjectsWithTag(Tags tag) => GameObject.FindGameObjectsWithTag(tag.ToString());
    public static GameObject FindByName(string name) => GameObject.Find(name);
    public static InventoryItem ConvertTextureToItem(string text)
    {
        Enum.TryParse(text.Replace("Icon",""), out InventoryItem item);
        return item;
    }
}
