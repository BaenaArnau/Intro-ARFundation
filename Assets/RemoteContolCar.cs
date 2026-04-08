using UnityEngine;


public class Coche : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float acceleration;
    public float maxSpeed;
    private float curSpeed;
    public float turnSpeed;
    public bool doAccelerate;
    private Rigidbody rig;


    public static Coche instance;


    void Awake()
    {
        instance = this;
        rig = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        if(doAccelerate)
        {
            curSpeed = Mathf.Clamp(curSpeed + (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }
        else
        {
            curSpeed = Mathf.Clamp(curSpeed - (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }


        rig.linearVelocity = transform.forward * curSpeed;
    }


    public void Turn (float rate)
    {
        transform.Rotate(Vector3.up, rate * turnSpeed * Time.deltaTime);
    }
}

