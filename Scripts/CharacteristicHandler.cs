using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacteristicHandler : MonoBehaviour
{
    private DataBase dataBase;
    private CarSelection carSelection;
    private TextMeshProUGUI motorTorqueCoeffTMP, fuelTMP, controllabilityCoeffTMP;


    private GameObject motorTorqueCoeffButton;
    private GameObject fuelTankSizeButton;
    private GameObject controllabilityCoeffButton;


    [SerializeField]public int priceCoeff;

    private int speedCoeff, fuelTankCoeff, handleabilityCoeff;
    private string visibleCarName;

    private GameObject visibleCar;


    private void Awake()
    {
        priceCoeff = 10;

        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();

        if (SceneManager.GetActiveScene().name == "CarSelection")
        {
            carSelection = GameObject.Find("Car_holder").GetComponent<CarSelection>();

            motorTorqueCoeffButton = GameObject.Find("motorTorqueCoeff").transform.Find("Button").gameObject;
            fuelTankSizeButton = GameObject.Find("FuelTankSize").transform.Find("Button").gameObject;
            controllabilityCoeffButton = GameObject.Find("controllabilityCoeff").transform.Find("Button").gameObject;
            

            motorTorqueCoeffTMP = GameObject.Find("motorTorqueCoeff_lvl_tmp").GetComponent<TextMeshProUGUI>();
            fuelTMP = GameObject.Find("FuelTankSize_lvl_tmp").GetComponent<TextMeshProUGUI>();
            controllabilityCoeffTMP = GameObject.Find("controllabilityCoeff_lvl_tmp").GetComponent<TextMeshProUGUI>();



            Reload_characteristics();
        }
        
        


    }

    public void Reload_characteristics()
    {
        visibleCarName = carSelection.Active_car_name();
        Show_characteristic(motorTorqueCoeffTMP, motorTorqueCoeffButton, "\'motorTorqueCoeff\'");
        Show_characteristic(fuelTMP, fuelTankSizeButton, "\'FuelTankSize\'");
        Show_characteristic(controllabilityCoeffTMP, controllabilityCoeffButton, "\'controllabilityCoeff\'");
    }

    private void Show_characteristic(TextMeshProUGUI _tmp, GameObject _button, string _charactName)
    {
        string _text;
        int _price, _lvl;
        visibleCarName = carSelection.Active_car_name();
        _text = dataBase.Get_data_int(visibleCarName, "value", "characteristic", _charactName);
        _lvl = int.Parse(_text);
        _text += "/10";
        if (_lvl < 10)
        {
            _price = Get_characteristic_price(_charactName);
            _button.transform.Find("Button_tmp").GetComponent<TextMeshProUGUI>().text = _price.ToString();
            _button.GetComponent<Button>().enabled = true;
        }
        else
        {
            _button.transform.Find("Button_tmp").GetComponent<TextMeshProUGUI>().text = "Max";
            _button.GetComponent<Button>().enabled = false;
        }

        _tmp.text = _text;
        

    }

    public int Get_characteristic_price(string _charactName)
    {
        int _price;
        _price = int.Parse(dataBase.Get_data_int(visibleCarName, "value", "characteristic", _charactName))*5 + priceCoeff;

        return _price;
    }
    
    public int Get_characteristic_inf(string _carName, string _charactName)
    {
        int _value;
        
        
        _value = int.Parse(dataBase.Get_data_int(_carName,"value", "characteristic", _charactName ));
        return _value;
    }
}
