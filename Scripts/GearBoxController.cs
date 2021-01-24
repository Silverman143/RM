using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBoxController : MonoBehaviour
{
    [SerializeField] public GameObject reverseGear;
    [SerializeField] public GameObject forwardGear;

    [SerializeField]private CarController carController;


    private void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();

    }

    private void Switch_to_R()
    {
        carController.Set_reverse(true);
        forwardGear.SetActive(false);
        reverseGear.SetActive(true);
    }

    private void Switch_to_D()
    {
        carController.Set_reverse(false);
        forwardGear.SetActive(true);
        reverseGear.SetActive(false);
    }

    public void Switch_gear()
    {
        if (forwardGear.activeSelf == true)
        {
            Switch_to_R();
        }
        else
        {
            Switch_to_D();
        }
    }

}
