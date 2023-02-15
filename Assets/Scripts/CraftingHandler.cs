using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CraftingHandler : MonoBehaviour
{
    public GameObject[] left_slots;
    public GameObject[] right_slots;
    public GameObject result_slot;
    private int dimension = 3;
    private Inventory inventory;

    private GameObject pnlCrafting;
    [SerializeField] private GameObject pnlCraftingReference;
    [SerializeField] private GameObject pnlCraftingPrefab;
    private Text helpText;
    private bool isPaused;
    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;
    [SerializeField] private Texture2D uiTexture;
    [SerializeField] private Texture2D pikeTexture;

    private bool isGrassActive;
    private bool isWoodActive;
    private bool isCobbleStoneActive;

    private static List<string> recipe1 = new List<string>{ "CobbleStoneDrop","WoodDrop","GrassDrop"};
    private static List<string> recipe2 = new List<string>{ "CobbleStoneDrop","WoodDrop","GrassDrop"};

    void Start()
    {
        SetInitialState();
    }

    private void SetInitialState()
    {
        pnlCrafting = GameObject.FindGameObjectWithTag("CraftingPanel");
        if (pnlCrafting == null)
        {
            pnlCraftingReference.SetActive(true);
            pnlCrafting = pnlCraftingReference;
        } 
        //pnlCrafting = pnlCraftingReference;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<CollectorScript>().inventory;
        result_slot = GameObject.FindGameObjectWithTag("ResultCrafting_Slot");
        helpText = GameObject.Find("HelpText").GetComponent<Text>();

        left_slots = new GameObject[dimension + dimension];
        right_slots = new GameObject[dimension];

        for (int i = 0; i < left_slots.Length; i++)
        {
            left_slots[i] = GameObject.FindGameObjectsWithTag("Left_Crafting_Slot")[i];

            //right slots are less
            if (i < 3)
                right_slots[i] = GameObject.FindGameObjectsWithTag("Right_Crafting_Slot")[i];
        }

        isPaused = false;
        isGrassActive = false;
        isWoodActive = false;
        isCobbleStoneActive = false;
        pnlCrafting.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isPaused)
        {
            //Clear slots
            //ClearSlots();

            //Change game pause state
            ChangePauseStatus();

            //Fill the left slots with the inventory items
            FillLeftSlotsWithInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            ChangePauseStatus();
            DestroyAndCreate();
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckRecipe();
            
        }
    }

    //New panel 
    private void DestroyAndCreate()
    {
        Destroy(pnlCrafting);
        GameObject newPnlCrafting = Instantiate(pnlCraftingPrefab);

        newPnlCrafting.transform.SetParent(GameObject.Find("UI").transform);

        newPnlCrafting.GetComponent<RectTransform>().offsetMin = new Vector2(100, 100);
        newPnlCrafting.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -100);

        newPnlCrafting.transform.SetAsLastSibling();
        newPnlCrafting.SetActive(true);
        SetInitialState();
    }

    private void FillLeftSlotsWithInventory()
    {
        for (int i = 0; i < left_slots.Length; i++)
        {
            //Find the first slot empty
            if (isSlotEmpty(left_slots[i]))
            {
                //Find the first non zero quantity element
                foreach (KeyValuePair<string, int> pair in inventory.GetStoredBlocks())
                {
                    if (pair.Value > 0)
                    {
                        RawImage itemImg = left_slots[i].transform.GetChild(0).GetComponent<RawImage>();
                        Texture2D tex = ConvertDropToTexture(pair.Key);
                        //tex is null if the texture is already active in the slot
                        if (tex)
                        {
                            itemImg.texture = tex;
                            Color color = itemImg.color;
                            color.a = 1f;
                            itemImg.color = color;
                            break; // break to pass to the next slot, otherwise it overwrites the same slot
                        }
                    }
                }
            }
        }
    }

    //Convert drop name in the inventory dictionary into the texture name
    // it return the texture to add in the slot or null if the texture is already in the slot
    private Texture2D ConvertDropToTexture(string text)
    {
        switch (text)
        {
            case "CobbleStoneDrop":
                if (!isCobbleStoneActive)
                {
                    isCobbleStoneActive = true;
                    return cobbleStoneTexture;
                }
                return null;
            case "WoodDrop":
                if (!isWoodActive)
                {
                    isWoodActive = true;
                    return woodTexture;
                }
                return null;
            case "GrassDrop":
                if (!isGrassActive)
                {
                    isGrassActive = true;
                    return grassTexture;
                }
                return null;
            default:
                return null;
        }
    }

    private bool isSlotEmpty(GameObject slot)
    {
        if (!slot.transform.GetChild(0).GetComponent<RawImage>().texture) return true;
        return false;
    }


    private void CheckRecipe()
    {
        List<string> input_recipe = new List<string>();
        for (int i = 0; i < right_slots.Length; i++)
        {
            if (right_slots[i].transform.childCount > 0)
            {
                string tex = right_slots[i].transform.GetChild(0).gameObject.GetComponent<RawImage>().texture.name;
                input_recipe.Add(ConvertTextureToDrop(tex));
            }

        }
        //check if the provided materials form a recipe
        if (input_recipe.OrderBy(i => i).SequenceEqual(recipe1.OrderBy(i => i)))
        {
            //create recipe1
            if (result_slot.transform.childCount > 0)
            {
                RawImage rawImgSon = result_slot.transform.GetChild(0).GetComponent<RawImage>();
                if (rawImgSon.texture == null)
                {
                    rawImgSon.texture = pikeTexture;
                    Color sonColor = rawImgSon.color;
                    sonColor.a = 1f;
                    rawImgSon.color = sonColor;
                    helpText.text = "That's how it's done! You got a Pike! The UI will now close to let you play";
                    inventory.IncrementItem("Pike");
                    StartCoroutine(CloseUI());
                    //StartCoroutine(MyCoroutine());
                    
                }
            }

        }
        else
        {
            helpText.text = "Not a correct combination... Try another recipe and press R";
        }

    }

    //Da mettere in una classe statica
    private string ConvertTextureToDrop(string text)
    {
        switch (text)
        {
            case "CobbleStoneBlockIcon":
                return "CobbleStoneDrop";
            case "WoodBlockIcon":
                return "WoodDrop";
            case "GrassBlockIcon":
                return "GrassDrop";
            default:
                return null;
        }
    }
    //Game Pause Management
    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }

    void UpdateGamePause()
    {
        if (isPaused)
        {
            //Stop time
            Time.timeScale = 0;
        }
        else
        {
            //Reactivate time
            Time.timeScale = 1;
        }
        pnlCrafting.SetActive(isPaused);
        GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!isPaused);
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    IEnumerator CloseUI()
    {
        GetComponent<CraftingHandler>().enabled = false;
        Debug.Log("Coroutine started");
        pnlCrafting.GetComponent<Image>().raycastTarget = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(4f);

        ChangePauseStatus();
        Debug.Log("Disabling");
        DestroyAndCreate();
        yield return null;
    }
}
