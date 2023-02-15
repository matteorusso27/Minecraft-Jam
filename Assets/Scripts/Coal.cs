using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : BlockScript
{
    public Material[] crackMaterials;
    private void Awake()
    {
        maxHealth = 50f;
        currentHealth = maxHealth;
        dropTag = "CoalDrop";
        materials = crackMaterials;
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
