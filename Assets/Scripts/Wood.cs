using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BlockScript
{
    public Material[] crackMaterials;
    private void Awake()
    {
        maxHealth = 75f;
        currentHealth = maxHealth;
        dropTag = "WoodDrop";
        materials = crackMaterials;
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
