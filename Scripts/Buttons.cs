using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private CarController controller;
    private DataBase dataBase;
    private CharacteristicHandler characteristicHandler;
    private CarSelection carSelection;

    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject menuButtons;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject gearBox;
    [SerializeField] public GameObject storePanel;
    [SerializeField] public GameObject turnArrows;

    private DataController dataController;

    
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Road")
        {
            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        }

        if (SceneManager.GetActiveScene().name == "CarSelection")
        {
            dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
            
            carSelection = GameObject.Find("Car_holder").GetComponent<CarSelection>();
        }


        dataController = GameObject.Find("DataController").GetComponent<DataController>();


    }

    public void Play_button()
    {
        SceneManager.LoadScene("Road");
    }

    public void Car_selection_Button()
    {
        SceneManager.LoadScene("CarSelection");
    }

    public void Exit_button()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart_lvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void Reverse()
    {
        if (controller.reverse == true)
        {
            controller.reverse = false;
        }
        else controller.reverse = true;
    }

    public void Upgrate_fuel ()
    {
        int _price, _coins;
        string _carName;
        _carName = carSelection.Active_car_name();
        _coins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
        _price = characteristicHandler.Get_characteristic_price("\'FuelTankSize\'");

        



        if (_coins > _price)
        {
            int _fuelCoeff;


            _fuelCoeff = int.Parse(dataBase.Get_data_int(_carName, "value", "characteristic", "\'FuelTankSize\'"));
           
            _coins -= _price;
            dataBase.Update_source("sources", _coins.ToString(), "gold");
            _fuelCoeff += 1;
            dataBase.Update_characteristics(_carName, _fuelCoeff.ToString(), "FuelTankSize");
            dataController.Reload_coins();
            characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
            characteristicHandler.Reload_characteristics();

        }
        else Debug.Log("ERROR!!!!!!!!");
    }

    public void Upgrade_controllability()
    {
        int _price, _coins;
        string _carName;
        _carName = carSelection.Active_car_name();
        _coins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
        _price = characteristicHandler.Get_characteristic_price("\'controllabilityCoeff\'");





        if (_coins > _price)
        {
            int _controllabilityCoeff;


            _controllabilityCoeff = int.Parse(dataBase.Get_data_int(_carName, "value", "characteristic", "\'controllabilityCoeff\'"));

            _coins -= _price;
            dataBase.Update_source("sources", _coins.ToString(), "gold");
            _controllabilityCoeff += 1;
            dataBase.Update_characteristics(_carName, _controllabilityCoeff.ToString(), "controllabilityCoeff");
            dataController.Reload_coins();
            characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
            characteristicHandler.Reload_characteristics();

        }
        else Debug.Log("ERROR!!!!!!!!");
    }

    public void Upgrade_motorTorqueCoeff()
    {
        int _price, _coins;
        string _carName;
        _carName = carSelection.Active_car_name();
        _coins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
        _price = characteristicHandler.Get_characteristic_price("\'motorTorqueCoeff\'");





        if (_coins > _price)
        {
            int _motorTorqueCoeff;


            _motorTorqueCoeff = int.Parse(dataBase.Get_data_int(_carName, "value", "characteristic", "\'motorTorqueCoeff\'"));

            _coins -= _price;
            dataBase.Update_source("sources", _coins.ToString(), "gold");
            _motorTorqueCoeff += 1;
            dataBase.Update_characteristics(_carName, _motorTorqueCoeff.ToString(), "motorTorqueCoeff");
            dataController.Reload_coins();
            characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
            characteristicHandler.Reload_characteristics();

        }
        else Debug.Log("ERROR!!!!!!!!");
    }


    

    public void Pause_button()
    {
        Time.timeScale = 0;
        gearBox.SetActive(false);
        turnArrows.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Buy_car_button()
    {
        int _coins, _price;
        string _carName;
        
        _carName = carSelection.Active_car_name();
        _coins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        _price = int.Parse(dataBase.Get_data_int("cars", "price", "carName", "\'" + _carName + "\'"));

        if (_coins > _price)
        {
            _coins -= _price;
            dataBase.Update_source("sources", _coins.ToString(), "gold");
            dataBase.Update_car_bought("cars", "1", _carName);
            dataController.Reload_coins();
            carSelection.Hide_sell_window();
            characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
            characteristicHandler.Reload_characteristics();

        }
        else
        {
            Debug.LogError("Error!!!!!!");
        }

    }

    
    public void Show_store()
    {
        
        menuButtons.SetActive(false);
        storePanel.SetActive(true);


    }

    public void Collect_daily_bonus()
    {
        GameObject _dailyBonus = GameObject.Find("DailyBonus");
        _dailyBonus.GetComponent<DailyBonusController>().Collect_button();
        dataController.Reload_coins();
        _dailyBonus.transform.Find("Panel").gameObject.SetActive(false);
        menuButtons.SetActive(true);

    }

}
