using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class GameStateHandler : MonoBehaviour
{
    public enum GameState
    {
        Play,
        Pause,
        ClosingUI
    }

    private static GameState currentState = GameState.Play;
    public bool IsPaused => currentState == GameState.Pause;
    private bool CanOpenUI => currentState == GameState.Play;
    private bool CanCloseUI => currentState != GameState.ClosingUI;

    [SerializeField] private GameObject Instructions;
    [SerializeField] public GameObject pnlCrafting;
    [SerializeField] private CraftingHandler CraftingHandler;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instructions.SetActive(!Instructions.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Q) && CanOpenUI)
        {
            //Change game pause state
            ChangePauseStatus();

            //Fill the left slots with the inventory items
            CraftingHandler.FillLeftSlotsWithInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && IsPaused && CanCloseUI)
        {
            ChangePauseStatus();
            GameObject pnlReference;
            CraftingHandler.DestroyAndCreate(out pnlReference);
            pnlCrafting = pnlReference;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CraftingHandler.CheckRecipe())
                StartCoroutine(CloseUI());
        }
    }

    //Game Pause Management
    public void ChangePauseStatus()
    {
        UpdateGamePause();
    }

    void UpdateGamePause()
    {
        if (currentState == GameState.Play)
        {
            //Stop time
            Time.timeScale = 0;
        }
        else
        {
            //Reactivate time
            Time.timeScale = 1;
        }
        pnlCrafting.SetActive(!pnlCrafting.activeInHierarchy);

        currentState = currentState == GameState.Play ? GameState.Pause : GameState.Play;
        FindGameObjectWithTag(Tags.Player).GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!IsPaused);
    }

    private IEnumerator CloseUI()
    {
        currentState = GameState.ClosingUI;
        pnlCrafting.GetComponent<Image>().raycastTarget = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(2f);

        ChangePauseStatus();
        GameObject pnlReference;
        CraftingHandler.DestroyAndCreate(out pnlReference);
        pnlCrafting = pnlReference;
    }
}
