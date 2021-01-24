using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public enum Axel
{
    Front,
    Back
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider wheelCollider;
    public Axel axel;

}

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float maxAcceleration = 100.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private List<Wheel> wheels;

    [SerializeField] public float speed;

    private float inputX, inputY;
    private float _startPos, _pos;
    private Rigidbody rb;


    private GameHandler gameHandler;
    [SerializeField] private CharacteristicHandler characteristicHandler;

    float fuelHealVolume = 0.3f;

    private Vector3 _roadVector;

    public bool reverse;

    public bool mobileController;

    private int motorTorqueCoeff;
    
    private int controllabilityCoeff;
    private int fuelTankSize;

    private float currentSpeed;

    private bool bonus;

    private float halfDisplayWidth;
    private float halfDisplayHeight;

    private DataBase dataBase;

    private bool soundOn, vibrationOn;



    [Header("UI elements")]
    private GameObject turnArrowLeft;
    private GameObject turnArrowRight;

    private bool leftTurn, rightTurn, noTurn;

    private void Start()
    {
        dataBase = GameObject.Find("DataController").GetComponent<DataBase>();
        characteristicHandler = GameObject.Find("Player").GetComponent<CharacteristicHandler>();
        if (SceneManager.GetActiveScene().name != "Training") gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        rb = GetComponent<Rigidbody>();
        _roadVector = rb.transform.forward;
        reverse = false;
        soundOn = SettingsButtons.soundOn;
        vibrationOn = SettingsButtons.vibrationOn;

        turnArrowLeft = GameObject.Find("TurnArrows").transform.Find("Left").gameObject;
        turnArrowRight = GameObject.Find("TurnArrows").transform.Find("Right").gameObject;


        if (Application.isMobilePlatform)
        {
            mobileController = true;
            Resolution currentResolution = Screen.currentResolution;

            halfDisplayWidth = currentResolution.width / 2;
            halfDisplayHeight = currentResolution.height / 2;

        }

        

        motorTorqueCoeff = characteristicHandler.Get_characteristic_inf(gameObject.name, "\'motorTorqueCoeff\'");
        
        fuelTankSize = characteristicHandler.Get_characteristic_inf(gameObject.name, "\'FuelTankSize\'");
        controllabilityCoeff = characteristicHandler.Get_characteristic_inf(gameObject.name, "\'controllabilityCoeff\'");

        string name = gameObject.name;
        name = name.Replace("(Clone)", "");
        /// Setting params
        foreach (var wheel in wheels)
        {
            int _springCoeff, _damperCoeff, _stiffnessFF, _stiffnessSF, _coeff;
            _coeff = int.Parse(dataBase.Get_data_int(name, "value", "characteristic", "\'controllabilityCoeff\'"));

            JointSpring _js = wheel.wheelCollider.suspensionSpring;
            
            _js.spring = wheel.wheelCollider.suspensionSpring.spring * (1 + 0.1f * _coeff);
            
            _js.damper = wheel.wheelCollider.suspensionSpring.damper * (1 + 0.02f * _coeff);
            
            wheel.wheelCollider.suspensionSpring = _js;

            WheelFrictionCurve _fFriction = wheel.wheelCollider.forwardFriction;
            WheelFrictionCurve _sFriction = wheel.wheelCollider.sidewaysFriction;

            _fFriction.stiffness += _coeff * 0.03f;
            _sFriction.stiffness += _coeff * 0.03f;

            wheel.wheelCollider.forwardFriction = _fFriction;
            wheel.wheelCollider.sidewaysFriction = _sFriction;

            

        }




        speed = int.Parse(dataBase.Get_data_int(name, "value", "characteristic", "\'startSpeed\'")) * (motorTorqueCoeff*0.05f+1);

        
        
    }


    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if ((_touch.phase == TouchPhase.Stationary | _touch.phase == TouchPhase.Began | _touch.phase == TouchPhase.Moved) & (_touch.position.y <= halfDisplayHeight))
            {
               

                if (_touch.position.x >= halfDisplayWidth)
                {
                    inputX += 0.05f;
                    if (!rightTurn)
                    {
                        ///Change turn arrows 
                        turnArrowRight.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                        turnArrowRight.GetComponent<Image>().color = new Color32(0, 0, 0, 81);

                        turnArrowLeft.transform.localScale = new Vector3(1f, 1f, 1f);
                        turnArrowLeft.GetComponent<Image>().color = new Color32(225, 225, 225, 81);
                        rightTurn = true;
                        leftTurn = false;
                        noTurn = false;
                    }
                    
                }
                if (_touch.position.x < halfDisplayWidth)
                {
                    inputX -= 0.05f;

                    if (!leftTurn)
                    {
                        ///Change turn arrows 
                        turnArrowLeft.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                        turnArrowLeft.GetComponent<Image>().color = new Color32(0, 0, 0, 81);

                        turnArrowRight.transform.localScale = new Vector3(1f, 1f, 1f);
                        turnArrowRight.GetComponent<Image>().color = new Color32(225, 225, 225, 81);

                        leftTurn = true;
                        rightTurn = false;
                        noTurn = false;
                    }

                    

                }

                if (inputX > 1) inputX = 1;
                if (inputX < -1) inputX = -1;


                //if (inputY < 1 & inputY > -1)
                //{
                //    if (!reverse) inputY += 0.03f;
                //    else inputY -= 0.03f;
                //}
                //Move();
                //Turn();
                //AnimatWheels();
               
            }

            


        }


        if (inputY < 1 & inputY > -1)
        {
            if (!reverse) inputY += 0.03f;
            else inputY -= 0.03f;
        }


        if (Input.touchCount == 0)
        {
            if (!noTurn)
            {
                ///Change turn arrows 
                turnArrowLeft.transform.localScale = new Vector3(1f, 1f, 1f);
                turnArrowLeft.GetComponent<Image>().color = new Color32(225, 225, 225, 81); ;

                turnArrowRight.transform.localScale = new Vector3(1f, 1f, 1f);
                turnArrowRight.GetComponent<Image>().color = new Color32(225, 225, 225, 81);

                leftTurn = false;
                rightTurn = false;
                noTurn = true;
            }

            if (inputX > 0 & inputX < 0.1f)
            {
                inputX -= 0.1f;
            }
            if (inputX < 0 & inputX > 0.1f)
            {
                inputX += 0.1f;
            }
            else
            {
                inputX = 0;
            }
        }




        if (!mobileController)
        {
            Get_inputs();
            Move();
            Turn();
            AnimatWheels();
        }

        Move();
        Turn();
        AnimatWheels();
    }

    public void Set_reverse(bool _value)
    {
        reverse = _value;
        inputY = 0;
    }

    public float Current_speed()
    {
        float _speed;
        _speed = rb.velocity.magnitude * 3.6f;

        return _speed;
    }

    private void Move()
    {
        currentSpeed = Current_speed();
        

        foreach (var wheel in wheels)
        {

            if (wheel.axel == Axel.Back)
            {
                if (currentSpeed < 30 & !reverse)
                {
                    wheel.wheelCollider.motorTorque = inputY * 250 * (motorTorqueCoeff * 0.1f + 1) * 3;
                }
                if (currentSpeed < 30 & reverse)
                {
                    wheel.wheelCollider.motorTorque = inputY * 250 * (motorTorqueCoeff * 0.1f + 1);
                }
                if (currentSpeed >= 30)
                {
                    wheel.wheelCollider.motorTorque = inputY * 250 * (motorTorqueCoeff * 0.1f + 1);
                }
                if (currentSpeed >= speed)
                {
                    wheel.wheelCollider.motorTorque = 0;
                }
            }





        }
    }

    private void Get_inputs()
    {
        inputX = Input.GetAxis("Horizontal");
        
        //inputY = Input.GetAxis("Vertical");
        
    }

    private void Turn()
    {
        
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
                if (_steerAngle > 45) _steerAngle = 45;
                if (_steerAngle < -45) _steerAngle = 45;
                
                wheel.wheelCollider.steerAngle = _steerAngle;



            }
        }
    }

    private void AnimatWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;

            wheel.wheelCollider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;

        }
    }
    private void Set_wheel_characteristics()
    {
        int _controllabilityCoeff;

        _controllabilityCoeff = characteristicHandler.Get_characteristic_inf(gameObject.name, "\'controllabilityCoeff\'");
        

        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;

            //wheel.wheelCollider.

        }
    }

    private void Test_input_t()
    {
        if (Input.touchCount > 0)
        {
            float _startPosX = 0;
            
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Beagn touch");
                    _startPosX = touch.position.x;
                    break;

                case TouchPhase.Moved:
                    if (touch.position.x < _startPosX)
                    {
                        Debug.Log(string.Format("startPos in Move = {0}", _startPosX));
                        inputX = 0.01f * Time.deltaTime;
                    }
                    else
                    {
                        inputX = -1 * 0.01f * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Ended:
                    _startPosX = 0;
                    break;
            }


        }

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameHandler.isGameOver)
        {
            if (other.gameObject.tag == "Fuel Tank")
            {

                gameHandler.Take_healing(fuelHealVolume);
                if (vibrationOn)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        Vibration.Init();
                        Vibration.Vibrate(100);
                    }

                    else if (Application.platform == RuntimePlatform.IPhonePlayer) Vibration.VibratePeek();
                }




                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "Coin")
            {
                gameHandler.Take_coin();
                if (vibrationOn & Application.platform == RuntimePlatform.Android)
                {
                    Vibration.Init();
                    Vibration.Vibrate(100);
                }
                else if (vibrationOn & Application.platform == RuntimePlatform.IPhonePlayer) Vibration.VibratePeek();
                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "Obstruction")
            {
                if (vibrationOn) { }
                Handheld.Vibrate();

                var _test = 0.1f - (0.015f * fuelTankSize);

                Debug.Log(string.Format("FtSize = {0}", fuelTankSize));
                gameHandler.Take_damage(0.1f - (0.005f * fuelTankSize));

            }
        }

        

        
    }

    

    
}
