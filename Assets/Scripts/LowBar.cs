using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class LowBar : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject[] highlights;
    [SerializeField] GameObject[] texts;
    private int dimension = 5;
    Inventory inventory;
    private int currentHighlightIndex = 0;

    private GameStateHandler GameStateHandler;
    
    //Textures
    [SerializeField] private Texture2D grassTexture;
    [SerializeField] private Texture2D cobbleStoneTexture;
    [SerializeField] private Texture2D woodTexture;
    [SerializeField] private Texture2D coalTexture;
    [SerializeField] private Texture2D blankTexture;
    [SerializeField] private Texture2D pikeTexture;

    private List<InventoryItem> placedIcons;

    public Texture2D GrassTexture { get => grassTexture;}
    public Texture2D CobbleStoneTexture { get => cobbleStoneTexture;}
    public Texture2D WoodTexture { get => woodTexture;}
    public Texture2D CoalTexture { get => coalTexture;}
    public Texture2D PikeTexture { get => pikeTexture;}
    public Texture2D BlankTexture { get => blankTexture;}

    private void Start()
    {
        GameStateHandler = FindGameObjectWithTag(Tags.GameStateHandler).GetComponent<GameStateHandler>();
        inventory = FindGameObjectWithTag(Tags.Player).GetComponent<CollectorScript>().inventory;
        slots = new GameObject[dimension];
        highlights = new GameObject[dimension];
        texts = new GameObject[dimension];
        placedIcons = new List<InventoryItem>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = FindByName("Slot" + (i+1).ToString());
            highlights[i] = FindByName("Highlight" + (i+1).ToString());
            texts[i] = FindByName("Text" + (i+1).ToString());
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
        if (GameStateHandler.IsPaused) return;

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

    private bool IsTextureBlank(GameObject gobject) => gobject.GetComponent<RawImage>().texture.name == "Blank" ? true : false;

    private void UpdateIcons()
    {
        foreach (KeyValuePair<InventoryItem, int> item in inventory.GetStoredBlocks())
        {
            if (item.Value > 0)
            {
                AddIcon(item.Key);
            }
            else
            {
                RemoveIcon(item.Key);
            }
        }
    }
    public Texture2D ItemToTexture(InventoryItem item)
    {
        switch (item)
        {
            case InventoryItem.Grass:
                return GrassTexture;
            case InventoryItem.Coal:
                return CoalTexture;
            case InventoryItem.CobbleStone:
                return CobbleStoneTexture;
            case InventoryItem.Wood:
                return WoodTexture;
            case InventoryItem.Pike:
                return PikeTexture;
            default:
                return BlankTexture;
        }
    }
    private void AddIcon(InventoryItem item)
    {
        if (!placedIcons.Contains(item))
        {
            int emptySlotIdx = FindEmptySlot();
            if (emptySlotIdx == -1) return;
            slots[emptySlotIdx].GetComponent<RawImage>().texture = ItemToTexture(item);
            placedIcons.Add(item);
        }
    }

    private void RemoveIcon(InventoryItem item)
    {
        if (placedIcons.Contains(item))
        {
            int findSlotIdx = FindSlot(item.ToString() + "Icon");
            if (findSlotIdx == -1) return;
            slots[findSlotIdx].GetComponent<RawImage>().texture = blankTexture;
            placedIcons.Remove(item);
        }
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (IsTextureBlank(slots[i])) return i;
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

    public bool IsHighlighted(InventoryItem item) => IsHighlighted(item.ToString());

    public bool IsHighlighted(string textName)
    {
        var slot = FindSlot(textName);
        return slot == currentHighlightIndex;
    }

    public InventoryItem FindHighlightBlockToBuild()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentHighlightIndex)
            {
                string textName = slots[i].GetComponent<RawImage>().texture.name;
                var item = ConvertTextureToItem(textName);
                if (inventory.GetQuantity(item) > 0)
                {
                    inventory.DecrementItem(item);
                    return item;
                }
            }
        }
        return InventoryItem.None;
    }

    private void UpdateText()
    {
        void ChangeTextForItem(InventoryItem item, string textToUpdate)
        {
            int slotIndex = FindSlot(item.ToString()+"Icon");
            if (slotIndex != -1)
            {
                texts[slotIndex].GetComponent<Text>().text = textToUpdate.ToString();
            }
        }

        foreach (KeyValuePair<InventoryItem, int> item in inventory.GetStoredBlocks())
        {
            ChangeTextForItem(item.Key, item.Value.ToString());
        }

        ClearText();
    }
    private void ClearText()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (IsTextureBlank(slots[i]) || slots[i].GetComponent<RawImage>().texture.name == "PikeIcon")
                texts[i].GetComponent<Text>().text = string.Empty;
        }
    }
}
