using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public RoadGenerator roadGenerator;


    private void Start()
    {
        roadGenerator = FindObjectOfType<RoadGenerator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            roadGenerator.SendMessage("SpownRoad");
        }
        
    }
}
