using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    private Transform _obstruction;
    private Vector3 _offset;

    private Transform player;
    

    void Start()
    {
        
        _player = GameObject.FindGameObjectWithTag("Player");
        _offset = transform.position - _player.transform.position;
        Debug.Log(_player);
    }

    private void LateUpdate()
    {
        transform.position = _player.transform.position + _offset;
        ViewObstructed();
    }

    private void ViewObstructed()
    {

        RaycastHit hit;
        Ray ray = new Ray(transform.position, _player.transform.position - transform.position);
        Physics.Raycast(ray, out hit);

        
        



    }


}
