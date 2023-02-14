using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : BlockScript
{
    public Material[] crackMaterials;
    private void Awake()
    {
        maxHealth = 35f;
        currentHealth = maxHealth;
        dropTag = "GrassDrop";
        materials = crackMaterials;
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.GetComponent<Renderer>().material.color = Color.green;
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
