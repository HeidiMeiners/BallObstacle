using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public float force = 5f;
    public float angular = 20f;
    public GameObject ball;
    public GameObject plane;
    public TextMeshProUGUI text;
    public TextMeshProUGUI win;
    public KeyDirection[] keys;
    public KeyDirection jump;

    private Rigidbody _rigidbody;
    private Vector3 _torque = Vector3.zero;
    private Vector3 _jump = Vector3.zero;
    private float coins = 0;
    private GameObject[] coinsLeft;
    private GameObject[] pilars;
    private Boolean rotation = true;
    private List<Vector3> positionPilars=new List<Vector3>();

    [System.Serializable]
    public struct KeyDirection
    {
        public KeyCode key;
        public Vector3 direction;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.maxAngularVelocity = angular;

        coinsLeft = GameObject.FindGameObjectsWithTag("Coin");

        pilars = GameObject.FindGameObjectsWithTag("Pilars");

        text.text = "Coins collected: " + coins;

        foreach (GameObject pilar in pilars)
        {
            positionPilars.Add(new Vector3(pilar.transform.position.x, pilar.transform.position.y, pilar.transform.position.z));
        }
    }

    void Update()
    {
        if(rotation)
        {
            if(plane.transform.rotation == Quaternion.Euler(new Vector3 (12,0,0))) {
                rotation = false;
            }
            plane.transform.Rotate((1 * Time.deltaTime), 0, 0);
        }
        else
        {
            if(plane.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
            {
                rotation = true;
            }
            plane.transform.Rotate((1 * Time.deltaTime), 0, 0);
        }

        if (ball.transform.position.y < -10)
        {
            ball.transform.position = new Vector3(1,2,1);
        }

        if(coins == 3)
        {
            text.text = "";
            win.text = "You win!!";

        }

        foreach (KeyDirection key in keys)
        {
            if (Input.GetKey(key.key))
            {
                _torque += key.direction;
            }
        }

        if (Input.GetKeyDown(jump.key))
        {
            _jump += jump.direction;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddTorque(_torque * speed, ForceMode.Force);
        _torque = Vector3.zero;

        _rigidbody.AddForce(_jump * force, ForceMode.Impulse);
        _jump = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            coins += 1;
            text.text = "Coins collected: " + coins;
        }
    }

    public void click()
    {

        foreach (GameObject coin in coinsLeft)
        {
            coin.SetActive(true);
        }

        int n = 0;
        foreach (GameObject pilar in pilars)
        {
            pilar.transform.position=positionPilars[n];
            pilar.transform.rotation=Quaternion.identity;
            n++;
        }

        coins = 0;

        text.text = "Coins collected: " + coins;

        ball.transform.position = new Vector3(1, 2, 1);

        plane.transform.rotation=Quaternion.identity;

        
    }
}
