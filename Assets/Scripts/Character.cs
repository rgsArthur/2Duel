using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public string[] possibleNames = { "Вася", "Петя", "ubiitsa_2008", "Олег Иванович", "some_name", "213214", "gdklgmrlkgmalaerfa" };
    public int maxHealth = 100; 
    public int baseDamage = 15;
    public float attackInterval = 2f;

    public Text nameText;
    public Slider healthSlider;
    public Text statusText;

    public int currentHealth;
    public float attackTimer;
    public int currentDamage;


    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;
    }

    protected virtual void Update()
    {
        attackTimer = Mathf.Max(0, attackTimer - Time.deltaTime);
    }

    public virtual void Attack(Character target)
    {
        if (attackTimer > 0 || currentHealth <= 0) return;

        target.TakeDamage(currentDamage);
        attackTimer = attackInterval;
        target.statusText.text = Convert.ToString(currentDamage);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
    public virtual void ResetCharacter()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;
        attackTimer = 0;
        if (statusText != null) statusText.text = "";
        if (healthSlider != null) healthSlider.value = maxHealth;
        UpdateHealthUI();
    }
}