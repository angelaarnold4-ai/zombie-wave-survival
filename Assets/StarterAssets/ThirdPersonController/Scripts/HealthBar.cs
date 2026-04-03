using UnityEngine;
using UnityEngine.UI; // Required for using the Slider component

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void UpdateHealth(float health)
    {
        slider.value = health;
    }
}