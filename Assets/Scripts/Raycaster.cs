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
    [SerializeField] GameObject coalPrefab;

    private Animator armAnimator;
    public bool isGamePaused;

    private bool isPikeActive;

    private float damagePerSecond = 10;
    private float handDamage = 80f;
    private float pikeDamage = 130f;
    private void Awake()
    {
        UpdateLowBar script = GameObject.FindGameObjectWithTag("LowBar").GetComponent<UpdateLowBar>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!isGamePaused);
        armAnimator = GameObject.Find("Arm").GetComponent<Animator>();
        if (script) lowBarScript = script;
        else
        {
            Debug.LogError("Not found reference");
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
                CheckHit();

            }
            else if (isRightClick)
            {
                CheckDamage();
            }
            else
            {
                armAnimator.SetBool("isDestroying", false);
            }
        }
            
        if (isPikeActive)
        {
            damagePerSecond = pikeDamage;
            
        }
        else
        {
            damagePerSecond = handDamage;
        }

        //GameObject.Find("Pike").SetActive(isPikeActive);
    }

    
    private void CastRay(out RaycastHit hit)
    {
        Ray lastRay = cam.ScreenPointToRay(new Vector3(x, y));
        if (Physics.Raycast(lastRay, out hit, maxDistanceHit))
        {
            Vector3 hitPoint = hit.point;
        }   
    }

    private void CheckHit()
    {
        RaycastHit hit;
        CastRay(out hit);
        //Possibility to create blocks at certain distance

        if (hit.distance > minDistanceHit && hit.collider.gameObject.CompareTag("Block"))
        {
            CreateBlock(ref hit);
        }
    }

    private void CreateBlock(ref RaycastHit hit)
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
            Debug.LogError("Prefab not instantiated");
        }
    }
    
    private void DamageEnemy(GameObject enemyObj)
    {
        if(isPikeActive)
            enemyObj.GetComponent<EnemyScript>().TakeDamage(damagePerSecond);
    }
    
    private void CheckDamage()
    {
        RaycastHit hit;
        CastRay(out hit);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Block"))
            {
                GameObject go = hit.collider.gameObject;
                go.GetComponent<BlockScript>().TakeDamage(damagePerSecond);
            }
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                DamageEnemy(hit.collider.gameObject);
            }
            armAnimator.SetBool("isDestroying", true);
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
            case "CoalBlockIcon":
                gObject = coalPrefab;
                return;
            default:
                gObject = null;
                break;
        }
    }
}
