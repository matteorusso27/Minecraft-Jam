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


    bool isPaused;
    private GameObject pnlCrafting;

    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;

    private bool isGrassActive;
    private bool isWoodActive;
    private bool isCobbleStoneActive;

    private static List<string> recipe1 = new List<string>{ "CobbleStoneDrop","WoodDrop","GrassDrop"};

    void Start()
    {
        pnlCrafting = GameObject.Find("CraftingPanel");
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<CollectorScript>().inventory;
        result_slot = GameObject.FindGameObjectWithTag("ResultCrafting_Slot");


        left_slots = new GameObject[dimension + dimension];
        right_slots = new GameObject[dimension];

        for (int i = 0; i < left_slots.Length; i++)
        {
            left_slots[i] = GameObject.FindGameObjectsWithTag("Left_Crafting_Slot")[i];

            //right slots are less
            if (i < 3)
                right_slots[i] = GameObject.FindGameObjectsWithTag("Left_Crafting_Slot")[i];
        }

        isPaused = false;
        pnlCrafting.SetActive(isPaused);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Clear slots
            ClearSlots();

            //Change game pause state
            ChangePauseStatus();

            //Fill the left slots with the inventory items
            FillLeftSlotsWithInventory();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckRecipe();
        }
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

    private void ClearSlots()
    {
        for (int i = 0; i < left_slots.Length; i++)
        {
            left_slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
            Color color = left_slots[i].transform.GetChild(0).GetComponent<RawImage>().color;
            color.a = 0f;
            left_slots[i].transform.GetChild(0).GetComponent<RawImage>().color = color;

            if(i < 3)
            {
                right_slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
                color = left_slots[i].transform.GetChild(0).GetComponent<RawImage>().color;
                color.a = 0f;
                right_slots[i].transform.GetChild(0).GetComponent<RawImage>().color = color;
            }

            isWoodActive = false;
            isGrassActive = false;
            isCobbleStoneActive = false;
        }
    }

    private bool CheckRecipe()
    {
        List<string> input_recipe = new List<string>();
        for(int i = 0; i < right_slots.Length; i++)
        {
            string tex = right_slots[i].transform.GetChild(0).GetComponent<RawImage>().texture.name;
            input_recipe.Add(ConvertTextureToDrop(tex));
        }
        //check if the two lists are equal
        return input_recipe.OrderBy(i => i).SequenceEqual(recipe1.OrderBy(i => i));
       
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
    
 
}