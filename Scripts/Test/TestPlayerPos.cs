using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerPos : MonoBehaviour
{
    private GameObject car;

    private void Start()
    {
        car = GameObject.Find("Car");


    }

    private void Update()
    {
        transform.position = car.transform.position;
    }
}
