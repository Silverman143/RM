using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] public RoadBlock[] roadPrefs;
    [SerializeField] public GameObject[] roads;
    public GameObject NPC_car;
    public RoadBlock firstRoad;
    public Transform obstructions;
    public GameObject hatchPref;
    public GameObject fuelTankPref;
    public GameObject coinPref;

    public GameObject road1;


    private List<RoadBlock> existRoads = new List<RoadBlock>();


    void Start()
    {
        NPC_car = Resources.Load<GameObject>("Prefabs/NPC_cars/ambulance");
        ///z - (5, -8)
        ///x (+60)
        ///Road1(Clone)
        Debug.Log(NPC_car.name);

        existRoads.Add(firstRoad);

        Vector3 frPos = firstRoad.transform.position;

       

        Fuel_tanks_spowner(frPos, true);
        Coin_spowner(frPos, true);

        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            List<Vector3> _existHatchsPos = new List<Vector3>();
            Vector3 pos = new Vector3();
            bool _goodPos = true;

            pos.y = -0.9f;
            pos.x = Random.Range(frPos.x , frPos.x + 50);
            pos.z = Random.Range(frPos.z - 7, frPos.z + 7);



            if (_existHatchsPos.Count > 0)
            {
                for (int k = 0; k < _existHatchsPos.Count - 1; k++)
                {

                    float _dist = Vector3.Distance(pos, _existHatchsPos[k]);

                    if (_dist < 15f)
                    {
                        _goodPos = false;
                        break;

                    }

                }

                if (_goodPos)
                {
                    GameObject newHatch = Instantiate(hatchPref, pos, Quaternion.identity, obstructions);

                    _existHatchsPos.Add(pos);
                }
            }
            else
            {
                GameObject newHatch = Instantiate(hatchPref, pos, Quaternion.identity, obstructions);
                _existHatchsPos.Add(pos);

            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        SpownRoad();
    }



    public void SpownRoad()
    {
        Vector3 newRoadPos = new Vector3();
        List<Vector3> _existHatchsPos = new List<Vector3>();

        RoadBlock newRoad = Instantiate(roadPrefs[Random.Range(0, roadPrefs.Length)], road1.transform);
        
        newRoadPos = existRoads[existRoads.Count - 1].transform.position;
        newRoadPos.x += 100;
        newRoad.transform.position = newRoadPos;
        StaticBatchingUtility.Combine(newRoad.gameObject);
        existRoads.Add(newRoad);
        if(newRoad.name == "Road1(Clone)" & Random.Range(0, 2) == 0)
        {
            Create_npc_car(newRoadPos);
        }

     
        Fuel_tanks_spowner(newRoadPos, false);
        Coin_spowner(newRoadPos, false);
       
        int _amount = Random.Range(10, 25);
        
        while (_amount>0)
        {
            Vector3 pos = new Vector3();
            bool _goodPos = true;

            pos.y = -0.9f;
            pos.x = Random.Range(newRoadPos.x - 45, newRoadPos.x + 45);
            pos.z = Random.Range(newRoadPos.z - 7, newRoadPos.z + 7);



            if (_existHatchsPos.Count > 0)
            {
                for (int k = 0; k < _existHatchsPos.Count - 1; k++)
                {

                    float _dist = Vector3.Distance(pos, _existHatchsPos[k]);

                    if (_dist < 2f)
                    {
                        _goodPos = false;

                        break;

                    }

                }

                if (_goodPos)
                {
                    
                    GameObject newHatch = Instantiate(hatchPref, pos, Quaternion.identity, obstructions);
                    
                    _existHatchsPos.Add(pos);
                    _amount--;
                }
            }
            else
            {
                GameObject newHatch = Instantiate(hatchPref, pos, Quaternion.identity, obstructions);
                
                _existHatchsPos.Add(pos);

            }
        }

      

    }

    private void Fuel_tanks_spowner(Vector3 _newRoadPos, bool _isFirst)
    {
        int _amount = Random.RandomRange(1, 3);
        List<Vector3> _fuelPoss = new List<Vector3>();

        while (_amount > 0)
        {
            bool _goodPos = true;
            Vector3 pos = new Vector3();

            if (!_isFirst)
            {
                pos.x = Random.Range(_newRoadPos.x - 50, _newRoadPos.x + 50);
            }
            else
            {
                pos.x = Random.Range(_newRoadPos.x - 15, _newRoadPos.x + 50);
            }
            pos.y = -0.5f;
            
            pos.z = Random.Range(_newRoadPos.z - 7, _newRoadPos.z + 7);

            if (_fuelPoss.Count > 0)
            {
                for (int k = 0; k < _fuelPoss.Count - 1; k++)
                {
                    if (Vector3.Distance(pos, _fuelPoss[k]) < 10) _goodPos = false;
                    else _goodPos = true;
                }

                if (_goodPos)
                {
                    GameObject newFuelTank = Instantiate(fuelTankPref, pos, Quaternion.identity);
                    _fuelPoss.Add(newFuelTank.transform.position);
                    _amount--;
                }
                
            }
            else
            {
                GameObject newFuelTank = Instantiate(fuelTankPref, pos, Quaternion.identity);
                _fuelPoss.Add(newFuelTank.transform.position);
            }
        }

        
    }


    private void Coin_spowner(Vector3 _newRoadPos, bool _isFirst)
    {
        for (int i = 0; i < Random.Range(3, 10); i++)
        {

            Vector3 pos = new Vector3();

            if (!_isFirst)
            {
                pos.x = Random.Range(_newRoadPos.x - 50, _newRoadPos.x + 50);
            }
            else
            {
                pos.x = Random.Range(_newRoadPos.x - 15, _newRoadPos.x + 50);
            }

            pos.y = -0.5f;
            
            pos.z = Random.Range(_newRoadPos.z - 7, _newRoadPos.z + 7);

            GameObject newFuelTank = Instantiate(coinPref, pos, Quaternion.identity);



        }
    }

    private void Create_npc_car(Vector3 _newRoadPos)
    {
        Vector3 _npc_pos = new Vector3();
        _npc_pos.y = -0.9f;
        _npc_pos.x = _newRoadPos.x + 60;
        _npc_pos.z = Random.Range(-7, 5);


        Instantiate(NPC_car, _npc_pos, Quaternion.Euler(new Vector3(0, -90, 0)), obstructions);
    }
}
