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
        cubedrop.GetComponent<Renderer>().material.color = new Color(151 / 255, 73 / 255, 4 / 255, 1);
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
