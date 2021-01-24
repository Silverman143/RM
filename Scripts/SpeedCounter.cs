using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedCounter : MonoBehaviour
{
    private CarController carController;
    private TextMeshProUGUI speedTMP;
    private float updateTime;

    private void Start()
    {
        

        
        speedTMP = GameObject.Find("SpeedCounter").GetComponent<TextMeshProUGUI>();
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        updateTime = 1f;
    }

    private void Update()
    {
        updateTime -= 0.1f;
        if (updateTime<0)
        {
            string _text;
            float currentSpeed;


            currentSpeed = carController.Current_speed();

            _text = string.Format("{0:0.0}", currentSpeed) + "s";

            speedTMP.text = _text;
            updateTime = 1f;
        }
    }
}
