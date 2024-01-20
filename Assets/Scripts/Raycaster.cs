using UnityEngine;
using static Utils;

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
    [SerializeField] private CraftingHandler craftingHandler;

    [SerializeField] GameObject grassPrefab;
    [SerializeField] GameObject cobbleStonePrefab;
    [SerializeField] GameObject woodPrefab;
    [SerializeField] GameObject coalPrefab;

    [SerializeField] GameObject pike;

    private Animator armAnimator;
    public bool isGamePaused => craftingHandler.isGamePaused();
    private bool isPikeActive => pike.activeInHierarchy;

    private float damagePerSecond = 10;
    private float handDamage = 80f;
    private float pikeDamage = 130f;
    private void Start()
    {
        lowBarScript = GameObject.FindGameObjectWithTag(Tags.LowBar.ToString()).GetComponent<UpdateLowBar>();
        GameObject.FindGameObjectWithTag(Tags.Player.ToString()).GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!isGamePaused);
        armAnimator = GameObject.Find("Arm").GetComponent<Animator>();
    }

    private void Update()
    {
        bool isLeftClick = Input.GetMouseButtonDown(0);
        bool isRightClick = Input.GetMouseButton(1);
        
        //Right click to create
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

        pike.SetActive(lowBarScript.IsHighlighted("pike"));
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

        if (hit.distance > minDistanceHit && hit.collider.gameObject.CompareTag(Tags.Block.ToString()))
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
        if (prefab && prefab.tag.Equals(Tags.Block.ToString())) // I don't want to build if I handle the pike
        {
            if (hit.normal.x != 0)
            {
                posX += hit.normal.x;
            }
            else if (hit.normal.y != 0)
            {
                posY += hit.normal.y;
            }
            else
            {
                posZ += hit.normal.z;
            }

            Instantiate(prefab, new Vector3(posX, posY, posZ), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab not instantiated");
        }
    }
    
    private void DamageEnemy(GameObject enemyObj)
    {
        if(isPikeActive) enemyObj.GetComponent<EnemyScript>().TakeDamage(damagePerSecond);
    }
    
    private void CheckDamage()
    {
        RaycastHit hit;
        CastRay(out hit);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag(Tags.Block.ToString()))
            {
                GameObject go = hit.collider.gameObject;
                go.GetComponent<BlockScript>().TakeDamage(damagePerSecond);
            }
            else if (hit.collider.gameObject.CompareTag(Tags.Enemy.ToString()))
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
