using UnityEngine;


public class Coche : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float acceleration;
    public float maxSpeed;
    private float _curSpeed;
    private float _currentTurnRate;
    public float turnSpeed = 180f;
    public bool doAccelerate;
    private Rigidbody _rig;


    public static Coche Instance;


    void Awake()
    {
        Instance = this;
        _rig = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        if(doAccelerate)
        {
            _curSpeed = Mathf.Clamp(_curSpeed + (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }
        else
        {
            _curSpeed = Mathf.Clamp(_curSpeed - (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }


        if (_currentTurnRate != 0f)
        {
            transform.Rotate(0f, _currentTurnRate * turnSpeed * Time.deltaTime, 0f);
        }

        _rig.linearVelocity = transform.forward * _curSpeed;

        _currentTurnRate = 0f; // Reset cada frame
    }


    public void Turn (float rate)
    {
        _currentTurnRate = rate;
    }
}

