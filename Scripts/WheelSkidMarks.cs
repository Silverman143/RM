using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WheelSkidMarks : MonoBehaviour
{
    private Vector3 carForwardV;
    private Vector3 wheelForwardV;

    private Rigidbody car;
    private TrailRenderer trailR;
    private WheelCollider wCollider;
    private AudioController audioController;
    

    private void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        trailR = GetComponent<TrailRenderer>();
        wCollider = GetComponent<WheelCollider>();
        if (SceneManager.GetActiveScene().name == "Road")
        {
            audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
            
        }
    }

    private void Update()
    {
        if (Drift() && wCollider.GetGroundHit(out WheelHit hit))
        {
            trailR.emitting = true;
            audioController.WheelsCreakController(true);
            
        }
        else
        {
            audioController.WheelsCreakController(false);
            trailR.emitting = false;
        }
    }

    private bool Drift()
    {
        carForwardV = car.velocity.normalized;
        wheelForwardV = gameObject.transform.forward;



        if (Vector3.Angle(carForwardV, wheelForwardV) > 10)
        {

            audioController.WheelsCreakController(true);
            return true;
        }

        else return false;
        
        
    }

}
