using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class megaman : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpforce;
    [SerializeField] float dashForce;
    [SerializeField] int saltosMaximos;

    [SerializeField] BoxCollider2D misPies;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    int saltosRestantes;
    float movDash=1;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myBody.velocity = new Vector2(0, 0);
        saltosRestantes = saltosMaximos;
    }

    // Update is called once per frame
    void Update()
    {
        correr();
        saltar();
        caer();
        disparar();
        dash();
        procesarMovimiento();
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
        if (enSuelo())
        {
            saltosRestantes = saltosMaximos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            saltosRestantes = saltosRestantes - 1;
            myBody.velocity = new Vector2(0, 0);
            myBody.AddForce(new Vector2(0, jumpforce));
            myAnimator.SetTrigger("jump");
        }
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
        float dashForceGo = 0;
        if (Input.GetKeyDown(KeyCode.X) && enSuelo() && movDash!= 0)
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
            myAnimator.SetTrigger("dash");
        }
    }

    void disparar()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            myAnimator.SetLayerWeight(1, 1);
        }
    }
    void procesarMovimiento()
    {

    }








}

