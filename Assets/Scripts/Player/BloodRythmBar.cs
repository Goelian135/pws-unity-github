using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodRythmBar : MonoBehaviour
{
    public Slider bloodRythmSlider;

    public void SetMaxBloodRythm(int bloodRythmAmount)
    {
        bloodRythmSlider.maxValue = bloodRythmAmount;
    }

    public void SetBloodRythm(float bloodRythmAmount)
    {
        bloodRythmSlider.value = bloodRythmAmount;
    }
    
}
