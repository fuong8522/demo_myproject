using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHeal(int healmax)
    {
        slider.maxValue= healmax;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHeal(int heal)
    {
        slider.value= heal; 
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
