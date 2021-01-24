using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsController : MonoBehaviour
{

    [SerializeField] public bool testMod = true;
    private DataController dataController;
    private DataBase dataBase;
    private GameHandler gameHandler;
    [SerializeField]
    public GameObject audioController;
    private AudioSource[] audioSs;

    private bool adsOn;

    public bool interstitialReady;
    public bool rewardedVideoReady;

    [Header("googleAdMob components")]

    private InterstitialAd interstitial;
    public RewardedAd rewardedAd;


    private string iosInterstitialID = "*";
    private string androidInterstitialId = "*";

    private string testInerstitialAndroid = "*";
    private string testInterstitialIos = "*";


    private string iosRewardedVideoID = "*";
    private string androidRewardedVideoID = "*";

    private string testIosRewardedVideoID = "*";
    private string testAndroidRewardedVideoID = "*";


    [Header("Ui components")]

    [SerializeField] public GameObject restartButton;
    [SerializeField] public GameObject quitButton;

    private void Start()
    {
        testMod = true;
        dataController = GameObject.Find("DataController").GetComponent<DataController>();
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();

        interstitialReady = false;
        rewardedVideoReady = false;

        if (int.Parse(dataBase.Get_res_inf("sources", "noAds")) == 1) adsOn = false;
        else adsOn = true;

        if (adsOn)
        {
            MobileAds.Initialize(initStatus => { });

            Request_Interstitial();

            Request_rewarded_video();

        }

    }


    public void Show_ad(bool _rewVideo)
    {
        if (adsOn)
        {

            if (_rewVideo)
            {
                Debug.Log(string.Format("Status of rewVideo = {0}", this.rewardedAd.IsLoaded()));
                if (this.rewardedAd.IsLoaded())
                {

                    Debug.Log("<color = red> Показать видео за награду </color> ");
                    this.rewardedAd.Show();
                }
            }
            else
            {

                if (this.interstitial.IsLoaded())
                {
                    this.interstitial.Show();
                }
            }
        }


    }

    private void Request_Interstitial()
    {
        if (testMod)
        {

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.interstitial = new InterstitialAd(testInterstitialIos);
            }
            else
            {
                Debug.Log("Android platform !!!!!!!!");
                this.interstitial = new InterstitialAd(testInerstitialAndroid);
            }



        }

        else if (!testMod)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.interstitial = new InterstitialAd(iosInterstitialID);
            }

            else
            {
                this.interstitial = new InterstitialAd(androidInterstitialId);
            }



        }


        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);



    }

    private void Request_rewarded_video()
    {


        if (testMod)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Android platform !!!!!!!!");
                this.rewardedAd = new RewardedAd(testAndroidRewardedVideoID);

            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.rewardedAd = new RewardedAd(testIosRewardedVideoID);

            }
            else
            {
                this.rewardedAd = new RewardedAd(testAndroidRewardedVideoID);
            }

        }
        else
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                this.rewardedAd = new RewardedAd(androidRewardedVideoID);

            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.rewardedAd = new RewardedAd(iosRewardedVideoID);

            }

        }

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    /*
     *     ============= Interstitial ==================
     */

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        interstitialReady = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load Interstitial");
        interstitial.Destroy();
        Request_Interstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //Audio of


        if (SettingsButtons.soundOn)
        {
            audioSs = GameObject.Find("AudioController").GetComponentsInChildren<AudioSource>();
            foreach (AudioSource s in audioSs)
            {
                s.Stop();
            }
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {


        //Audio on


        if (SettingsButtons.soundOn)
        {
            audioSs = GameObject.Find("AudioController").GetComponentsInChildren<AudioSource>();
            foreach (AudioSource s in audioSs)
                    {
                        s.Play();
                    }
        }



        interstitial.Destroy();
        Request_Interstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }





    /*
     *     ============= Rewarded Ad ==================
     */




    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        rewardedVideoReady = true;
        Debug.Log("Рекламное видео готово");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Failed to load rewVideo");
        Request_rewarded_video();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        if (SettingsButtons.soundOn)
        {
            audioSs = GameObject.Find("AudioController").GetComponentsInChildren<AudioSource>();

            foreach (AudioSource s in audioSs)
            {
                s.Stop();
            }
        }

    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        // Извиниться перед пользователем
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (SettingsButtons.soundOn)
        {
            audioSs = GameObject.Find("AudioController").GetComponentsInChildren<AudioSource>();

            foreach (AudioSource s in audioSs)
            {
                s.Play();
            }
        }

    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {

        dataController.Add_coins(true);
        gameHandler.Quit_ads();


    }






}
