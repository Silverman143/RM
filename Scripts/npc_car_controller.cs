using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class npc_car_controller : MonoBehaviour
{
    [SerializeField]
    private List<Wheel> wheels;
    private Rigidbody rb;
    private Transform player;
    private bool isHited = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void FixedUpdate()
    {
        if(player.position.x-transform.position.x > 20)
        {
            Destroy(gameObject);
        }
        if (!isHited)
        {
            rb.AddForce(transform.forward * 10000f);
            AnimatWheels();
        }
        
        
    }


    private void AnimatWheels()
    {
        foreach (var wheel in wheels)
        {


            wheel.model.transform.Rotate(new Vector3(4, 0, 0));

            
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHited = true;
        }
    }
}
