using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLowBar : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject[] highlights;
    [SerializeField] GameObject[] texts;
    private int dimension = 5;
    Dictionary<string, int> storedBlocks;
    private int currentHighlightIndex = 0;

    //Textures
    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;
    [SerializeField] private Texture2D blankTexture;

    private bool isGrassTexture;
    private bool isCobbleStoneTexture;
    private bool isWoodTexture;

    private void Start()
    {
        storedBlocks = GameObject.FindGameObjectWithTag("Player").GetComponent<CollectorScript>().storedBlocks;
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

    private bool isTextureBlank(GameObject gobject)
    {
        if (gobject.GetComponent<RawImage>().texture.name == "Blank") return true;
        return false;
    }

    private void UpdateIcons()
    {
        foreach (KeyValuePair<string, int> item in storedBlocks)
        {
            //Add icons
            if(item.Value != 0)
            {
                switch (item.Key)
                {
                    case "CobbleStoneDrop":

                        if (!isCobbleStoneTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = cobbleStoneTexture;
                            isCobbleStoneTexture = true;
                        }
                        break;
                    case "WoodDrop":

                        if (!isWoodTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = woodTexture;
                            isWoodTexture = true;
                        }
                        break;
                    case "GrassDrop":

                        if (!isGrassTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = grassTexture;
                            isGrassTexture = true;
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
                    case "CobbleStoneDrop":
                        if (isCobbleStoneTexture)
                        {
                            slots[FindSlot("CobbleStoneBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isCobbleStoneTexture = !isCobbleStoneTexture;
                        }
                            
                        break;
                    case "WoodDrop":
                        if (isWoodTexture)
                        {
                            slots[FindSlot("WoodBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isWoodTexture = !isWoodTexture;
                        }
                            
                        break;
                    case "GrassDrop":
                        if (isGrassTexture)
                        {
                            slots[FindSlot("GrassBlockIcon")].GetComponent<RawImage>().texture = blankTexture;
                            isGrassTexture = !isGrassTexture;
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
    private int FindSlot(string textureName)
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

    public string FindHighlightBlock()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentHighlightIndex)
            {
                string textName = slots[i].GetComponent<RawImage>().texture.name;
                if (FindMaterialQuantity(textName) > 0)
                    return textName;
            }
        }
        return null;
    }

    private int FindMaterialQuantity(string texName)
    {
        switch (texName)
        {
            //update quantities when you build
            case "CobbleStoneBlockIcon":
                return storedBlocks["CobbleStoneDrop"] > 0 ? storedBlocks["CobbleStoneDrop"]-- : 0;
            case "WoodBlockIcon":
                return storedBlocks["WoodDrop"] > 0 ? storedBlocks["WoodDrop"]-- : 0;
            case "GrassBlockIcon":
                return storedBlocks["GrassDrop"] > 0 ? storedBlocks["GrassDrop"]-- : 0;
            default:
                return -1;
        }
    }

    private void UpdateText()
    {
        foreach (KeyValuePair<string, int> item in storedBlocks)
        {
            int textToUpdate = item.Value;
                switch (item.Key)
                {
                    case "CobbleStoneDrop":
                        
                        int slotIndex = FindSlot("CobbleStoneBlockIcon");
                        if (slotIndex != -1)
                        {
                             texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }
                    break;

                    case "WoodDrop":

                        slotIndex = FindSlot("WoodBlockIcon");
                        if (slotIndex != -1)
                        {
                            texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
                        }

                    break;
                    case "GrassDrop":

                        slotIndex = FindSlot("GrassBlockIcon");
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
            if (isTextureBlank(slots[i]))
                texts[i].GetComponent<Text>().text = string.Empty;
        }
    }
}