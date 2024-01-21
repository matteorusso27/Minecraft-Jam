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
    [SerializeField] public GameObject PnlCrafting;
    [SerializeField] private CraftingHandler CraftingHandler;
    [SerializeField] private Raycaster Raycaster;

    void Update()
    {
        UpdateUI();
        UpdatePauseState();
        UpdateRaycasting();
    }

    private void UpdateUI()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instructions.SetActive(!Instructions.activeInHierarchy);
        }
    }

    private void UpdatePauseState()
    {
        if (Input.GetKeyDown(KeyCode.Q) && CanOpenUI)
        {
            UpdateGamePause();

            //Fill the left slots with the inventory items
            CraftingHandler.FillLeftSlotsWithInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && IsPaused && CanCloseUI)
        {
            UpdateGamePause();
            CraftingHandler.DestroyAndCreate(out PnlCrafting);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CraftingHandler.CheckRecipe())
                StartCoroutine(CloseUI());
        }
    }

    private void UpdateRaycasting()
    {
        bool isLeftClick = Input.GetMouseButtonDown(0);
        bool isRightClick = Input.GetMouseButton(1);

        if (!IsPaused)
        {
            if (isLeftClick)
            {
                Raycaster.CheckHit();
            }
            else if (isRightClick)
            {
                Raycaster.CheckDamage();
            }
            Raycaster.armAnimator.SetBool("isDestroying", isRightClick);
        }
        Raycaster.pike.SetActive(Raycaster.lowBarScript.IsHighlighted("Pike"));
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
        PnlCrafting.SetActive(!PnlCrafting.activeInHierarchy);

        currentState = currentState == GameState.Play ? GameState.Pause : GameState.Play;
        FindGameObjectWithTag(Tags.Player).GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!IsPaused);
    }

    private IEnumerator CloseUI()
    {
        currentState = GameState.ClosingUI;
        PnlCrafting.GetComponent<Image>().raycastTarget = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(2f);

        UpdateGamePause();
        GameObject pnlReference;
        CraftingHandler.DestroyAndCreate(out pnlReference);
        PnlCrafting = pnlReference;
    }
}
