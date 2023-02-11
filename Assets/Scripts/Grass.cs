using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : BlockScript
{
    private void Awake()
    {
        maxHealth = 35;
        health = maxHealth;
        dropTag = "GrassDrop";
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.GetComponent<Renderer>().material.color = Color.green;
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
