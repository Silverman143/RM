using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealController : MonoBehaviour
{
    private static bool tutorial = true;
    private DataBase dataBase;


    private void Start()
    {
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
    }

    public void Button()
    {
        dataBase.Update_source("sources", "1", "tutorial");
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
