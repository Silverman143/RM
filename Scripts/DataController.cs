using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    private DataBase dataBase;

    [SerializeField] private TextMeshProUGUI coinsTMP;
    [SerializeField] private TextMeshProUGUI recordTMP;
    [SerializeField] private Transform carHolder;
    [SerializeField] private PlayerGetter playerGetter;
    [SerializeField] public DailyRewardComponent dailyRewComp;

    [Header("Bonus test")]

    [SerializeField] public DailyBonusController dailyBonusController;
    [SerializeField] public GameObject menuButtons;
    [SerializeField] public GameObject fingerTutorial;


    [SerializeField] public GameObject loadingPanel;
    
    private void Start()
    {
        
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
        if (SceneManager.GetActiveScene().name == "Menu" | SceneManager.GetActiveScene().name == "Road")
        {
            playerGetter = GameObject.Find("Player").GetComponent<PlayerGetter>();
        }
        
        dataBase.Create_table();
        dataBase.Create_cars_table();
        coinsTMP = GameObject.Find("Coins_tmp").GetComponent<TextMeshProUGUI>();
        
        string _car = dataBase.Get_data("cars", "carName", "available", "1"); ;


        
        if (_car == "Error")
        {
            dataBase.Create_cars_table();
            dataBase.Add_new_cars("cars", "muscle_car", "1");
        }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            

            StartCoroutine(dailyRewComp.CheckDailyReward( day =>
            {

                if (day == -1)
                {
                    //No internet connection
                    Debug.Log("No internet connection");
                    loadingPanel.SetActive(false);
                    menuButtons.SetActive(true);
                }
                if (day == -2)
                {
                    // Http error
                    Debug.Log("Http Error");
                    loadingPanel.SetActive(false);
                    menuButtons.SetActive(true);
                }
                if (day == 0)
                {
                    Debug.Log("<color=red> Show menu Buttons </color>");
                    loadingPanel.SetActive(false);
                    menuButtons.SetActive(true);
                }
                if (day >= 1)
                {
                    loadingPanel.SetActive(false);
                    dailyBonusController.transform.Find("Panel").gameObject.SetActive(true);

                    if (int.Parse(dataBase.Get_data_day_counter("firstTime")) == 0)
                    {
                        fingerTutorial.SetActive(true);
                        dataBase.Upload_day_counter("firstTime", 1);
                    }


                    dailyBonusController.Show_bonus(day);
                }




            }));

            playerGetter.Show_car_in_menu();
            recordTMP = GameObject.Find("Record_tmp").GetComponent<TextMeshProUGUI>();
            recordTMP.text = "Best: " + dataBase.Get_res_inf("sources", "record");


        }

        if (SceneManager.GetActiveScene().name == "Menu" | SceneManager.GetActiveScene().name == "CarSelection")
        {
            coinsTMP = GameObject.Find("Coins_tmp").GetComponent<TextMeshProUGUI>();
            coinsTMP.text = dataBase.Get_res_inf("sources", "gold");
            
        }

        if (SceneManager.GetActiveScene().name == "CarSelection")
        {
            carHolder = GameObject.Find("Car_holder").transform;

            for (int i = 0; i< carHolder.childCount; i++)
            {
                dataBase.Add_new_cars("cars", carHolder.GetChild(i).name, "0");
                dataBase.Create_characteristic_table(carHolder.GetChild(i).name);
            }
        }

        
        

    }

    public void Add_coins(bool _isBonus)
    {
        int _wasCoins, _addCoins;

        _wasCoins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        _addCoins = int.Parse(coinsTMP.text);
        

        if (!_isBonus)
        {
            _addCoins += _wasCoins;
            
        }
        else if (_isBonus)
        {
            _addCoins *= 2;
            Debug.Log(_addCoins);
            coinsTMP.text = _addCoins.ToString();
            _addCoins += _wasCoins;
        }

        dataBase.Update_source("sources", _addCoins.ToString(), "gold");

    }

    public void Select_car(string _oldCar, string _newCar)
    {
        dataBase.Change_car_availabe(_oldCar, "0");
        dataBase.Change_car_availabe(_newCar, "1");

    }

    public string Get_active_car()
    {
        string _data;
        _data = dataBase.Get_data("cars", "carName", "available", "1");

        return (_data);
    }

    public void Reload_coins()
    {
        coinsTMP.text = dataBase.Get_res_inf("sources", "gold");
    }


}
