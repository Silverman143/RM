using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrails : MonoBehaviour
{
    private TrailRenderer trail;
    private WheelCollider wheelCollider;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        WheelHit hit;
        if (wheelCollider.GetGroundHit(out hit))
        {
            trail.emitting = true;
            
        }
        else
        {
            trail.emitting = false;
            
        }
    }
}
