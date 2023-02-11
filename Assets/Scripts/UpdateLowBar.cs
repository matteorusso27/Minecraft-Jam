using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLowBar : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject[] highlights;
    [SerializeField] GameObject[] texts;
    Dictionary<string, int> storedBlocks;
    private int currentHighlightIndex = 0;

    //Textures
    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;

    private bool isGrassTexture;
    private bool isCobbleStoneTexture;
    private bool isWoodTexture;

    private void Start()
    {
        storedBlocks = GameObject.FindGameObjectWithTag("Player").GetComponent<CollectorScript>().storedBlocks;
        slots = new GameObject[8];
        highlights = new GameObject[8];
        texts = new GameObject[8];

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
                        else
                        {
                            //update text
                            if (item.Value != 1)
                            {
                                int slotIndex = FindSlot("CobbleStoneBlockIcon");
                                texts[slotIndex].GetComponent<Text>().text = item.Value.ToString();
                            }
                                
                        }
                        break;
                    case "WoodDrop":
                        if (!isWoodTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = woodTexture;
                            isWoodTexture = true;
                        }
                        else
                        {
                            //update text
                            if (item.Value != 1)
                            {
                                int slotIndex = FindSlot("WoodBlockIcon");
                                texts[slotIndex].GetComponent<Text>().text = item.Value.ToString();
                            }
                        }
                        break;
                    case "GrassDrop":
                        if (!isGrassTexture)
                        {
                            slots[FindEmptySlot()].GetComponent<RawImage>().texture = grassTexture;
                            isGrassTexture = true;
                        }
                        else {
                            //update text
                            if (item.Value != 1)
                            {
                                int slotIndex = FindSlot("GrassBlockIcon");
                                texts[slotIndex].GetComponent<Text>().text = item.Value.ToString();
                            }
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
                return slots[i].GetComponent<RawImage>().texture.name;
            }
        }
        return null;
    }
}
