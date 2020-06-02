using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TyreParticleContro : MonoBehaviour
{
   
    Rigidbody2D myRB;
    
    public ParticleSystem dust;
    public Material[] particles;
    public bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        //grounded = true;
        myRB = GetComponent<Rigidbody2D>();
        if (dust != null)
        {
            dust.enableEmission = false;
            dust.GetComponent<ParticleSystemRenderer>().material = particles[UIScript.level];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (dust != null)
        {
            float slipFactor = Mathf.Abs(myRB.angularVelocity) / myRB.velocity.magnitude;

            if (slipFactor > 600)
            {
                dust.startSpeed = 40;
            }
            else
            {
                dust.startSpeed = 0;
            }
            if (myRB.velocity.x < 0)
            {
                dust.startSpeed = 0;
            }
            if (Mathf.Abs(myRB.angularVelocity) < 40 && myRB.angularVelocity < 60)
            {
                dust.enableEmission = false;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground")|| collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
           
            if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                dust.transform.position = (Vector3)collision.GetContact(0).point + new Vector3(0, 0, 10);
                if (dust != null)
                    dust.enableEmission = true;
            }
            grounded = true;     
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            grounded = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                if (dust != null)
                    dust.enableEmission = false;
            }

            grounded = false;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            grounded = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            grounded = true;
            dust.transform.position = (Vector3)collision.GetContact(0).point+new Vector3(0,0,10);
        }
    }
}
