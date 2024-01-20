using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static Utils;

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
    [SerializeField] private Texture2D coalTexture;
    [SerializeField] private Texture2D uiTexture;
    [SerializeField] private Texture2D pikeTexture;

    private bool isGrassActive;
    private bool isWoodActive;
    private bool isCobbleStoneActive;
    private bool isCoalActive;

    private static List<InventoryItem> recipe1 = new List<InventoryItem> { InventoryItem.CobbleStone, InventoryItem.Wood, InventoryItem.Coal};
    private static int[] quantities1 = new int[] { 5,5,5 };
    private static List<InventoryItem> recipe2 = new List<InventoryItem> { InventoryItem.CobbleStone, InventoryItem.Wood, InventoryItem.Grass};

    void Start()
    {
        SetInitialState();
    }

    private void SetInitialState()
    {
        pnlCrafting = FindGameObjectWithTag(Tags.CraftingPanel);
        if (pnlCrafting == null)
        {
            pnlCraftingReference.SetActive(true);
            pnlCrafting = pnlCraftingReference;
        } 
        inventory = FindGameObjectWithTag(Tags.Player).GetComponent<CollectorScript>().inventory;
        result_slot = FindGameObjectWithTag(Tags.ResultCrafting_Slot);
        helpText = GameObject.Find("HelpText").GetComponent<Text>();

        left_slots = new GameObject[dimension + dimension];
        right_slots = new GameObject[dimension];

        for (int i = 0; i < left_slots.Length; i++)
        {
            left_slots[i] = FindGameObjectsWithTag(Tags.Left_Crafting_Slot)[i];

            //right slots are less
            if (i < 3)
                right_slots[i] = FindGameObjectsWithTag(Tags.Right_Crafting_Slot)[i];
        }

        isGrassActive = false;
        isWoodActive = false;
        isCobbleStoneActive = false;
        isCoalActive = false;
        pnlCrafting.SetActive(false);
    }

    //New panel 
    public void DestroyAndCreate(out GameObject newPnlReference)
    {
        Destroy(pnlCrafting);
        GameObject newPnlCrafting = Instantiate(pnlCraftingPrefab);

        newPnlCrafting.transform.SetParent(GameObject.Find("UI").transform);

        newPnlCrafting.GetComponent<RectTransform>().offsetMin = new Vector2(100, 100);
        newPnlCrafting.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -100);
        newPnlCrafting.gameObject.tag = Tags.CraftingPanel.ToString();
        newPnlCrafting.transform.SetAsLastSibling();
        newPnlCrafting.SetActive(true);
        pnlCraftingReference = newPnlCrafting;
        newPnlReference = newPnlCrafting;
        SetInitialState();
    }

    public void FillLeftSlotsWithInventory()
    {
        for (int i = 0; i < left_slots.Length; i++)
        {
            //Find the first slot empty
            if (isSlotEmpty(left_slots[i]))
            {
                //Find the first non zero quantity element
                foreach (KeyValuePair<InventoryItem, int> pair in inventory.GetStoredBlocks())
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
                            itemImg.GetComponentInChildren<Text>().text = pair.Value.ToString();
                            break; // break to pass to the next slot, otherwise it overwrites the same slot
                        }
                    }
                }
            }
        }
    }

    //Convert drop name in the inventory dictionary into the texture name
    // it return the texture to add in the slot or null if the texture is already in the slot
    public Texture2D ConvertDropToTexture(InventoryItem text)
    {
        switch (text)
        {
            case InventoryItem.CobbleStone:
                if (!isCobbleStoneActive)
                {
                    isCobbleStoneActive = true;
                    return cobbleStoneTexture;
                }
                return null;
            case InventoryItem.Wood:
                if (!isWoodActive)
                {
                    isWoodActive = true;
                    return woodTexture;
                }
                return null;
            case InventoryItem.Grass:
                if (!isGrassActive)
                {
                    isGrassActive = true;
                    return grassTexture;
                }
                return null;
            case InventoryItem.Coal:
                if (!isCoalActive)
                {
                    isCoalActive = true;
                    return coalTexture;
                }
                return null;
            default:
                return null;
        }
    }

    private bool isSlotEmpty(GameObject slot) => !slot.transform.GetChild(0).GetComponent<RawImage>().texture ? true : false;
    public bool CheckRecipe()
    {
        List<InventoryItem> input_recipe = new List<InventoryItem>();
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
            //Check quantities
            if (AreMaterialsEnough(recipe1, quantities1))
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
                        inventory.IncrementItem(InventoryItem.Pike);
                        return true;
                    }
                }
            }
            else
            {
                helpText.text = "Quantites are not enough to create the recipe";
                return false;
            }
        }
        else
        {
            helpText.text = "Not a correct combination... Try another recipe and press R";
            return false;
        }
        return false;
    }

    private bool AreMaterialsEnough(List<InventoryItem> recipe, int[] quantities)
    {
        for (int i = 0; i < recipe.Count; i++)
        {
            if (inventory.GetQuantity(recipe[i]) >= quantities[i])
                continue;
            else
            {
                return false;
            }
        }

        //Remove quantities because the recipe is correct
        for (int i = 0; i < recipe.Count; i++)
        {
            inventory.DecrementItem(recipe[i],quantities1[i]);
        }
        return true;
        
    }
}
