using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class megaman : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpforce;
    //[SerializeField] float dashForce;
    //[SerializeField] float dashForceMin;
    //[SerializeField] float dashForceU;
    //[SerializeField] float duracionDash;

    [SerializeField] BoxCollider2D misPies;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;

    bool inicioSuelo;
    int salto = 1;
    

    //-----------------------------------------------------------mecanica dash

    IEnumerator dashCoroutine;
    float movDash = 1;
    bool isDashing;
    bool canDash=true;
    private float direccion = 1;
    float gravedadNormal;
    


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myBody.velocity = new Vector2(0, 0);
        gravedadNormal = myBody.gravityScale; // mecanica del dash
        
      


    }

    // Update is called once per frame
    void Update()
    {
        correr();
        saltar();
        caer();
        enSuelo();
        if(movDash!=0)
        {
            direccion = movDash;
        }
        if(Input.GetKeyDown(KeyCode.X)&&canDash==true&&enSuelo())
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(0.1f, 0);
            StartCoroutine(dashCoroutine);
            myAnimator.SetBool("dash", true);
        }
        if(Input.GetKey(KeyCode.X) && canDash == true && enSuelo())
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(0.3f, 1);
            StartCoroutine(dashCoroutine);
            myAnimator.SetBool("dash", true);
        }

    }
    private void FixedUpdate()
    {
        //algunos arreglos codigo dash (en especifico aqui es la fuerza de empuje)
        if (isDashing)
        {
            myBody.AddForce(new Vector2(direccion*5, 0), ForceMode2D.Impulse);
        }
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
            movDash = movH;// importante para que el dash sirva 
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
            enSuelo();
            if (inicioSuelo == true && salto == 1)
            {
                saltarHeroe();
                salto = 2;
            }
            else if (inicioSuelo == true && salto == 2)
            {
                saltarHeroe();
                salto = 1;
                inicioSuelo = false;
            }




        }
    }

    void contadorSalto()
    {
        salto = 0;
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
        if (misPies.IsTouchingLayers(LayerMask.GetMask("ground")))
        {
            inicioSuelo = true;
        }
        else
        {
            inicioSuelo = false;
        }

        return inicioSuelo;
    }
     /*concatenador para que sirva el dash donde se definen los parametros para modificarlos
        arriba de la duracion y el cool down del dash*/
    IEnumerator Dash(float duracionDash,float dashCooldown)
    {
        Vector2 velocidadOriginal = myBody.velocity;
        isDashing = true;
        canDash = false;
        myAnimator.SetBool("dash", true);
        myBody.gravityScale = 0;
        myBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(duracionDash);
        isDashing = false;
        myAnimator.SetBool("dash", false);
        myBody.gravityScale = gravedadNormal;
        myBody.velocity = velocidadOriginal;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }





}