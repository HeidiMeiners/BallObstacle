using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public float force = 5f;
    public float angular = 20f;
    public GameObject ball;
    public TextMeshProUGUI text;
    public TextMeshProUGUI win;
    public KeyDirection[] keys;
    public KeyDirection jump;

    private Rigidbody _rigidbody;
    private Vector3 _torque = Vector3.zero;
    private Vector3 _jump = Vector3.zero;
    private float coins = 0;
    private GameObject[] coinsLeft;

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

        text.text = "Coins collected: " + coins;
    }

    void Update()
    {
        if(ball.transform.position.y < 0)
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

        coins = 0;

        text.text = "Coins collected: " + coins;

        ball.transform.position = new Vector3(1, 2, 1);
    }
}
