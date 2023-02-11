using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 1000;
    public int health = 1000;

    public Texture2D tex;
    //Block drop animation
    private GameObject cubedrop;
    protected Coroutine DropAnimationCoRoutine;
    private float offsetAnimation = 0.2f;
    private static Vector3 scaleChangeNewCube = new Vector3(0.2f, 0.2f, 0.2f);
    private float dropUpwardsSpeed = 0.25f;
    private float dropRotationSpeed = 15f;
    public void GetDamage()
    {
        Color alpha = GetComponent<Renderer>().material.color;
        health--;
        if (health == 0) DropAndDestroy();
        if (health == maxHealth * (3 / 4)) GetComponent<Renderer>().material.color = Color.black;
        if (health == maxHealth * (1 / 2)) GetComponent<Renderer>().material.color = Color.white;
        if (health == maxHealth * (1 / 4)) GetComponent<Renderer>().material.color = Color.red;
    }

    public virtual IEnumerator DropMovement(GameObject cubedrop)
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

    private void DropAndDestroy()
    {
        HideBlock();
        float posX = gameObject.transform.position.x;
        float posY = gameObject.transform.position.y;
        float posZ = gameObject.transform.position.z;
        cubedrop = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), 
                                new Vector3(posX, posY, posZ), Quaternion.identity);
        cubedrop.transform.localScale = scaleChangeNewCube;
        DropAnimationCoRoutine = StartCoroutine((DropMovement(cubedrop)));
    }
    private void HideBlock()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        Destroy(cubedrop);
    }
}
