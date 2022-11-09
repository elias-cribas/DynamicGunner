using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsScript : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public int points;

    void Start()
    {
        
    }

    void Update()
    {
        CoinText.text = "PONTOS : " + points;
    }

}
