using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BlockScript
{
    private void Awake()
    {
        maxHealth = 50;
        health = maxHealth;
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.GetComponent<Renderer>().material.color = new Color(151/255,73/255,4/255,1);
    }
}
