using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Parametros de los transitions levels
    public float levelStartDelay = 3f;//time between levels
    private Text levelText;
    private Text nameUser;
    private Text timeUser;
    private Text from;
    private GameObject levelImage;
    public bool doingSetup;
    
    //Parametros del juego
    public BoardManager boardScript;
    public CameraFollow follwingCamera;
    public static GameManager instance = null;
    public int zone; //Pedir al usuario la zona 
    public int playerTime; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
    public int playerMoney;
    public string playerName;
    public float turnDelay = 0.1f;
    private List<Cleaner> cleaners;

    
    //Parametros de UI del jugador
    public Text textTime;
    public Text textMoney;
    public Text textInfo;

    public float tiempo;

    public string enemyQuote = "";



    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        cleaners = new List<Cleaner>();
        boardScript = GetComponent<BoardManager>();

        
        InitGame();

    }
    private void Start()
    {
        //Ponemos los valores de el jugador
        zone = 1; //Pedir al usuario la zona 
        playerTime = 120; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
        playerMoney = 1000;
        playerName = "Jaume Tabernero";

    }
    private void OnLevelWasLoaded(int level)
    {
        zone++;
        tiempo = 5;
        InitGame();
    }
    
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        nameUser = GameObject.Find("NameUser").GetComponent<Text>();
        timeUser = GameObject.Find("BoardingTime").GetComponent<Text>();
        from = GameObject.Find("From").GetComponent<Text>();

        if (zone==1 || zone==4 || zone == 7)//boardng zone
        {
            levelText.text = "Entrance Level";
        }
        else if(zone == 2 || zone == 5 || zone == 8)//Security zone
        {
            levelText.text = "Security Level";
        }
        else//last zone
        {
            levelText.text = "Boarding Level";
        }

        if (zone <= 3)
        {
            from.text = "Frankfurt";
        }
        else if (zone<=6 && zone>3)
        {
            from.text = "NEW YORK";
        }
        else
        {
            from.text = "Moscow";
        }

        nameUser.text = "elJefe";
        timeUser.text = playerTime.ToString();
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        
        textTime = GameObject.FindWithTag("Time").GetComponent(typeof(Text)) as Text;
        textMoney = GameObject.FindWithTag("Money").GetComponent(typeof(Text)) as Text;
        textInfo = GameObject.FindWithTag("Info").GetComponent(typeof(Text)) as Text;

        textTime.text = playerTime.ToString();
        textMoney.text = playerMoney.ToString();
        textInfo.text = "";


        cleaners.Clear();
        boardScript.Start();
        boardScript.SetUpScene();
    }

    IEnumerator MoveEnemies()
    {
       
        yield return new WaitForSeconds(turnDelay);//Fem un delay
        if (cleaners.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < cleaners.Count; i++)
        {
            cleaners[i].MoveCleaner();
            yield return new WaitForSeconds(cleaners[i].moveTime);
        }
        


    }
    public void AddCleanerToList(Cleaner script)
    {
        cleaners.Add(script);
    }

    // Update is called once per frame
    void Update()
    {
        if (!doingSetup)
        {
            StartCoroutine(MoveEnemies());
            textTime.text = playerTime.ToString();
            textMoney.text = playerMoney.ToString();
            textInfo.text = enemyQuote;
        }
       
    }
    
    public void GameOver()//Que hacer cuando finalize el juego
    {

    }
    
    private void FixedUpdate()
    {
        if (!doingSetup)
        {
            if (tiempo <= 0)
            {
                playerTime = playerTime - 1;
                tiempo = 5;
            }
            else
            {
                tiempo = tiempo - 1 * Time.deltaTime;
            }

            if(playerTime<=0)
            {
                GameOver();
            }



        }


    }
    IEnumerator ShowText(string quote, GameObject whoSees, float waitingTime)
    {

        if (whoSees.tag == "Shopper")
        {

            enemyQuote = quote;
            yield return new WaitForSeconds(waitingTime);//Fem un delay
            enemyQuote = "";
            Shopper shopperScript = whoSees.GetComponent(typeof(Shopper)) as Shopper;
            shopperScript.interaction = false;
        }
        else if (whoSees.tag == "Thief")
        {
            enemyQuote = quote;
            yield return new WaitForSeconds(waitingTime);//Fem un delay
            enemyQuote = "";
            Thief thiefScript = whoSees.GetComponent(typeof(Thief)) as Thief;
            thiefScript.interaction = false;

        }
        else if (whoSees.tag == "Cleaner")
        {
            enemyQuote = quote;
            yield return new WaitForSeconds(waitingTime);
            enemyQuote = "";
        }
        else
        {
            enemyQuote = quote;
            yield return new WaitForSeconds(waitingTime);
            enemyQuote = "";
        }



    }



    public void PlayerSeen(string Quote,int moneyspend,int timespend, GameObject whoSees, float tiempo)
    {
        StartCoroutine(ShowText(Quote, whoSees,tiempo));
        playerTime = playerTime - timespend;
        
        if (playerMoney - moneyspend <= 0)
        {
            playerMoney = 0;
        }
        else
        {
            playerMoney = playerMoney - moneyspend;
        }
    }










}
