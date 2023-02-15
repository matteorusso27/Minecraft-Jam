using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    protected float maxHealth;
    public float currentHealth; //levare il public, è solo per debug

    private int handDamage = 100; //debug value, set this to 12f
    private int pikeDamage = 30;
    //Block drop variables
    protected GameObject cubedrop;
    protected string dropTag = "Drop"; 
    protected Coroutine DropAnimationCoRoutine;
    protected float offsetAnimation = 0.2f;
    protected static Vector3 scaleChangeNewCube = new Vector3(0.2f, 0.2f, 0.2f);
    protected float dropUpwardsSpeed = 0.25f;
    protected float dropRotationSpeed = 15f;

    protected Material[] materials;
    [SerializeField] protected Material basicMaterial;

    public void TakeDamage(bool isPikeActive)
    {
        if (isPikeActive)
        {
            currentHealth -= pikeDamage * Time.deltaTime; //damage per second
        }
        else
        {
            currentHealth -= handDamage * Time.deltaTime; //damage per second
        }
        if (currentHealth < 0) DropAndDestroy();
        ChangeMaterial();
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
        cubedrop.GetComponent<Renderer>().material = basicMaterial;
        DropAnimationCoRoutine = StartCoroutine((DropMovement(cubedrop)));
    }
    
    protected void ChangeMaterial()
    {
        float healthLevel = currentHealth / maxHealth;
        if (healthLevel < 0.7f) GetComponent<Renderer>().material = materials[0];
        if (healthLevel < 0.4f) GetComponent<Renderer>().material = materials[1];
        if (healthLevel < 0.25f) GetComponent<Renderer>().material = materials[2];
    }
    

}
