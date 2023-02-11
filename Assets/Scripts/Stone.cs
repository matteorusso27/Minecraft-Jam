using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : BlockScript
{
    private void Awake()
    {
        maxHealth = 200;
        health = maxHealth;
    }

    protected override void SpawnDrop()
    {
        base.SpawnDrop();
        cubedrop.GetComponent<Renderer>().material.color = Color.grey;
    }
   
}
