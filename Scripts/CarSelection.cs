using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSelection : MonoBehaviour
{
    private int activeCarIndex;
    private string activeCarName;
    private DataController dataController;
    private DataBase dataBase;


    [SerializeField] public GameObject selectButton;
    public GameObject stat;
    [SerializeField] public GameObject sellWindow;
    [SerializeField] private TextMeshProUGUI priseTMP;

    private CharacteristicHandler characteristicHandler;
    void Start()
    {
        dataController = GameObject.Find("DataController").GetComponent<DataController>();
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
        characteristicHandler = GameObject.Find("Stat").GetComponent<CharacteristicHandler>();
        
        stat = GameObject.Find("Stat");


        activeCarName = dataBase.Get_data("cars", "carName", "available", "1");

        

        for (int i =0; i < transform.childCount; i++ )
        {
            if (transform.GetChild(i).gameObject.name == activeCarName)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                activeCarIndex = i;
                characteristicHandler.Reload_characteristics();

            }

            dataBase.Add_new_cars("cars", transform.GetChild(i).gameObject.name, "0");
        }




        

    }

    public void Next_car ()
    {
        string _oldCar, _newCar;
        

        if (activeCarIndex < transform.childCount-1)
        {
            transform.GetChild(activeCarIndex).gameObject.SetActive(false);
            _oldCar = transform.GetChild(activeCarIndex).gameObject.name;
            activeCarIndex += 1;
            transform.GetChild(activeCarIndex).gameObject.SetActive(true);
            _newCar = transform.GetChild(activeCarIndex).gameObject.name;

            int _isBought = int.Parse(dataBase.Get_data_int("cars", "bought", "carName", "\'" + _newCar + "\'"));

            if (_isBought == 0)
            {
                Show_sell_window(_newCar);
            }
            if (_isBought == 1)
            {
                Hide_sell_window();
                Select_button_active(_newCar);
                characteristicHandler.Reload_characteristics();
            }

            

        }
        else
        {
            transform.GetChild(activeCarIndex).gameObject.SetActive(false);
            _oldCar = transform.GetChild(activeCarIndex).gameObject.name; ;
            activeCarIndex = 0;
            transform.GetChild(activeCarIndex).gameObject.SetActive(true);
            _newCar = transform.GetChild(activeCarIndex).gameObject.name;
            Debug.Log(string.Format("last activIndex = {0}", activeCarIndex));

            int _isBought = int.Parse(dataBase.Get_data_int("cars", "bought", "carName", "\'" + _newCar + "\'"));

            if (_isBought == 0)
            {
                Show_sell_window(_newCar);
            }
            if (_isBought == 1)
            {
                Hide_sell_window();
                Select_button_active(_newCar);
                characteristicHandler.Reload_characteristics();
            }

        }


    }

    public void Previouse_car()
    {
        string _newCar;
        
        if (activeCarIndex>0)
        {
            transform.GetChild(activeCarIndex).gameObject.SetActive(false);
            activeCarIndex -= 1;
            transform.GetChild(activeCarIndex).gameObject.SetActive(true);
            _newCar = transform.GetChild(activeCarIndex).gameObject.name;

            int _isBought = int.Parse(dataBase.Get_data_int("cars", "bought", "carName", "\'" + _newCar + "\'"));

            if (_isBought == 0)
            {
                Show_sell_window(_newCar);
            }
            if (_isBought == 1)
            {
                Hide_sell_window();
                Select_button_active(_newCar);
                characteristicHandler.Reload_characteristics();
            }
        }
        else
        {
            transform.GetChild(activeCarIndex).gameObject.SetActive(false);
            activeCarIndex = transform.childCount-1;
            transform.GetChild(activeCarIndex).gameObject.SetActive(true);
            _newCar = transform.GetChild(activeCarIndex).gameObject.name;


            int _isBought = int.Parse(dataBase.Get_data_int("cars", "bought", "carName", "\'" + _newCar + "\'"));

            if (_isBought == 0)
            {
                Show_sell_window(_newCar);
            }
            if (_isBought == 1)
            {
                Hide_sell_window();
                Select_button_active(_newCar);
                characteristicHandler.Reload_characteristics();
            }
        }
    }


    private void Select_button_active(string _name)
    {
        int _value;

        string _data = dataBase.Get_data_int("cars", "available", "carName", "\'" + _name + "\'");

        _value = int.Parse(_data);
        
        if (_value == 0)
        {
            selectButton.SetActive(true);

        }
        if (_value == 1)
        {
            selectButton.SetActive(false);
        }
    }

    public void Select_button_func()
    {
        string _oldCar = dataController.Get_active_car();


        dataController.Select_car(_oldCar, transform.GetChild(activeCarIndex).gameObject.name);
        selectButton.SetActive(false);
    }

    public string Active_car_name()
    {
        string _name;
        _name = transform.GetChild(activeCarIndex).gameObject.name;
        return _name;
    }

    private void Show_sell_window(string _carName)
    {
        stat.SetActive(false);
        selectButton.SetActive(false);
        sellWindow.SetActive(true);
        priseTMP.text = dataBase.Get_data_int("cars", "price", "carName", "\'" + _carName + "\'");

    }
    public void Hide_sell_window()
    {
        stat.SetActive(true);
        selectButton.SetActive(true);
        sellWindow.SetActive(false);
    }
}
