using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    private DataBase dataBase;
    private TextMeshProUGUI coinsTMP;

    [SerializeField] public GameObject notificationWin;
    [SerializeField] public GameObject menuButtons;
    [SerializeField] public Animator animator;
    

    private void Start()
    {

        coinsTMP = GameObject.Find("Coins_tmp").GetComponent<TextMeshProUGUI>();
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
    }

    public void Button_low_kit()
    {
        Add_coins(1500);
        coinsTMP.text = dataBase.Get_res_inf("sources", "gold");

        Purchase_done();
    }

    public void Button_medium_kit()
    {
        Add_coins(4200);
        coinsTMP.text = dataBase.Get_res_inf("sources", "gold");
        Purchase_done();
    }

    public void Button_big_kit()
    {
        Add_coins(10000);
        coinsTMP.text = dataBase.Get_res_inf("sources", "gold");
        Purchase_done();
    }


    public void Purchase_failed()
    {
        var _sorryMessage = "Purchase failed \n Sorry = (";

        notificationWin.SetActive(true);
        notificationWin.GetComponentInChildren<Text>().text = _sorryMessage;
    }

    private void Purchase_done()
    {
        var _thanksMessage = "Thank you for supporting my project. \n Have a nice game)";
        notificationWin.SetActive(true);
        notificationWin.GetComponentInChildren<Text>().text = _thanksMessage;
    }
    
    public void Button_back()
    {
        
        gameObject.SetActive(false);
        menuButtons.SetActive(true);
    }


    private void Add_coins(int _amount)
    {
        int _wasCoins;

        _wasCoins= int.Parse(dataBase.Get_res_inf("sources", "gold"));
        _wasCoins += _amount;
        dataBase.Update_source("sources", _wasCoins.ToString(), "gold");
    }
}
