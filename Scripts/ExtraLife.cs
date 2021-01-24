using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    [SerializeField] private GameObject playerCar;
    [SerializeField] public GameObject flipButton;
    [SerializeField] private float timer;
    [SerializeField] private bool flipButtonOn;

    private void Start()
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        timer = 0;
        flipButtonOn = false;
    }

    private void FixedUpdate()
    {
        

       if ((playerCar.transform.up.y <= 0.6f | playerCar.transform.up.y >= 1.4f) & !flipButtonOn)
        {
            timer += 1f;
            if (timer >= 50f)
            {
                flipButton.SetActive(true);
                timer = 0;
            }
        }
       else
        {
            flipButton.SetActive(false);
            timer = 0;
        }
    }


    public void Flip_the_car()
    {
        Vector3 _pos = playerCar.transform.position;
        
       
        _pos.y += 0.5f;

        playerCar.transform.rotation = Quaternion.Euler(0, 90, 0);
        playerCar.transform.position = _pos;
        flipButtonOn = false;
        flipButton.SetActive(false);
        

    }
}
