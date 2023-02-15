using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobbleStone : BlockScript
{
    public Material[] crackMaterials;
    private void Awake()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        dropTag = "CobbleStoneDrop";
        materials = crackMaterials;
    }
    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
