using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DailyRewardComponent : MonoBehaviour
{
    [Header("Properties")]
    public int serverTime;
    public int lastReceiveBonusTime;
    public int dayInRow;

    [Space]

    public bool isNetworkError;
    public bool isHttpError;
    public bool isLoaded;
    public bool isCompleteLoaded;
    


    [Header("Settings")]

    [SerializeField] private string localLastReceiveBonuseTimeKey;
    [SerializeField] private string localDayInRowKey;

    [Space]

    [SerializeField] private string serverURL;

    [Space]

    [SerializeField] private bool useLocalData;
    [SerializeField] private int localTime;


    [Header("Sourcses")]
    [SerializeField] public DataBase db;

    private void Awake()
    {


        if (this.useLocalData)
        {
            LoadLocalData();
            this.serverTime = this.localTime;
            this.isCompleteLoaded = true;
            this.isLoaded = true;

        }
        else
        {
            LoadLocalData();
            StartCoroutine(routine: SendRequest());

        }
    }


    private IEnumerator SendRequest()
    {
        

        UnityWebRequest request = UnityWebRequest.Get(this.serverURL);

        

        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {

            
            this.isNetworkError = true;
            this.isLoaded = true;
            Debug.Log(message: "{GameLog} => [DailyRewardComponent] - (<color=yellow>NetWorkError</color>) -> " + request.error);
            yield break;
        }

        if (request.isHttpError)
        {
            

            this.isHttpError = true;
            this.isLoaded = true;
            Debug.Log(message: "{GameLog} => [DailyRewardComponent] - (<color=yellow>HttpError</color>) -> " + request.error);

        }

        string json = request.downloadHandler.text;
        ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(json);

        

        this.serverTime = response.unixtime;
        this.isCompleteLoaded = true;
        this.isLoaded = true;

    }

    public IEnumerator CheckDailyReward(Action<int> callback)
    {
        while (!this.isLoaded)
        {
            yield return new WaitForSeconds(0.25f);
        }
        if (this.isNetworkError)
        {
            callback(-1);
            yield break;
        }
        if (this.isHttpError)
        {
            callback(-2);

        }

        if (this.isCompleteLoaded)
        {
            Calulate_day();
            callback(this.dayInRow);
        }
    }

    

    private void LoadLocalData()
    {



        this.lastReceiveBonusTime = int.Parse(db.Get_data_day_counter("localLastReceiveBonuseTimeKey"));
        this.dayInRow = int.Parse(db.Get_data_day_counter("localDayInRowKey"));
        

    }

    private void SetLastReceiveBonusTime()
    {
        this.lastReceiveBonusTime = this.serverTime;
        db.Upload_day_counter("localLastReceiveBonuseTimeKey", this.lastReceiveBonusTime);
        db.Upload_day_counter("localDayInRowKey", this.dayInRow);
        
    }

    private void Calulate_day()
    {
        if (this.dayInRow == 0)
        {
            this.dayInRow = 1;
            SetLastReceiveBonusTime();
        }
        else
        {
            int _timeDifference = this.serverTime - this.lastReceiveBonusTime;

            if (_timeDifference <= 86400)
            {
                this.dayInRow = 0;
            }
            if (_timeDifference > 86400 && _timeDifference < 86400 * 2)
            {
                if (this.dayInRow == 7)
                {
                    this.dayInRow = 1;
                }
                else
                {
                    this.dayInRow += 1;
                }

                SetLastReceiveBonusTime();
            }
            if (_timeDifference > 86400 * 2)
            {
                this.dayInRow = 1;
                SetLastReceiveBonusTime();
            }
        }
    }



}
