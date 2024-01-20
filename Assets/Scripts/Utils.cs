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
        ResultCrafting_Slot
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
}
