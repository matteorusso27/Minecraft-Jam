using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobbleStone : BlockScript
{
    private void Awake()
    {
        maxHealth = 250;
        health = maxHealth;
        dropTag = "CobbleStoneDrop";
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.GetComponent<Renderer>().material.color = new Color(151 / 255, 73 / 255, 4 / 255, 1);
        cubedrop.gameObject.name = dropTag;
        cubedrop.gameObject.tag = dropTag;
    }
}
