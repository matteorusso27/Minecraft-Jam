using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class HealthManager: MonoBehaviour
{
    private float currentHealth;
    private float maxHealth = 50f;
    [SerializeField] Slider slider;
    [SerializeField] GameObject pnlDeath;
    private float timeToHeal = 7f;

    IEnumerator HealCoroutine = null;
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
        if(currentHealthPercentage >= 1f) slider.fillRect.GetComponent<Image>().color = Color.green;
        if(currentHealthPercentage < 0.5f) slider.fillRect.GetComponent<Image>().color = Color.yellow;
        if(currentHealthPercentage < 0.2f) slider.fillRect.GetComponent<Image>().color = Color.red;
        if (currentHealthPercentage <= 0f) {
            FindGameObjectWithTag(Tags.Player).GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(false);
            GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
            pnlDeath.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (HealCoroutine == null)
        {
            HealCoroutine = IncrementHealth();
            StartCoroutine(HealCoroutine);
        }
    }

    IEnumerator IncrementHealth()
    {
        while (true)
        {
            currentHealth += 10f;
            if(currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
                StopCoroutine(HealCoroutine);
                HealCoroutine = null;
            }
            yield return new WaitForSeconds(timeToHeal);
        }
    }
}
