using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetMaxTime(float time)
    {
        Debug.Log(time + " passed to max score");
        _slider.maxValue = time;
        _slider.value = _slider.maxValue;
    }

    public void SetTime(float time)
    {
        _slider.value = time;
    }
}