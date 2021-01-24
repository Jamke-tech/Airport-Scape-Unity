using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Parametros de los transitions levels
    public float levelStartDelay = 3f;//time between levels

    public int randomanswer;

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
    public int level; //Pedir al usuario la zona 
    public int playerTimeAvailable = 120; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
    public int playerMoneyWin;
    public int playerSuspicious;
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
        questionAvailable.Add("Do you live with your parents?");
        questionAvailable.Add("Would you be able to forgive the betrayal of a friend?");
        questionAvailable.Add("Do you feel that you are a brave person?");
        questionAvailable.Add("Have you ever had your hand or tongue stuck to something?");
        questionAvailable.Add("Have you ever kissed or been kissed in the rain?");
        questionAvailable.Add("Have you ever left the country?");
        questionAvailable.Add("Do you usually follow your brain more than the heart?");
        questionAvailable.Add("Have you ever fallen asleep at school or work?");
        questionAvailable.Add("Have your parents caught a lie of great caliber?");
        questionAvailable.Add("Have you got in the car of people you just met?");
        questionAvailable.Add("Do you fear snakes?");
        questionAvailable.Add("Have you ever been arrested?");
        questionAvailable.Add("Do you consider yourself an intelligent person?");
        questionAvailable.Add("Do you like DSA subject?");
        questionAvailable.Add("Have you ever been broken or have you broken your heart?");
        questionAvailable.Add("Do you know how to play at least one musical instrument?");
        questionAvailable.Add("Is programming the best activity in the world?");
        questionAvailable.Add("Would you share your last piece of food with me?");
        questionAvailable.Add("Do you usually be friends with your ex?");
        questionAvailable.Add("If it were possible to colonize Mars while we are still alive, would you go to a colony?");

        levelImage = GameObject.Find("LevelImage");

        levelText = GameObject.Find("LevelText").GetComponent<Text>() as Text;
        nameUser = GameObject.Find("NameUser").GetComponent<Text>() as Text;
        timeUser = GameObject.Find("BoardingTime").GetComponent<Text>() as Text;
        from = GameObject.Find("From").GetComponent<Text>() as Text;
        Debug.Log("Entro Awake");
        #if UNITY_ANDROID
            SetParameters();
        #else
            SetParametersComputer();
        #endif

        InitGame();

    }
    private void SetParameters()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        playerMoneyWin = int.Parse(intent.Call<String>("getStringExtra", "playerMoney"));
        playerSuspicious = int.Parse(intent.Call<String>("getStringExtra", "playerSuspicious"));
        level = int.Parse(intent.Call<String>("getStringExtra", "level"));
        playerName = intent.Call<String>("getStringExtra", "playerName");

        //Tenemos que poner las list de las preguntas
        Debug.Log("Entro Start");
        //Ponemos los valores de el jugador
        level = 1; //Pedir al usuario la zona 
        playerTimeAvailable = 120; //Pedir al Android que tiempo tiene i que dinero tiene i suspicious
        playerMoneyWin = 1000;
        playerName = "Jaume Tabernero";
        


    }
    private void SetParametersComputer()
    {
    }
    private void OnLevelWasLoaded(int level)
    {

        this.level++;
       // AndroidJavaClass playGameActivity = new AndroidJavaClass("com.example.log_in_java.PlayGameActivity");
        //playGameActivity.Call("SaveGame",this.level,playerTimeAvailable,false,playerSuspicious);
               
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
        doingSetup = true;//Para evitar que las cosas se muevan i perdaos puntos ni tiempo
        AndroidJavaClass playGameActivity = new AndroidJavaClass("com.example.log_in_java.PlayGameActivity");
        String mapa = playGameActivity.Call<String>("GetMapa", this.level);
        boardScript.mapa = mapa;


        dialogImage = GameObject.Find("DialogImage");
        dialogImage.SetActive(false);
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
            from.text = "New York";
        }
        else
        {
            from.text = "Moscow";
        }

        nameUser.text = playerName;
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
        //dialogImage = GameObject.Find("DialogImage");
        dialogImage.SetActive(true);
        //inixializamos 5 de las preguntas del vector de preguntas que tenemos;
        List<string> questionToChoose = questionAvailable;
        for (int i=0; i<4; i++)
        {
            int randpos = UnityEngine.Random.Range(0, questionToChoose.Count);
            Debug.Log(randpos);
            questions.Add(questionToChoose[randpos]);
            questionToChoose.RemoveAt(randpos);

        }
        questions.Add("Does the Airport Escape project deserve a 10 ?");//Esta pregunta no es random pq es la mejor

        ShowQuestion(0);
    }

    public void ShowQuestion(int numQuestion)
    {
        questionText = GameObject.Find("Question").GetComponent<Text>() as Text;
        questionText.text = questions[numQuestion];
        randomanswer = UnityEngine.Random.Range(0, 1);
    }

    public void EndSecurity()
    {
        dialogImage.SetActive(false);
        doingSetup = false;
        SceneManager.LoadScene(0);

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


    public void FinalGame()//Acciones hacer cuando finalizamos el juego
    {

    }










}
