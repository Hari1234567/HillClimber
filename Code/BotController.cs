using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    // Start is called before the first frame update
    WheelJoint2D bWheel;
    public Rigidbody2D fWheelRB;
    Rigidbody2D myRB;
    bool reverse = false;
    float speedfactor = 1;
    float motorSpeed;
    public bool isTruck=false;
    float move;
    JointMotor2D driverMotor;
    public float feedbackFactor=0.8f;

    TyreParticleContro frontTyre, backTyre;
    public GameObject test;
    AudioSource engineSFX, brakeSound;
    public static float botX;
    bool braked;
    public static bool botDead=false;
    public static bool won;

    void Start()
    {
        botDead = false;
        frontTyre = GetComponentsInChildren<TyreParticleContro>()[0];
        backTyre = GetComponentsInChildren<TyreParticleContro>()[1];
        GetComponents<AudioSource>()[2].volume = 0.3f;
        engineSFX = GetComponents<AudioSource>()[0];
        brakeSound = GetComponents<AudioSource>()[1];
        engineSFX.volume = 0.5f;
        bWheel = GetComponents<WheelJoint2D>()[0];
        frontTyre.grounded = false;
        backTyre.grounded = false;
        myRB = GetComponent<Rigidbody2D>();
        driverMotor = bWheel.motor;
        motorSpeed = driverMotor.motorSpeed;
        botX = transform.position.x;
        move = 0;
        braked = false;
      
        Collider2D[] botColliders = GetComponentsInChildren<Collider2D>();
        Collider2D[] playerColliders = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Collider2D>();

        for(int i = 0; i < botColliders.Length; i++)
        {
            Physics2D.IgnoreCollision(botColliders[i], playerColliders[i]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        botX = transform.position.x;
        float zAngle = transform.eulerAngles.z;

        if (!botDead)
        {
            if (UIScript.vehicle == 1)
            {
                speedfactor = 1.6f;
            }
            move = -1;
            if (transform.rotation.eulerAngles.z > 60 && transform.rotation.eulerAngles.z < 320)
            {
                move = 1;
                if (Mathf.Abs(myRB.angularVelocity) < 100)
                    myRB.AddTorque(-40);
            }
            else if (transform.rotation.eulerAngles.z < 330 && transform.rotation.eulerAngles.z > 180)
            {
                move = -1;
                if (Mathf.Abs(myRB.angularVelocity) < 100)
                {
                    if (!frontTyre.grounded)
                    {
                        fWheelRB.angularVelocity = 0;
                        move = 1;
                    }
                    myRB.AddTorque(40);
                }
            }
            if (UIScript.level == 2)
            {
                myRB.gravityScale = 0.166f;
            }





            if (Mathf.Abs(move) > 0.5f)
            {
                if (move < 0)
                {
                    driverMotor.motorSpeed = speedfactor * move * Mathf.Abs(motorSpeed);
                }
                else
                {
                    if (myRB.velocity.x > 2)
                    {
                        reverse = false;
                        driverMotor.motorSpeed = 0;
                    }
                    else
                    {
                        reverse = true;

                        driverMotor.motorSpeed = speedfactor * move * Mathf.Abs(motorSpeed);
                    }
                }
                bWheel.motor = driverMotor;
                bWheel.useMotor = true;
                if (Mathf.Abs(myRB.angularVelocity) < 100)
                {
                    if (move < 0) { }
                    // myRB.AddTorque(-move * 35);
                    else
                    {
                        if (!reverse)
                            fWheelRB.angularVelocity = 0;
                        else
                            fWheelRB.angularVelocity = 1000f;
                        if (UIScript.vehicle == 1)
                        {
                            // myRB.AddTorque(-move * 45);
                        }
                        else
                        {
                            //myRB.AddTorque(-move * 45);
                        }
                    }
                }


            }
            else
            {
                bWheel.useMotor = false;

            }

        }
    }
}
