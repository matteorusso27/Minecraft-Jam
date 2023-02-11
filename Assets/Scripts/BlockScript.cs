using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    protected int maxHealth;
    public int health; //levare il public, è solo per debug

    //Block drop variables
    protected GameObject cubedrop;
    protected string dropTag = "Drop"; 
    protected Coroutine DropAnimationCoRoutine;
    protected float offsetAnimation = 0.2f;
    protected static Vector3 scaleChangeNewCube = new Vector3(0.2f, 0.2f, 0.2f);
    protected float dropUpwardsSpeed = 0.25f;
    protected float dropRotationSpeed = 15f;

    public void GetDamage()
    {
        health--;
        if (health == 0) DropAndDestroy();
        if (health <= maxHealth * (3 / 4)) GetComponent<Renderer>().material.color = Color.blue;
        if (health <= maxHealth * (1 / 2)) GetComponent<Renderer>().material.color = Color.grey;
        if (health <= maxHealth * (1 / 4)) GetComponent<Renderer>().material.color = Color.red;
    }

    protected IEnumerator DropMovement(GameObject cubedrop)
    {
        Vector3 startPoint = cubedrop.transform.position;
        Vector3 endPoint = cubedrop.transform.position;
        Vector3 currentTarget = endPoint;
        startPoint = new Vector3(startPoint.x, startPoint.y + offsetAnimation, startPoint.z);
        endPoint = new Vector3(startPoint.x, startPoint.y - offsetAnimation, startPoint.z);
        currentTarget = endPoint;
        while (true)
        {
            cubedrop.transform.position = Vector3.MoveTowards(cubedrop.transform.position, currentTarget, dropUpwardsSpeed * Time.deltaTime);

            if (cubedrop.transform.position == endPoint)
            {
                currentTarget = startPoint;
            }
            else if (cubedrop.transform.position == startPoint)
            {
                currentTarget = endPoint;
            }
      
            cubedrop.transform.Rotate(Vector3.up * dropRotationSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    protected void DropAndDestroy()
    {
        HideBlock();
        SpawnDrop();
    }
    protected void HideBlock()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    protected virtual void SpawnDrop()
    {
        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;
        float posZ = gameObject.transform.position.z;
        cubedrop = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube),
                                new Vector3(posX, posY, posZ), Quaternion.identity);
        transform.SetParent(cubedrop.transform);
        cubedrop.transform.localScale = scaleChangeNewCube;
        cubedrop.GetComponent<BoxCollider>().isTrigger = true;
        DropAnimationCoRoutine = StartCoroutine((DropMovement(cubedrop)));
    }
}
