using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCraftingPanel : MonoBehaviour
{

    bool isPaused;
    [SerializeField] private GameObject pnlCrafting;

    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //attivare o disattivare la pausa
            ChangePauseStatus();
        }
    }

    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }

    void UpdateGamePause()
    {
        if (isPaused)
        {
            //ferma il tempo
            Time.timeScale = 0;
        }
        else
        {
            //riavvia il tempo
            Time.timeScale = 1;
        }
        pnlCrafting.SetActive(isPaused);
        GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!isPaused);
    }
}

