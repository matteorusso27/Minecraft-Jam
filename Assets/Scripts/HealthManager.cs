using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager: MonoBehaviour
{
    private float currentHealth;
    private float maxHealth = 50f;
    [SerializeField] Slider slider;
    [SerializeField] GameObject pnlDeath;

    private float currentHealthPercentage
    {
        get
        {
            return (currentHealth / maxHealth);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        slider.fillRect.GetComponent<Image>().color = Color.green;
    }
    void Update()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        slider.value = currentHealthPercentage;
        if(currentHealthPercentage < 0.5f) slider.fillRect.GetComponent<Image>().color = Color.yellow;
        if(currentHealthPercentage < 0.2f) slider.fillRect.GetComponent<Image>().color = Color.red;
        if (currentHealthPercentage <= 0f) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(false);
            GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
            pnlDeath.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
