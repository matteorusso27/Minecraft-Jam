using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public Camera cam;
    int x = (Screen.width / 2);
    int y = (Screen.height / 2);
    float maxDistanceHit = 5f;
    float minDistanceHit = 1.5f;

    [SerializeField] GameObject cube;
    enum Axis
    {
        X,
        Y,
        Z
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //tasto destro del mouse per creare
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            castRay(out hit);
            CreateBlock(ref hit);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            castRay(out hit);
            DamageBlock(ref hit);

        }
    }

    
    private void castRay(out RaycastHit hit)
    {
        Ray lastRay = cam.ScreenPointToRay(new Vector3(x, y));
        if (Physics.Raycast(lastRay, out hit, maxDistanceHit))
        {
            Vector3 hitPoint = hit.point;
            Debug.Log("Posizione hit: " + hitPoint);
            Debug.Log("Normale: " + hit.normal);
        }   
    }

    private void CreateBlock(ref RaycastHit hit)
    {
        //Possibility to create blocks at certain distance
        if (hit.distance > minDistanceHit)
        {
            float posX = hit.collider.gameObject.transform.position.x;
            float posY = hit.collider.gameObject.transform.position.y;
            float posZ = hit.collider.gameObject.transform.position.z;

            if (hit.normal.x != 0)
            {
                Instantiate(cube, new Vector3(hit.normal.x + posX, posY, posZ), Quaternion.identity);
            }
            else if (hit.normal.y != 0)
            {
                Instantiate(cube, new Vector3(posX, posY + hit.normal.y, posZ), Quaternion.identity);
            }
            else
            {
                Instantiate(cube, new Vector3(posX, posY, posZ + hit.normal.z), Quaternion.identity);
            }
        }
    }

    private void DamageBlock(ref RaycastHit hit)
    {
        Destroy(hit.collider.gameObject);
    }
}
