using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameHandler : MonoBehaviour
{
    private GameObject player;

    private HealthBar healthBar;
    private float healthCount = 1f;
    private float time;
    private TextMeshProUGUI coinCounterText;
    private TextMeshProUGUI distanceTMP;
    private int coinsAmount;
    private DataBase dataBase;
    private CharacteristicHandler characteristicHandler;
    [SerializeField] private GameObject bonusPanel;

    [SerializeField] public GameObject speedCounter;

    public static bool rwReady = false;
    private float fuelTime;

    private bool pausGame = false;
    [SerializeField] public static bool isGameOver = false;

    public GameObject gameOverPanel;
    [SerializeField] public GameObject gearBox;


    private int fuelTankSize;
    private float consconsumptionValue;


    private DataController dataController;


    private static int nomberOfGames = 0;

    private AdsController adsController;

    public GameObject tutorial;
    public GameObject turnArrows;
    public GameObject flipButton;

    public GameObject restartButton;
    public GameObject quitButton;

    

    private void Awake()
    {
        
    }

    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        isGameOver = false;
        Time.timeScale = 1;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        coinCounterText = GameObject.Find("Coins_tmp").GetComponent<TextMeshProUGUI>();
        distanceTMP = GameObject.Find("Distance_tmp").GetComponent<TextMeshProUGUI>();
     
        dataController = GameObject.Find("DataController").GetComponent<DataController>();
        characteristicHandler = GameObject.Find("Player").GetComponent<CharacteristicHandler>();
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
        coinsAmount = 0;
        adsController = GameObject.Find("AdsController").GetComponent<AdsController>();

        gameOverPanel.SetActive(false);
        fuelTankSize = fuelTankSize = characteristicHandler.Get_characteristic_inf(GameObject.FindGameObjectWithTag("Player").name, "\'FuelTankSize\'");
        

        time = 0f;
        consconsumptionValue = 0.005f - (0.00015f * fuelTankSize);

        player = GameObject.FindGameObjectWithTag("Player");

        int _isTutorialed = int.Parse( dataBase.Get_res_inf("sources", "tutorial"));

        if (SceneManager.GetActiveScene().name == "Road" & _isTutorialed == 0)
        {
            Time.timeScale = 0;
            tutorial.SetActive(true);
        }
        


    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {

                healthBar.SetSize(healthCount);

            }

            if (healthCount < 0.5f & time > 0.03f)
            {
                time = 0f;
                healthBar.LowHealth();
            }
            if (healthCount > 0.5f)
            {
                healthBar.Normal_color();

            }
            else
            {
                time += Time.deltaTime;
            }


            Fuel_consumption(consconsumptionValue);

            if (healthCount <= 0f) Game_over();
        }
        

    }

    public void Take_damage(float _volume)
    {
        healthCount -= _volume;
        Normalize_fuel_counter();
        healthBar.SetSize(healthCount);
    }
    public void Take_healing(float _volume)
    {
        healthCount += _volume;
        Normalize_fuel_counter();
        healthBar.SetSize(healthCount);
    }

    private void Fuel_consumption(float _value)
    {
        if (fuelTime < 0)
        {
            fuelTime = 0.1f;
            Take_damage(_value);
        }
        else
        {
            fuelTime -= Time.deltaTime;
        }

    }
    private void Normalize_fuel_counter()
    {
        if (healthCount > 1f) healthCount = 1f;
        if (healthCount < 0) healthCount = 0f;
        

    }

    public void Stop_button()
    {
        if (pausGame)
        {
            turnArrows.SetActive(true);
            Time.timeScale = 1;
            pausGame = false;
        }
        else
        {
            turnArrows.SetActive(false);
            Time.timeScale = 0;
            pausGame = true;
        }
    }

    private void Game_over()
    {
        isGameOver = true;
        turnArrows.SetActive(false);
        if (flipButton.activeSelf) flipButton.SetActive(false);

        player.GetComponentInChildren<CarController>().enabled = false;

        player.GetComponent<Rigidbody>().mass *= 10000;
        speedCounter.SetActive(false);
        gearBox.SetActive(false);
        string _recordText = distanceTMP.text;
        int _record = int.Parse(_recordText);
        int _dbRecord = int.Parse(dataBase.Get_res_inf("sources", "record"));
        if (_record > _dbRecord)  dataBase.Update_source("sources", _record.ToString(), "record");

        gameOverPanel.SetActive(true);

        

        if (adsController.rewardedAd.IsLoaded())
        {
            Debug.Log("Try to show ads REw");
            restartButton.SetActive(false);
            quitButton.SetActive(false);

            bonusPanel.SetActive(true);


            BonusTimer.showAds = true;
        }
        else
        {
            //Debug.Log("Try to show ads");
            //adsController.Show_ad(false);

            restartButton.SetActive(false);
            quitButton.SetActive(false);

            bonusPanel.SetActive(true);


            BonusTimer.showAds = true;
        }



        

        //if (nomberOfGames>5)
        //{
            
        //    adsController.Show_ad(false);
        //    //bonusPanel.SetActive(true);

        //    //BonusTimer.showAds = true;
        //}
        //else if (rwReady)
        //{
        //    bonusPanel.SetActive(true);

        //    BonusTimer.showAds = true;
        //}

        nomberOfGames++;
        dataController.Add_coins(false);


    }

    public void Take_coin()
    {
        coinsAmount += 1;
        coinCounterText.text = coinsAmount.ToString();


    }

    public void Show_bonus_coins()
    {
        int _bonus = coinsAmount * 3;

        coinCounterText.text = _bonus.ToString();
    }


    public void OnApplicationQuit()
    {
        Stop_button();
    }

    public void OnApplicationPause(bool pause)
    {
        Stop_button();
    }

    public void Quit_ads()
    {
        Show_bonus_coins();
        turnArrows.SetActive(false);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
    }
}
