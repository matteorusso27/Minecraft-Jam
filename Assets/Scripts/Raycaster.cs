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

    private UpdateLowBar lowBarScript;

    [SerializeField] GameObject grassPrefab;
    [SerializeField] GameObject cobbleStonePrefab;
    [SerializeField] GameObject woodPrefab;

    private Animator armAnimator;
    public bool isGamePaused;

    private bool isPikeActive;

    private void Awake()
    {
        UpdateLowBar script = GameObject.FindGameObjectWithTag("LowBar").GetComponent<UpdateLowBar>();
        armAnimator = GameObject.Find("Arm").GetComponent<Animator>();
        if (script) lowBarScript = script;
        else
        {
            Debug.Log("Not found reference");
        }
    }
    private void Update()
    {
        bool isLeftClick = Input.GetMouseButtonDown(0);
        bool isRightClick = Input.GetMouseButton(1);
        
        isGamePaused = GameObject.FindGameObjectWithTag("CraftManager").GetComponent<CraftingHandler>().isGamePaused();
        isPikeActive = GameObject.FindGameObjectWithTag("LowBar").GetComponent<UpdateLowBar>().isPikeActive();
        //tasto destro del mouse per creare
        if (!isGamePaused)
        {
            if (isLeftClick)
            {
                CreateBlock();

            }
            else if (isRightClick)
            {
                DamageBlock();

            }
            else
            {
                armAnimator.SetBool("isDestroying", false);
            }
        }
        
    }

    
    private void castRay(out RaycastHit hit)
    {
        Ray lastRay = cam.ScreenPointToRay(new Vector3(x, y));
        if (Physics.Raycast(lastRay, out hit, maxDistanceHit))
        {
            Vector3 hitPoint = hit.point;
        }   
    }

    private void CreateBlock()
    {
        RaycastHit hit;
        castRay(out hit);
        //Possibility to create blocks at certain distance
        if (hit.distance > minDistanceHit && hit.collider.gameObject.CompareTag("Block"))
        {
            float posX = hit.collider.gameObject.transform.position.x;
            float posY = hit.collider.gameObject.transform.position.y;
            float posZ = hit.collider.gameObject.transform.position.z;

            GameObject prefab;
            prefabToBuild(out prefab);
            if (prefab && prefab.tag == "Block") // I don't want to build if I handle the pike
            {
                if (hit.normal.x != 0)
                {
                    Instantiate(prefab, new Vector3(hit.normal.x + posX, posY, posZ), Quaternion.identity);
                }
                else if (hit.normal.y != 0)
                {
                    Instantiate(prefab, new Vector3(posX, posY + hit.normal.y, posZ), Quaternion.identity);
                }
                else
                {
                    Instantiate(prefab, new Vector3(posX, posY, posZ + hit.normal.z), Quaternion.identity);
                }
            }
            else
            {
                Debug.Log("Prefab not instantiated");
            }
            
        }
    }

    private void DamageBlock()
    {
        RaycastHit hit;
        castRay(out hit);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Block"))
        {
            GameObject go = hit.collider.gameObject;
            go.GetComponent<BlockScript>().TakeDamage(isPikeActive);
            armAnimator.SetBool("isDestroying", true);
            Debug.Log("health: " + go.GetComponent<BlockScript>().currentHealth);
        }
    }
    private void prefabToBuild(out GameObject gObject)
    {
        string activeTex = lowBarScript.FindHighlightBlockToBuild();
        switch (activeTex)
        {
            case "CobbleStoneBlockIcon":
                gObject = cobbleStonePrefab;
                return;
            case "WoodBlockIcon":
                gObject = woodPrefab;
                return;
            case "GrassBlockIcon":
                gObject = grassPrefab;
                return;
            default:
                gObject = null;
                break;
        }
    }
}
