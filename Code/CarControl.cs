using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    public static float move;
    Rigidbody2D myRB;
    public Rigidbody2D fWheelRB;
    WheelJoint2D  bWheel;
    JointMotor2D driverMotor;
    public GameObject[] terrains;
    GameObject terrain;
    public static Vector3 terrainPoint;
    public static float perlinSeed=0;
    public static float referenceY;
    public static Vector3 carPos;
    float motorSpeed;
    public static float initX;
    float speedfactor=1;
    bool reverse = false;
    public bool isTruck = false;
    public static bool gameOver = false;
    public static float fuelAmt;
    public static int currentBaseScore;
    public static float bonusScore = 0;
    public static float airTime=0;
    bool baseGrounded;
    AudioSource engineSFX;
    AudioSource brakeSound;
    bool braked = false;
    public static bool won = false;

    



    public PhysicsMaterial2D carTyre;
    


    
    


    // Start is called before the first frame update
    void Start()
    {
        GetComponents<AudioSource>()[2].volume = 0.3f;
        engineSFX = GetComponents<AudioSource>()[0];
        brakeSound = GetComponents<AudioSource>()[1];
        
        engineSFX.volume = 0.5f;
        baseGrounded = false;
     
        airTime = -1;

        currentBaseScore = 0;
        bonusScore = 0;
        initX = transform.position.x;
        
        terrain = terrains[UIScript.level];
        bWheel = GetComponents<WheelJoint2D>()[0];
        WheelJoint2D fWheelJoint = GetComponents<WheelJoint2D>()[1];
        myRB = GetComponent<Rigidbody2D>();
        fuelAmt = 100;
        JointSuspension2D wheelSuspension = bWheel.suspension;
        wheelSuspension.frequency += 0.3f * UIScript.suspensionLevel[UIScript.vehicle];
        bWheel.suspension = wheelSuspension;
        
       fWheelJoint.suspension = wheelSuspension;
        driverMotor = bWheel.motor;
        driverMotor.motorSpeed += 100 * (UIScript.engineLevel[UIScript.vehicle]);
        bWheel.motor = driverMotor;
       carTyre.friction += 0.15f * UIScript.tyreLevel[UIScript.vehicle];
        if (UIScript.level == 2)
        {
            myRB.gravityScale = 0.166f;
        }
        else
        {
            myRB.gravityScale = 1f;
        }

        driverMotor = bWheel.motor;
        motorSpeed = driverMotor.motorSpeed;

        if (UIScript.vehicle==1)
        {
            speedfactor= 1.4f;
        }
        else
        {
            speedfactor = 1;
        }
     
        

    }
    public void accel()
    {
        move = 1;
    }

    // Update is called once per frame
    float initAirTime = 0;
    void FixedUpdate()
    {
     
        //move = -Input.GetAxisRaw("Horizontal");
        if (gameOver)
        {
            move = 0;
        }
        if (Mathf.Abs(move) > 0.5f)s
        {
            if (move < 0)
            {
                Debug.Log("DASDAS");

                //driverMotor.motorSpeed = Mathf.Lerp(driverMotor.motorSpeed,speedfactor * move * Mathf.Abs(motorSpeed),Time.fixedDeltaTime*8f);
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
                if (move < 0 )
                    myRB.AddTorque(-move * 35);
                else
                {
                    if (!reverse)
                        fWheelRB.angularVelocity = 0;
                    else
                        fWheelRB.angularVelocity = 1000f;
                    if (UIScript.vehicle == 1)
                    {
                        myRB.AddTorque(-move * 45);
                    }
                    else
                    {

                        myRB.AddTorque(-move * 45);

                    }
                }
            }


        }
        else
        {
            bWheel.useMotor = false;

        }

        TyreParticleContro frontTyre = GetComponentsInChildren<TyreParticleContro>()[0];
        TyreParticleContro backTyre = GetComponentsInChildren<TyreParticleContro>()[1];
        if (move == 1 && !braked && myRB.velocity.x>4f && (frontTyre.grounded||backTyre.grounded))
        {
            brakeSound.Play();
            braked = true;
        }
        if(move==-1 || move == 0)
        {
            braked = false;
        }
       
        if ((move == -1)||(move==1&&reverse))
        {
            if (UIScript.vehicle == 1)
                engineSFX.pitch = Mathf.Lerp(engineSFX.pitch, 3.6f, Time.deltaTime/2);

            else
            engineSFX.pitch = Mathf.Lerp(engineSFX.pitch, 2.2f, Time.deltaTime);
            

            
        }
        else
        {
            if (UIScript.vehicle == 1)
                engineSFX.pitch = Mathf.Lerp(engineSFX.pitch, 1.2f, Time.deltaTime);

            else
                engineSFX.pitch = Mathf.Lerp(engineSFX.pitch, 0.7f, Time.deltaTime);
          
        }
        if (!UIScript.raceMode)
        {
            if (frontTyre.grounded || backTyre.grounded || baseGrounded)
            {

                if (airTime >= 0)
                {

                    airTime = -1;
                }

            }
            else
            {

                if (airTime < 0)
                {
                    initAirTime = Time.time;
                }
                airTime = Time.time - initAirTime;

            }
        }

      
        
        carPos = transform.position;
        if(currentBaseScore< (int)(carPos.x - initX) / 4 && !UIScript.raceMode)
        currentBaseScore = (int)(carPos.x - initX)/4;
        
        if (fuelAmt <= 0)
        {
            gameOver = true;
           
        }
        if (gameOver)
        {
            move = 0;
        }
        
       

        if (terrainPoint.x - transform.position.x < 300)
        {
         
           
            Instantiate(terrain, terrainPoint, Quaternion.identity);
        }
        if (move == 0)
        {
            fuelAmt -= 0.05f;
        }
        else if(move<0 || (move>0 && reverse))
        {
            fuelAmt -= 0.1f;

        }
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("ground")|| collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            baseGrounded = true;
        }
       
    }
    
    

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            baseGrounded = false;
        }
    }








}
