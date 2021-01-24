using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BonusTimer : MonoBehaviour
{
    private float time = 6f;
    
    [SerializeField] public Image image;
    public static bool showAds = false;
    private bool _rewAd = false;
    private DataController dataController;
    private AdsController adsController;

    public GameObject quitButton;
    public GameObject restartButton;


    private void Start()
    {
        dataController = GameObject.Find("DataController").GetComponent<DataController>();
        adsController = GameObject.Find("AdsController").GetComponent<AdsController>();
    }

    void FixedUpdate()
    {
        if (showAds)
        {
            if (time > 0)
            {
                
                
                time -= Time.fixedDeltaTime;
                image.fillAmount = time / 6;
                
            }
            else
            {

                time = 6f;
                showAds = false;
                Skip_bonus_button();

            }
        }
        
    }

    public void Bonus_button()
    {

        time = 6f;
        showAds = false;
        adsController.Show_ad(true);
        gameObject.SetActive(false);
    }

    public void Skip_bonus_button()
    {
        adsController.Show_ad(false);
        quitButton.SetActive(true);
        restartButton.SetActive(true);
        gameObject.SetActive(false);

    }
    
}
