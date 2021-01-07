﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int timeRemaining;
    private Animator animator;
    
    public float moveTime = 0.1f;
    public int velocidad;
    public LayerMask blockinLayer; // per veure si el espai on ens movem esta ocupat o no 

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    public Joystick joy;

    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        timeRemaining = GameManager.instance.playerTime;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        velocidad = 5;
        joy = GameObject.FindWithTag("Joystick").GetComponent(typeof(Joystick)) as Joystick;

    }

    private void OnDisable()
    {
        GameManager.instance.playerTime = timeRemaining;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = 0;
        float vertical = 0;

        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        //Con joystick

        horizontal = joy.Horizontal;
        vertical = joy.Vertical;




        
        if (horizontal != 0 || vertical != 0)//mirem si ens estem intentant moure
        {
            //if (horizontal != 0)
               // vertical = 0;
            //if (vertical != 0)
               // horizontal = 0;

            StartCoroutine(Movement(new Vector3(horizontal, vertical, 0f)));

            /*Move(horizontal, vertical);
            if (hit.transform != null)
                Touch(hit);*/
        }

        //fEM SALTAR ELS TRIGGERS PER LES ANIMACIONS DEL PERSONATGE

        if (horizontal < 0)
        {
            animator.SetBool("Left",true);
            animator.SetBool("Right", false);
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);

        }
        else if (horizontal > 0)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
        }
        else if (vertical > 0)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
        }
        else if (vertical < 0)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);

        }



    }

    protected IEnumerator Movement(Vector3 inputplayer)
    {
        rb2D.MovePosition(GetComponent<Transform>().position + inputplayer * velocidad * Time.deltaTime);

        yield return null;
    }
    /*public void Touch(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Enemy")
        {
            LooseTime(hit.collider.damage);
        }
            

            Debug.Log(hit.collider.name);
    }*/
    public void LooseTime(int timeLost)
    {
        GameManager.instance.playerTime = GameManager.instance.playerTime - timeLost;
        //Saltar animacion de daño
        Debug.Log(GameManager.instance.playerTime);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {

        if(collision.gameObject.tag == "Cleaner")
        {
            LooseTime(10);//Daño de la limpieza
        }
        if(collision.gameObject.tag == "Thief")
        {
            //nos roba algo del inventario
        }
        if (collision.gameObject.tag == "Shopper")

        {
            //Nos hace perder mucho tiempo y podemos hacer que salgo algo de texto


        }
    }

}
