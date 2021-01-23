using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int timeRemaining;
    private Animator animator;
    
    public float moveTime = 0.1f;
    public float restartDelay = 1f;
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
        timeRemaining = GameManager.instance.playerTimeAvailable;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        velocidad = 30;
        joy = GameObject.FindWithTag("Joystick").GetComponent(typeof(Joystick)) as Joystick;
        

    }

    private void OnDisable()
    {
        GameManager.instance.playerTimeAvailable = timeRemaining;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.doingSetup)
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
                animator.SetBool("Left", true);
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



    }


    protected IEnumerator Movement(Vector3 inputplayer)
    {
        rb2D.MovePosition(GetComponent<Transform>().position + inputplayer * velocidad * Time.deltaTime);

        yield return null;
    }

    public void LooseTime(int timeLost)
    {
        GameManager.instance.playerTimeAvailable = GameManager.instance.playerTimeAvailable - timeLost;
        //Saltar animacion de daño
        Debug.Log(GameManager.instance.playerTimeAvailable);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {

        if(collision.gameObject.tag == "Cleaner")
        {
            GameManager.instance.PlayerSeen("Whatch your steps!!! WET FLOOR ", 0, 10, collision.gameObject, 5);
            SoundManager.instance.PlaySingleSound();


        }
        if(collision.gameObject.tag == "Thief")
        {

            GameManager.instance.PlayerSeen("JAJAJAJAJA I stole from you 10 Bugs", 10, 2, collision.gameObject, 5);
            SoundManager.instance.PlaySingleSound();

        }
        if (collision.gameObject.tag == "Shopper")

        {
            GameManager.instance.PlayerSeen("Ei!! Be Careful I'm here ", 0, 5, collision.gameObject, 5);
            SoundManager.instance.PlaySingleSound();


        }
        if (collision.gameObject.tag =="Secret")

        {
            GameManager.instance.PlayerSeen("You have discovered the secrets of this airport", 0, 0, collision.gameObject, 5);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Next")
        {
            //Invoke("NextLevel", restartDelay);
            Invoke("SecurityOn", restartDelay);
        }
        else if (other.tag == "Security")
        {
            Invoke("SecurityOn", restartDelay);
        }


   
    }
    private void NextLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void SecurityOn()
    {
        GameManager.instance.SecurityOn();
    }



    private void LooseMoney(int moneyLost)
    {
        GameManager.instance.playerMoneyWin = GameManager.instance.playerMoneyWin - moneyLost;
    }
}
