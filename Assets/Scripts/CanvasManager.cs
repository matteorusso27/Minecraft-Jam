using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject Lowbar;
    public GameObject DeathPanel;
    public GameObject CraftingPanel;
    public GameObject ScreenElements;

    void Start()
    {
        Lowbar.SetActive(true);
        ScreenElements.SetActive(true);
        CraftingPanel.SetActive(false);
        DeathPanel.SetActive(false);
    }
}
