﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 30;
    public int rows = 70;
    public GameObject[] floorTiles;
    public GameObject[] perimetralFloor;
    public GameObject[] enemyTiles;
    public GameObject[] externalWalls;
    public GameObject cleanersSprite;
    public GameObject playerSprite;
    public GameObject thiefSprite;
    public GameObject shopperSprite;
    public GameObject securityManSprite;
    public GameObject securityPointSprite;
    public GameObject securitySignalSprite;
    public GameObject[] plants;
    public GameObject benchHorizontal;
    public GameObject benchVertical;
    public GameObject bin;
    public GameObject arrivals;
    public GameObject departures;
    public GameObject baggage;
    public GameObject[] companys;
    public GameObject secretWall;
    public GameObject fence;
    public GameObject nextLevelSignal;
    public GameObject finalGameSignal;

    private GameObject mainCamera;

    private Transform boardHolder;
    private List<Vector3> gridPositionsCleanerPart1 = new List<Vector3>(); //To places the tiles


    public string mapa;

    public void Start()
    {
        //Buscamos la camara principal para asi poder assignar el personaje
        mainCamera = GameObject.FindWithTag("MainCamera");
    }
    public void SetMap(string mapa)
    {
        this.mapa = mapa;
    }


    void BoardSetup()
    {
        GameObject board = new GameObject("Board");
        board.AddComponent(typeof(CompositeCollider2D));
        boardHolder = board.transform;
        boardHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject playerinstance = new GameObject("Player");
        //String mapa;
        /*if (GameManager.instance.level == 1)
        {
            mapa = "40 20\r\n"
            + "########################################\r\n"
            + "#        a          S                  #\r\n"
            + "#                   #              P   #\r\n"
            + "#C                 V#V                 #\r\n"
            + "#                   #                  #\r\n"
            + "#                  V#V                 #\r\n"
            + "#         i         #                  #\r\n"
            + "#C                 b#V                 #\r\n"
            + "#                 Cp#p                 #\r\n"
            + "#                   #vvvvvvvvvvv  vvvvv#\r\n"
            + "#                   #                  #\r\n"
            + "#                   #                  #\r\n"
            + "# T               Cb#b                 #\r\n"
            + "# B B B B B B   B  V#V        P        #\r\n"
            + "#           C       #                  #\r\n"
            + "# B B B   B B B B  V#V                 #\r\n"
            + "#C                  #                 I#\r\n"
            + "# B B B B B B B B  V#V                 #\r\n"
            + "#                   #                F #\r\n"
            + "########################################\r\n";
            mapa = "50 30\r\n"
            + "##################################################\r\n"
            + "#                        p#                      #\r\n"
            + "#            i           p#           m      T   #\r\n"
            + "#                        p#                      #\r\n"
            + "#                                               p#\r\n"
            + "#                                               p#\r\n"
            + "#b                       p#                     p#\r\n"
            + "# B B B B     B B B      p#                      #\r\n"
            + "#                      C  #                      #\r\n"
            + "# B B B B B B B B B       #                 P    #\r\n"
            + "# b                    C  #         I            #\r\n"
            + "###########S###############                      #\r\n"
            + "#  p p p p p p p p p p p p                       #\r\n"
            + "#V                                     a         #\r\n"
            + "#    P                                           #\r\n"
            + "#V                       B B B       B B B B B B #\r\n"
            + "#                                                #\r\n"
            + "#V  C                    B B B       B B B B B B #\r\n"
            + "#                                                #\r\n"
            + "#b                       B B B       B B B B B B #\r\n"
            + "#vvvvvvvvvvvvvvvvvvvv    vvvvvvvvvvvvvvvvvvvvvvvv#\r\n"
            + "#    C                                           #\r\n"
            + "#V                                               #\r\n"
            + "#   B B B B B B  b                     d         #\r\n"
            + "#V                                               #\r\n"
            + "#   B B B B B B  b                           S   #\r\n"
            + "#V   C                                       S   #\r\n"
            + "#   B B B B B B  b                               #\r\n"
            + "#b                                              E#\r\n"
            + "##################################################\r\n";
            mapa = "70 30\r\n"

+ "######################################################################\r\n"
+ "#pppppppppppppppp#                   #         Vp                  E #\r\n"
+ "#I          V   V#                              p                    #\r\n"
+ "#                #                   #         Vp                    #\r\n"
+ "#           V   V#               s   #          p            C       #\r\n"
+ "#         C      #                   #         Vp                    #\r\n"
+ "#           V   V#              C    #          p                    #\r\n"
+ "#                #                   #         Vp    i     i         #\r\n"
+ "#      s    V   V#            i      #     C    p                    #\r\n"
+ "#                #        P          #         Vp                    #\r\n"
+ "#           V   V#                   #          p         s          #\r\n"
+ "# C              #                  b#         Vp              m     #\r\n"
+ "#                #vvv vvvvvvvvvvvvvvv#          p                   b#\r\n"
+ "#           s    #     pppppppppppppp#         Vp         Vp        V#\r\n"
+ "#                #            C      #    P     p C        p         #\r\n"
+ "#     d          #                   #         Vp         Vp        V#\r\n"
+ "#                #    s              #          p          p         #\r\n"
+ "#p               #                   #                    Vp        V#\r\n"
+ "#                #                   #     T               p         #\r\n"
+ "#      s         #     P             #                    Vp        V#\r\n"
+ "#                #                   #              C      p         #\r\n"
+ "#               s#                  b#   P                Vp    P   V#\r\n"
+ "# B B B B B      #vvvvvvvvvvvvv vvvvv#                     p         #\r\n"
+ "#                #ppppppppppppp      #               C    Vp        V#\r\n"
+ "# B B B B Bb     #                   #                     p         #\r\n"
+ "#                S                 P #                    Vp        V#\r\n"
+ "# B B B B B      #                   #        P            p         #\r\n"
+ "#                #                   #                    Vp        V#\r\n"
+ "#pppppppppppppppp#                   #                     p         #\r\n"
+ "######################################################################\r\n";


        }
        else if(GameManager.instance.level==2)
        {
            mapa = "15 10\r\n"
            + "###############\r\n"
            + "#I  s     Y  p#\r\n"
            + "#    vvvvvvvv #\r\n"
            + "#s            #\r\n"
            + "#vvvvvvvvvv   #\r\n"
            + "# p  s   B    #\r\n"
            + "#  s         p#\r\n"
            + "#        vvvvv#\r\n"
            + "# Y          x#\r\n"
            + "###############\r\n";
            mapa = "30 15\r\n"
+ "##############################\r\n"
+ "#   S  S      Y            x #\r\n"
+ "#                            #\r\n"
+ "#p                          p#\r\n"
+ "#p                          p#\r\n"
+ "#p                          p#\r\n"
+ "#                            #\r\n"
+ "# C                          #\r\n"
+ "#vvvvvvvvvvv  vvvvvvvvvvvvvvv#\r\n"
+ "#         S    S            p#\r\n"
+ "#                           b#\r\n"
+ "#                 B B B B B  #\r\n"
+ "#                            #\r\n"
+ "#I                B B B B B  #\r\n"
+ "##############################\r\n";
        }
        else
        {
            mapa = "40 20\r\n"
            + "########################################\r\n"
            + "#        a          S                  #\r\n"
            + "#                   #              P   #\r\n"
            + "#C                 V#V                 #\r\n"
            + "#                   #                  #\r\n"
            + "#                  V#V                 #\r\n"
            + "#         i         #                  #\r\n"
            + "#C                 b#V                 #\r\n"
            + "#                 Cp#p                 #\r\n"
            + "#                   #vvvvvvvvvvv  vvvvv#\r\n"
            + "#                   #                  #\r\n"
            + "#                   #                  #\r\n"
            + "# T               Cb#b                 #\r\n"
            + "# B B B B B B   B  V#V        P        #\r\n"
            + "#           C       #                  #\r\n"
            + "# B B B   B B B B  V#V                 #\r\n"
            + "#C                  #                 I#\r\n"
            + "# B B B B B B B B  V#V                 #\r\n"
            + "#                   #                F #\r\n"
            + "########################################\r\n";
            mapa = "70 20\r\n"
+ "######################################################################\r\n"
+ "#                    #     T           #                            F#\r\n"
+ "#            a       #                 # C                           #\r\n"
+ "#                   p#                 #                             #\r\n"
+ "# B B B B            #                 #                    B B B B  #\r\n"          
+ "#                   p#                                               #\r\n"
+ "# B B B B            #                 #                    B B B B  #\r\n"
+ "#                   p#               p #                             #\r\n"
+ "#                                    p # P                           #\r\n"
+ "#                    #               p #                             #\r\n"
+ "#P                   #               p #                        d    #\r\n"
+ "#                    ########s##########                             #\r\n"
+ "#p       b B B B B B #                                             V #\r\n"
+ "#                  C #  C                                            #\r\n" 
+ "#p       b B B B B B #    B B B B B B B                            V #\r\n"  
+ "#                  C #                                               #\r\n"                   
+ "#p       b B B B B B #    B B B B B B B              P    i        V #\r\n"
+ "#                    #                                               #\r\n"
+ "#I                   #  p  p  p  p  p  p  p  p  p  p  p  p  p  p  p  #\r\n"
+ "######################################################################\r\n";
        }*/

        mapa = mapa.Replace("\r\n", "\n");
        String[] maplines = mapa.Split('\n');
        String[] mesures = maplines[0].Split(' ');
        int columns = Int32.Parse(mesures[0]);
        int rows = Int32.Parse(mesures[1]);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                char ch = maplines[y + 1][x];
                switch (ch)
                {
                    case '#': //instanciamos una external wall
                        GameObject toInstantiate = externalWalls[Random.Range(0, externalWalls.Length)];
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x, rows-y, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(boardHolder);
                        break;
                    case 'S': //instanciamos una external wall secreta para mover
                        GameObject instanceSecretWall = Instantiate(secretWall, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instanceSecretWall.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'v': //instanciamos una fence de cristal
                        GameObject instancefence = Instantiate(fence, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instancefence.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'b'://papelera

                        GameObject instance2 = Instantiate(bin, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance2.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows-y,rows,columns);
                        break;

                    case 'B': //Banco Horizontal
                        GameObject instance3 = Instantiate(benchHorizontal, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance3.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;

                    case 'V'://Banco Vertical
                        GameObject instance4 = Instantiate(benchVertical, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance4.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;

                    case 'p': //planta
                        GameObject toInstantiate2 = plants[Random.Range(0, plants.Length)];
                        GameObject instance5 = Instantiate(toInstantiate2, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance5.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'd': //Cartel departures
                        GameObject instance6 = Instantiate(departures, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance6.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'a': //Cartel Arrivals
                        GameObject instance7 = Instantiate(arrivals, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance7.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'm': //Cartel baggage
                        GameObject instance8 = Instantiate(baggage, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance8.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'i': //Cartel de companyia aleatoria
                        GameObject toInstantiateCompanys = companys[Random.Range(0, companys.Length)];
                        GameObject instance9 = Instantiate(toInstantiateCompanys, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instance9.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'E'://Exit to next level
                        GameObject instanceexit =Instantiate(nextLevelSignal, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instanceexit.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'F'://Exit to next level
                        GameObject instancefinal = Instantiate(finalGameSignal, new Vector3(x, rows - y, 0f), Quaternion.identity) as GameObject;
                        instancefinal.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;

                    case 'C': //Cleaner                        
                        GameObject instance10 = Instantiate(cleanersSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instance10.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'T': //Thief                       
                        GameObject instanceThief = Instantiate(thiefSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instanceThief.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'P': //Shopper                        
                        GameObject instanceShopper = Instantiate(shopperSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instanceShopper.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 's': //policeMan                       
                        GameObject instanceSecurity = Instantiate(securityManSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instanceSecurity.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'x': //securityPoint                     
                        GameObject instanceSecurityPoint = Instantiate(securityPointSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instanceSecurityPoint.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;
                    case 'Y': //securitySignal                     
                        GameObject instanceSecuritySignal = Instantiate(securitySignalSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        instanceSecuritySignal.transform.SetParent(boardHolder);
                        intantiateFloor(x, rows - y, rows, columns);
                        break;

                    case 'I': //Inicio del Jugador                       
                        playerinstance = Instantiate(playerSprite, new Vector3(x, rows - y, 0f), Quaternion.identity);
                        intantiateFloor(x, rows - y, rows, columns);

                        break;
                    default:
                        intantiateFloor(x, rows - y, rows, columns);
                        break;

                }
            }


        }
        //Añadimos la camara de seguimiento del personage.

        CameraFollow camerascript = mainCamera.GetComponent<CameraFollow>();
        camerascript.following = playerinstance;
        //GameObject camerainstance =Instantiate(mainCamera, new Vector3(playerinstance.transform.position.x, playerinstance.transform.position.y, -10f), Quaternion.identity);
        

    }

    public void intantiateFloor(int x, int y, int rows, int columns)
    {
        GameObject floorToInstatiate = floorTiles[Random.Range(0, floorTiles.Length)];
        if (x == 1 || x == columns - 2 || y == 2 || y == rows - 1)//Ponemos el suelo perimetral
            floorToInstatiate = perimetralFloor[Random.Range(0, perimetralFloor.Length)];
        //Instanciamos el suelo sea el que sea
        GameObject instanceFloor = Instantiate(floorToInstatiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        instanceFloor.transform.SetParent(boardHolder);
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositionsCleanerPart1.Count);


        Vector3 randomPosition = gridPositionsCleanerPart1[randomIndex];


        gridPositionsCleanerPart1.RemoveAt(randomIndex);

        return randomPosition;
    }

    public void SetUpScene()
    {
        BoardSetup();
        //LayoutObjectAtRandom(cleanersSprite, cleanerCount.minimum, cleanerCount.maximum);

    }


}

