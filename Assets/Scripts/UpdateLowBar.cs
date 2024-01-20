using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class UpdateLowBar : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject[] highlights;
    [SerializeField] GameObject[] texts;
    private int dimension = 5;
    Inventory inventory;
    private int currentHighlightIndex = 0;

    //Textures
    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;
    [SerializeField] private Texture2D coalTexture;
    [SerializeField] private Texture2D blankTexture;
    [SerializeField] private Texture2D pikeTexture;

    private bool isGrassTexture;
    private bool isCobbleStoneTexture;
    private bool isWoodTexture;
    private bool isCoalTexture;
    private bool isPikeTexture;

    private void Start()
    {
        inventory = FindGameObjectWithTag(Tags.Player).GetComponent<CollectorScript>().inventory;
        slots = new GameObject[dimension];
        highlights = new GameObject[dimension];
        texts = new GameObject[dimension];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = GameObject.Find("Slot" + (i+1).ToString());
            highlights[i] = GameObject.Find("Highlight" + (i+1).ToString());
            texts[i] = GameObject.Find("Text" + (i+1).ToString());
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeHighlight();
        }
        UpdateIcons();
        UpdateText();
    }

    private void ChangeHighlight()
    {

        if (currentHighlightIndex + 1 >= slots.Length)
        {
            highlights[currentHighlightIndex].GetComponent<RawImage>().enabled = false;
            currentHighlightIndex = 0;
            highlights[currentHighlightIndex].GetComponent<RawImage>().enabled = true;
        }
        else
        {
            highlights[currentHighlightIndex++].GetComponent<RawImage>().enabled = false;
            highlights[currentHighlightIndex].GetComponent<RawImage>().enabled = true;
        }
        
    }

    private bool isTextureBlank(GameObject gobject) => gobject.GetComponent<RawImage>().texture.name == "Blank" ? true : false;

    private void UpdateIcons()
    {
        foreach (KeyValuePair<InventoryItem, int> item in inventory.GetStoredBlocks())
        {
            //Add icons
            if(item.Value != 0)
            {
                switch (item.Key)
                {
                    case InventoryItem.CobbleStone:

                        if (!isCobbleStoneTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = cobbleStoneTexture;
                            isCobbleStoneTexture = true;
                        }
                        break;
                    case InventoryItem.Wood:

                        if (!isWoodTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = woodTexture;
                            isWoodTexture = true;
                        }
                        break;
                    case InventoryItem.Grass:

                        if (!isGrassTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = grassTexture;
                            isGrassTexture = true;
                        }
                        break;
                   case InventoryItem.Coal:

                        if (!isCoalTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = coalTexture;
                            isCoalTexture = true;
                        }
                        break;

                   case InventoryItem.Pike:
                        if (!isPikeTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = pikeTexture;
                            isPikeTexture = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (item.Key)
                {
                    case InventoryItem.CobbleStone:
                        if (isCobbleStoneTexture)
                        {
                            slots[FindSlot("CobbleStoneBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isCobbleStoneTexture = !isCobbleStoneTexture;
                        }
                            
                        break;
                    case InventoryItem.Wood:
                        if (isWoodTexture)
                        {
                            slots[FindSlot("WoodBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isWoodTexture = !isWoodTexture;
                        }
                            
                        break;
                    case InventoryItem.Grass:
                        if (isGrassTexture)
                        {
                            slots[FindSlot("GrassBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isGrassTexture = !isGrassTexture;
                        }
                            
                        break;
                    case InventoryItem.Coal:
                        if (isCoalTexture)
                        {
                            slots[FindSlot("CoalBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isCoalTexture = !isCoalTexture;
                        }
                            
                        break;
                    case InventoryItem.Pike:
                        if (isPikeTexture)
                        {
                            slots[FindSlot("Pike")].GetComponent<RawImage>().texture = blankTexture;
                            isPikeTexture = !isPikeTexture;
                        }

                        break;
                    default:
                        break;
                }
            }
        }
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (isTextureBlank(slots[i])) return i;
        }

        return -1;
    }
    
    //Given the texture name returns the slot index
    public int FindSlot(string textureName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].GetComponent<RawImage>().texture.name == textureName)
            {
                return i;
            }
        }
        return -1;
    }

    public bool IsHighlighted(string textName)
    {
        var slot = FindSlot(textName);
        return slot == currentHighlightIndex;
    }

    public string FindHighlightBlockToBuild()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentHighlightIndex)
            {
                string textName = slots[i].GetComponent<RawImage>().texture.name;
                var dropType = ConvertTextureToDrop(textName);
                if (inventory.GetQuantity(dropType) > 0)
                {
                    inventory.DecrementItem(dropType);
                    return textName;
                }
            }
        }
        return null;
    }

    private InventoryItem ConvertTextureToDrop(string text)
    {
        switch (text)
        {
            case "CobbleStoneBlockIcon":
                return InventoryItem.CobbleStone;
            case "WoodBlockIcon":
                return InventoryItem.Wood;
            case "GrassBlockIcon":
                return InventoryItem.Grass;
            case "CoalBlockIcon":
                return InventoryItem.Coal;
            default:
                return InventoryItem.None;
        }
    }

    private void UpdateText()
    {
        foreach (KeyValuePair<InventoryItem, int> item in inventory.GetStoredBlocks())
        {
            int textToUpdate = item.Value;
                switch (item.Key)
                {
                    case InventoryItem.CobbleStone:
                        
                        int slotIndex = FindSlot("CobbleStoneBlockIcon");
                        if (slotIndex != -1)
                        {
                             texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }
                    break;

                    case InventoryItem.Wood:

                        slotIndex = FindSlot("WoodBlockIcon");
                        if (slotIndex != -1)
                        {
                            texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }

                    break;
                    case InventoryItem.Grass:

                        slotIndex = FindSlot("GrassBlockIcon");
                        if (slotIndex != -1)
                        {
                            texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }
                    break;

                    case InventoryItem.Coal:

                        slotIndex = FindSlot("CoalBlockIcon");
                        if (slotIndex != -1)
                        {
                            texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }
                    break;
                default:
                        break;
                }
        }
        ClearText();
    }
    private void ClearText()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (isTextureBlank(slots[i]) || slots[i].GetComponent<RawImage>().texture.name == "Pike")
                texts[i].GetComponent<Text>().text = string.Empty;
        }
    }
}
