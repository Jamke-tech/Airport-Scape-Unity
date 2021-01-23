using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopper : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private string[] directions = { "right", "up", "left", "down" };
    private float tiempo;
    RaycastHit2D objectHit;
    private int pos;
    private  int horizontal;
    private int vertical;
    public LayerMask blockinLayer;
    private SpriteRenderer spriteR;
    public Sprite[] sprites;
    public string[] vectorOfQuotes = { "Have you seen this new IPAD?? It's only 20 bugs", "Ei!! you, are you interested in Bitcoins (You have spend 10 bugs on BTC)", "Come hear and tastes our home made cookies!! (You have spend 15 Bugs)" };
    public int[] moneySpend = { 20, 10, 15 };
    public int timeSpend = 15;

    public bool interaction;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        tiempo = 3;
        pos = 0;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        interaction= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.doingSetup)
        {
            tiempo = tiempo - 1 * Time.deltaTime;
            if (tiempo >= 0)
            {
                string direction = directions[pos];
                spriteR.sprite = sprites[pos];

                if (direction == "right")
                {
                    horizontal = 1;
                    vertical = 0;

                }
                else if (direction == "up")
                {
                    horizontal = 0;
                    vertical = 1;

                }
                else if (direction == "left")
                {
                    horizontal = -1;
                    vertical = 0;
                }
                else
                {
                    horizontal = 0;
                    vertical = -1;
                }

                objectHit = Watch(5 * horizontal, 5 * vertical);

                if (objectHit != false && !interaction)
                {
                    if (objectHit.collider.tag == "Player")
                    {
                        interaction = true;
                        int numRand = Random.Range(0, vectorOfQuotes.Length);
                        GameManager.instance.PlayerSeen(vectorOfQuotes[numRand], moneySpend[numRand], timeSpend, gameObject, tiempo);
                    }
                }


            }
            else
            {
                if (pos < directions.Length - 1)
                {
                    pos++;
                }
                else
                {
                    pos = 0;
                }

                tiempo = 3;
                //Cambiamos el sprite
            }
        }

    }
    public RaycastHit2D Watch(int xCells, int yCells)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xCells,yCells);
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockinLayer);
        boxCollider.enabled = true;
        return hit;
    }
}
