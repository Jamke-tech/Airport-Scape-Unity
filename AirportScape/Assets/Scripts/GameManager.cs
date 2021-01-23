using System;
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
    private GameObject dialogImage;
    private Text questionText;
    private int numeroQuestion = 0;

    private List<string> questionAvailable = new List<string>();

    List<string> questions = new List<string>();


    //Parametros del juego
    public BoardManager boardScript;
    public CameraFollow follwingCamera;
    public static GameManager instance = null;
    private int level; //Pedir al usuario la zona 
    public int playerTimeAvailable = 120; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
    public int playerMoneyWin;
    private string playerName;
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
        questionAvailable.Add("Pregunta de Prueba");
        questionAvailable.Add("Pregunta de Prueba2");
        questionAvailable.Add("Pregunta de Prueba3");
        questionAvailable.Add("Pregunta de Prueba4");
        questionAvailable.Add("Pregunta de Prueba5");
        questionAvailable.Add("Pregunta de Prueba6");
        
        InitGame();

    }
    private void Start()
    {
        //Tenemos que poner las list de las preguntas

        //Ponemos los valores de el jugador
        level = 1; //Pedir al usuario la zona 
        playerTimeAvailable = 120; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
        playerMoneyWin = 1000;
        playerName = "Jaume Tabernero";

    }
    private void OnLevelWasLoaded(int level)
    {
        this.level++;

        //Pedir a android que guarde los parametros que tenemos ahora


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
        levelText = GameObject.Find("LevelText").GetComponent<Text>() as Text;
        nameUser = GameObject.Find("NameUser").GetComponent<Text>() as Text;
        timeUser = GameObject.Find("BoardingTime").GetComponent<Text>() as Text;
        from = GameObject.Find("From").GetComponent<Text>() as Text;

        if (level==1 || level==4 || level == 7)//boardng zone
        {
            levelText.text = "Entrance Level";
        }
        else if(level == 2 || level == 5 || level == 8)//Security zone
        {
            levelText.text = "Security Level";
        }
        else//last zone
        {
            levelText.text = "Boarding Level";
        }

        if (level <= 3)
        {
            from.text = "Frankfurt";
        }
        else if (level<=6 && level>3)
        {
            from.text = "NEW YORK";
        }
        else
        {
            from.text = "Moscow";
        }

        nameUser.text = "elJefe";
        timeUser.text = playerTimeAvailable.ToString();
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        
        textTime = GameObject.FindWithTag("Time").GetComponent(typeof(Text)) as Text;
        textMoney = GameObject.FindWithTag("Money").GetComponent(typeof(Text)) as Text;
        textInfo = GameObject.FindWithTag("Info").GetComponent(typeof(Text)) as Text;

        textTime.text = playerTimeAvailable.ToString();
        textMoney.text = playerMoneyWin.ToString();
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
            textTime.text = playerTimeAvailable.ToString();
            textMoney.text = playerMoneyWin.ToString();
            textInfo.text = enemyQuote;

            if (playerTimeAvailable <= 0)
                GameOver();


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
                playerTimeAvailable = playerTimeAvailable - 1;
                tiempo = 5;
            }
            else
            {
                tiempo = tiempo - 1 * Time.deltaTime;
            }

            if(playerTimeAvailable<=0)
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
        playerTimeAvailable = playerTimeAvailable - timespend;
        
        if (playerMoneyWin - moneyspend <= 0)
        {
            playerMoneyWin = 0;
        }
        else
        {
            playerMoneyWin = playerMoneyWin - moneyspend;
        }
    }

    public void SecurityOn()
    {
        doingSetup = true; //Lo ponemos para no permitir que el jugador entre i salga de seguridad
        dialogImage = GameObject.FindGameObjectWithTag("Dialog");
        dialogImage.SetActive(true);
        //inixializamos 5 de las preguntas del vector de preguntas que tenemos;
        List<string> questionToChoose = questionAvailable;
        for (int i=0; i<5; i++)
        {
            int randpos = UnityEngine.Random.Range(0, questionToChoose.Count);
            Debug.Log(randpos);
            questions.Add(questionToChoose[randpos]);
            questionToChoose.RemoveAt(randpos);

        }
        
        ShowQuestion(0);
    }

    public void ShowQuestion(int numQuestion)
    {
        questionText = GameObject.Find("Question").GetComponent<Text>() as Text;
        questionText.text = questions[numQuestion];
    }

    public void EndSecurity()
    {
        this.level++;
        dialogImage.SetActive(false);
        doingSetup = false;
        InitGame();

    }


    internal void YesPulsed()
    {
        //Hem de fer algo amb el temps per perdre de manera aleatoria segons supicious

        if (numeroQuestion + 1 < 5)
        {
            numeroQuestion++;
            ShowQuestion(numeroQuestion);
        }
        else
        {
            EndSecurity();
        }


    }

    internal void NoPulsed()
    {
        //Hem de fer algo per el temps de perdre de manera aleatoria temps

        
        
        
        
        if (numeroQuestion + 1 < 5)
        {
            numeroQuestion++;
            ShowQuestion(numeroQuestion);
        }
        else
        {
            EndSecurity();
        }



    }










}
