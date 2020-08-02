using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthSlider;


    public void TakeDamage(int amount)
    {
        healthSlider.value -= amount;
    }

    public void HealDamage(int amount)
    {
        healthSlider.value += amount;
    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
}
