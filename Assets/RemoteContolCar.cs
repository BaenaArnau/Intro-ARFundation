using UnityEngine;


public class Coche : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float acceleration;
    public float maxSpeed;
    private float curSpeed;
    private float currentTurnRate;
    public float turnSpeed = 180f;
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

        // Aplica rotación usando angularVelocity del Rigidbody
        if (currentTurnRate != 0f)
        {
            rig.angularVelocity = Vector3.up * (currentTurnRate * turnSpeed * Mathf.Deg2Rad);
        }
        else
        {
            rig.angularVelocity = Vector3.zero;
        }

        currentTurnRate = 0f; // Reset cada frame
    }


    public void Turn (float rate)
    {
        currentTurnRate = rate;
    }
}

