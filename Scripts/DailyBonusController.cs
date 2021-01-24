using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DailyBonusController : MonoBehaviour
{

    [SerializeField] public GameObject[] days;
    [SerializeField] public DataBase dataBase;
    [SerializeField] private TextMeshProUGUI bonusCoinsTMP;

    public void Show_bonus(int _dayInRow)
    {
        if (_dayInRow > 1)
        {
            for (int i = 0; i < _dayInRow - 1; i++)
            {
                Completed_day(days[i]);
                Debug.Log(string.Format("<color=red> compile Day = {0} </color>", i+1));
            }
            days[_dayInRow - 1].transform.Find("Plug").gameObject.SetActive(false);
            days[_dayInRow - 1].transform.Find("Bonus").gameObject.SetActive(true);
            

        }
        days[_dayInRow - 1].transform.Find("Bonus").transform.Find("BonusCoinPic").transform.GetComponent<Animator>().SetBool("Selected", true);
        bonusCoinsTMP = days[_dayInRow - 1].transform.Find("Bonus").GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Completed_day(GameObject _dayBlock)
    {
        if (_dayBlock.name != "Block_1")
        {
            _dayBlock.transform.Find("Plug").gameObject.SetActive(false);
            _dayBlock.transform.Find("Bonus").gameObject.SetActive(true);
        }
        _dayBlock.transform.Find("Ready").gameObject.SetActive(true);
    }

    public void Collect_button()
    {
        int _wasCoins, _addCoins;

        
        _wasCoins = int.Parse(dataBase.Get_res_inf("sources", "gold"));
        _addCoins = int.Parse(bonusCoinsTMP.text);
        _addCoins += _wasCoins;
        dataBase.Update_source("sources", _addCoins.ToString(), "gold");


    }
}
