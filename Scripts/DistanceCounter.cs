using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    private GameObject player;
    private Vector3 posStart, posEnd;
    private float distance;
    private int distanceCounter;


    private TextMeshProUGUI mText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        posStart = player.transform.position;
        posEnd = player.transform.position;
        mText = GameObject.Find("Distance_tmp").GetComponent<TextMeshProUGUI>();
        distance = 0;
        distanceCounter = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        posEnd = player.transform.position;
        distance = Vector3.Distance(posStart, posEnd);
        
        if (distance >= 5f)
        {
            posStart = player.transform.position;
            distanceCounter += 1;
            mText.text = distanceCounter.ToString();
        }
        
    }

    
}
