using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGetter : MonoBehaviour
{
    
    [SerializeField] private DataBase dataBase;
    private string activeCar;
    private GameObject carObj;

    void Awake()
    {
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();

        if (SceneManager.GetActiveScene().name == "Road" )
        {
            
            Create_car_on_road();
        }
        


    }

    public void Show_car_in_menu()
    {
        
        activeCar = dataBase.Get_data("cars", "carName", "available", "1");

        

        var car = Resources.Load<GameObject>(activeCar); 

        if (car == null)
        {
            Debug.Log("Не нашел обьект");
        }
        else
        {

            Transform player = GameObject.Find("Player").transform;
            carObj = Instantiate(car, player.position, player.rotation, player);
            carObj.tag = "Player";
            carObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            Turn_off_the_car(carObj);


        }
    }

    public void Turn_off_the_car(GameObject _car)
    {
        _car.GetComponent<CarController>().enabled = false;
        WheelSkidMarks[] _wheelsSM = _car.GetComponentsInChildren<WheelSkidMarks>();
        for (int i = 0; i < 4; i++)
        {
            _wheelsSM[i].enabled = false;
        }
    }

    public void Create_car_on_road()
    {
        
        activeCar = dataBase.Get_data("cars", "carName", "available", "1");

        var car = Resources.Load<GameObject>(activeCar);

        if (car == null)
        {
            Debug.Log("Не нашел обьект");
        }
        else
        {

            Transform player = GameObject.Find("Player").GetComponent<Transform>();
            carObj = Instantiate(car, player.position, player.rotation, player);
            carObj.tag = "Player";




        }
    }
}
    

