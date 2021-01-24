using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class SettingsButtons : MonoBehaviour
{
    [SerializeField] public Animator anim;

    [SerializeField] public GameObject soundButton;
    [SerializeField] public Sprite soundSpriteOn, soundSpriteOff;
    public static bool soundOn = true;

    [SerializeField] public GameObject musicButton;
    [SerializeField] public Sprite musicSpriteOn, musicSpriteOff;
    public static bool musicOn = true;

    [SerializeField] public GameObject vibrationButton;
    [SerializeField] public Sprite vibrationSpriteOn, vibrationSpriteOff;
    public static bool vibrationOn = true;

    [SerializeField] public GameObject adsButton;
    [SerializeField] public Sprite adsSpriteOn, adsSpriteOff;
    public static bool adsOn = true;

    public GameObject audioController;

    private DataBase dataBase;

    private AudioListener audioL;

    public AudioSource audioS;

    [SerializeField] GameObject notificationWin;


    private void Start()
    {
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();

        anim = GetComponent<Animator>();

        audioL = GameObject.Find("Main Camera").GetComponent<AudioListener>();

        soundSpriteOn = Resources.Load<Sprite>("Settings/SoundOn");
        soundSpriteOff = Resources.Load<Sprite>("Settings/SoundOff");

        musicSpriteOn = Resources.Load<Sprite>("Settings/MusicOn");
        musicSpriteOff = Resources.Load<Sprite>("Settings/MusicOff");
        
        vibrationSpriteOn = Resources.Load<Sprite>("Settings/VibrationOn");
        vibrationSpriteOff = Resources.Load<Sprite>("Settings/VibrationOff");

        

        adsSpriteOn = Resources.Load<Sprite>("Settings/AdsOn");

        if (!soundOn) soundButton.GetComponent<Image>().sprite = soundSpriteOff;
        if (!musicOn) musicButton.GetComponent<Image>().sprite = musicSpriteOff;
        if (!vibrationOn) vibrationButton.GetComponent<Image>().sprite = vibrationSpriteOff;

        if (int.Parse(dataBase.Get_res_inf("sources", "noAds")) == 1)
        {
            adsButton.SetActive(false);
        }


    }

   

    public void Show_settings()
    {
        if (anim.GetBool("showFirst") && anim.GetBool("isOpen"))
        {
            anim.SetBool("isOpen", false);
            
        }
        else if (anim.GetBool("showFirst") && !anim.GetBool("isOpen"))
        {
            anim.SetBool("isOpen", true);
            
        }
        else if (!anim.GetBool("showFirst"))
        {
            anim.SetBool("showFirst", true);
            
        }
    }

    public void Sound_button()
    {
        

        if (soundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOff;
            soundOn = false;
            
            
        }
        else if (!soundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSpriteOn;
            soundOn = true;
            
        }
        audioController.SetActive(soundOn);

    }

    public void Music_button()
    {
        AudioSource audioS = GameObject.Find("BGSound").GetComponent<AudioSource>();

        if (musicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOff;
            musicOn = false;
            
        }
        else if (!musicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicSpriteOn;
            musicOn = true;

        }
        audioS.enabled = musicOn;
    }

    public void Vibration_button()
    {
        if (vibrationOn)
        {
            vibrationButton.GetComponent<Image>().sprite = vibrationSpriteOff;
            vibrationOn = false;
        }
        else if (!vibrationOn)
        {
            vibrationButton.GetComponent<Image>().sprite = vibrationSpriteOn;
            vibrationOn = true;
        }
    }

    public void Ads_isOffed()
    {

        Debug.Log("1");
        dataBase.Update_source("sources", "1", "noAds");
        adsButton.SetActive(false);
        notificationWin.SetActive(true);
        var _thanksMessage = "Thank you for supporting my project. \n Have a nice game)";
        notificationWin.GetComponentInChildren<Text>().text = _thanksMessage;
        Debug.Log("2");
    }

    public void Ads_fail()
    {
        var _sorryMessage = "Purchase failed \n Sorry = (";

        notificationWin.SetActive(true);
        notificationWin.GetComponentInChildren<Text>().text = _sorryMessage;
    }
}
 