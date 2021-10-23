using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class megaman : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpforce;
   // [SerializeField] float dashForce;
    //[SerializeField] float dashSpeed;

    [SerializeField] BoxCollider2D misPies;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    
    float movDash = 1;
    int salto = 1;
    private float direccion;
    private bool sale;
    private bool alojandodireccion = true;
    private float dashActual;
    private bool canDash;
    public float duracionDash;
    public float dashSpeed;
    public float dashcooldown;

   





    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myBody.velocity = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        correr();
        saltar();
        caer();
        Disparar();
        dash();
    }
    void correr()
    {
        float movH = Input.GetAxis("Horizontal");
        if (movH != 0)
        {
            if (movH < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }

            myAnimator.SetBool("run", true);
            transform.Translate(new Vector2(movH * Time.deltaTime * speed, 0));
            movDash = movH;
        }
        else
        {
            myAnimator.SetBool("run", false);
        }

    }




    void saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo() && salto == 0)
            {
                salto = 1;
            }
            else if (enSuelo() && salto == 2)
            {
                salto = 0;
            }
            else if (!enSuelo() && salto == 0)
            {
                salto = 0;
            }
            else if (!enSuelo() && salto == 1)
            {
                salto = 0;
            }


            {
                if (enSuelo() && salto == 1)
                {
                    saltarHeroe();
                    salto = 2;
                }
                else if (!enSuelo() && salto == 2)
                {
                    saltarHeroe();
                    salto = 0;
                }
            }
        }
    }

    void saltarHeroe()
    {
        myBody.velocity = new Vector2(0, 0);
        myBody.AddForce(new Vector2(0, jumpforce));
        myAnimator.SetTrigger("jump");
    }

    void caer()
    {
        if (myBody.velocity.y < 0)
        {
            myAnimator.SetBool("fall", true);
        }
        else
        {
            myAnimator.SetBool("fall", false);
        }
    }

    bool enSuelo()
    {
        return (misPies.IsTouchingLayers(LayerMask.GetMask("ground")));
    }

    void dash()
    {
        if (Input.GetKeyDown(KeyCode.X)&& sale &&canDash)
        {
            if (dashActual<=0)
            {
                stopdash();
            }
            else
            {
                myAnimator.SetBool("dash", true);
                dashActual -= Time.deltaTime;
                if (alojandodireccion)
                    myBody.velocity = Vector2.right * dashSpeed;
                else
                    myBody.velocity = Vector2.left * dashSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetBool("dash", false);
            canDash = true;
            dashActual = duracionDash;
        }
       /* --------------codigo previo al que esta , el que servia en clase 
        * float dashForceGo = 0;
        if (Input.GetKey(KeyCode.X) && enSuelo() && movDash != 0)
        {

            if (movDash < 0)
            {
                dashForceGo = -dashForce;
            }
            else
            {
                dashForceGo = dashForce;
            }
            myBody.AddForce(new Vector2(dashForceGo, 0));
            this.myAnimator.SetTrigger("dash");
        }
       
       
        }*/

    }
    void stopdash()
    {
        myBody.velocity = Vector2.zero;
        dashActual = duracionDash;
        myAnimator.SetBool("dash", false);
        canDash = false;


    }
    

   

    void Disparar()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.myAnimator.SetLayerWeight(1, 1);
        }
    }
}