using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    public Slider _slider;
    public TextMeshProUGUI _sliderText;

    void Start()
    {
        _slider.onValueChanged.AddListener((ev) => {
            _sliderText.text = ev.ToString("0.00");
        });
    }

    
    void Update()
    {
        
    }
}
