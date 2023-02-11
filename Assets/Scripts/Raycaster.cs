using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public Camera cam;
    int x = (Screen.width / 2);
    int y = (Screen.height / 2);
    [SerializeField]
    float maxDistanceHit = 5f;
    [SerializeField]
    float minDistanceHit = 1f;

    [SerializeField] GameObject cube;
    
    private void Update()
    {
        bool isLeftClick = Input.GetMouseButtonDown(0);
        bool isRightClick = Input.GetMouseButton(1);
        //tasto destro del mouse per creare
        if (isLeftClick) 
        {
            CreateBlock();
        }
        else if (isRightClick)
        {
            DamageBlock();

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

    private void CreateBlock()
    {
        RaycastHit hit;
        castRay(out hit);
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

    private void DamageBlock()
    {
        RaycastHit hit;
        castRay(out hit);
        GameObject go = hit.collider.gameObject;

        go.GetComponent<BlockScript>().GetDamage();
        Debug.Log("health: "+ go.GetComponent<BlockScript>().health);
    }
}
