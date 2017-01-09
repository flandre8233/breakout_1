using UnityEngine;
using System.Collections;

public enum BreakoutGameState { playing, won, lost };

public class BreakoutGame : MonoBehaviour
{
    public static BreakoutGame SP;
    public bool gameStart;

    public GameObject yellowLine;
    public GameObject redLine;

    public Transform ballPrefab;
    public Transform paddleBallSpawnPoint;
    public paddleType playerPaddleType;
    public Transform paddleleftspawn;
    public Transform paddlerightspawn;
    public Transform lineSpawnPoint;

    private int totalBlocks;
    private int blocksHit;
    private BreakoutGameState gameState;
    public ballType thisball_type;

    [SerializeField]Transform first_Row;
    public float firstPositionZ;
    public GameObject left_new_roll;
    bool allow_ball = true;

    float deadTime = 2f;
    int deaddead = 0;

    void Awake()
    {
        SP = this;
        blocksHit = 0;
        gameState = BreakoutGameState.playing;
        totalBlocks = GameObject.FindGameObjectsWithTag("Pickup").Length;
        Time.timeScale = 1.0f;

    //SpawnBall(0);
}
    void Start() {
        //firstPositionZ = first_Row.position.z;
    }

    void SpawnBall(int ballSpawnMethod)
    {
        #region SpawnFunction
        GameObject cloneObjectleft;
        GameObject cloneObjectright;
        switch (ballSpawnMethod)
        {
            case 1:
                cloneObjectleft = (Network.Instantiate(ballPrefab, paddleBallSpawnPoint.position, Quaternion.identity,0) as Transform).gameObject; // left球射出的座標
                //Debug.Log(paddleleft.position);
                cloneObjectleft.GetComponent<Ball>().thisball_type = "left";

                break;
            case 2:
                cloneObjectright = (Network.Instantiate(ballPrefab, paddleBallSpawnPoint.position, Quaternion.identity, 0) as Transform).gameObject;
                cloneObjectright.GetComponent<Ball>().thisball_type = "right";
                break;
            case 0:
                //cloneObjectleft = (Instantiate(ballPrefab, paddleleftspawn.position, Quaternion.identity) as Transform).gameObject; // right球射出的座標
                //cloneObjectleft.GetComponent<Ball>().thisball_type = ballType.left;
                //cloneObjectright = (Instantiate(ballPrefab, paddlerightspawn.position, Quaternion.identity) as Transform).gameObject;
                //cloneObjectright.GetComponent<Ball>().thisball_type = ballType.right;
                break;
            default:
                break;
        }
        #endregion
    }

    bool doOnce = false;
    void Update()
    {
        
        if (SP.gameStart)
        {
            blockTimer();
            if (!doOnce)
            {
                doOnce = true;

                addNewLine(lineSpawnPoint.position, playerPaddleType.ToString() ); // spawn new line
                allBlockMove();
                addNewLine(lineSpawnPoint.position, playerPaddleType.ToString()); // spawn new line
                allBlockMove();
                addNewLine(lineSpawnPoint.position, playerPaddleType.ToString()); // spawn new line
                allBlockMove();
                addNewLine(lineSpawnPoint.position, playerPaddleType.ToString()); // spawn new line
                allBlockMove();
                addNewLine(lineSpawnPoint.position, playerPaddleType.ToString()); // spawn new line
                allBlockMove();
            }
            if (Input.GetMouseButtonDown(0) && allow_ball)
            {
                switch (playerPaddleType)
                {
                    case paddleType.left:
                        SpawnBall(1);
                        break;
                    case paddleType.right:
                        SpawnBall(2);
                        break;
                    default:
                        break;
                }
                
            }

            if (Input.GetMouseButtonDown(1))
            {

            }

            /*
            if (Input.GetMouseButtonDown(0) && allow_ball)
                SpawnBall(1);
            //if()
            if(Input.GetMouseButtonDown(0))
                SpawnBall(2);
            if (Input.GetMouseButtonDown(1) && allow_ball_right)//連線時更改，left射波，right道具
                SpawnBall(2);

            */
        }



    }

    float timeCounter = 0.0f;
    int countdown = 10;
    public void blockTimer()
    {
        if ((timeCounter += Time.deltaTime) >= countdown && countdown > 4 && gameStart )
        {
            timeCounter = 0.0f;
            countdown--;
            addNewLine(lineSpawnPoint.position, playerPaddleType.ToString()); // spawn new line
            allBlockMove();

        }
    }

    public void allBlockMove()
    {
        if (playerPaddleType == paddleType.left)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Pickup"))
            {
                if (item.GetComponent<Block>().twosideball == "left")
                {
                    item.GetComponent<Block>().move_block();
                }
            }
        }else
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Pickup"))
            {
                if (item.GetComponent<Block>().twosideball == "right")
                {
                    item.GetComponent<Block>().move_block();
                }
            }
        }

    }

    public void LeftBlockMove()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            if (item.GetComponent<Block>().twosideball == "right")
            {
                item.GetComponent<Block>().move_block();
            }
        }
    }


    public void RightBlockMove()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            if (item.GetComponent<Block>().twosideball == "left")
            {
                item.GetComponent<Block>().move_block();
            }
        }
    }


    void OnGUI(){
    
        GUILayout.Space(10);
        GUILayout.Label("  Hit: " + blocksHit + "/" + totalBlocks);
        wonOrLostSetting();

    }

    bool isSpawnRed = true;
    bool isSpawnYellow = true;
    int spawncounter = 0;

    void addNewLine(Vector3 v3,string type)
    {
        GameObject obj;
        spawncounter++;
        switch (spawncounter)
        {
            case 1:
                obj = Network.Instantiate(redLine, v3, Quaternion.identity, 0) as GameObject;
                foreach (var item in obj.GetComponentsInChildren<Block>())
                {
                    item.twosideball = type;
                }
                break;
            case 2:
                obj = Network.Instantiate(yellowLine, v3, Quaternion.identity, 0) as GameObject;
                foreach (var item in obj.GetComponentsInChildren<Block>())
                {
                    item.twosideball = type;
                }
                spawncounter = 0;
                break;
        }
    }

    void wonOrLostSetting()
    {
        if (gameState == BreakoutGameState.lost)
        {
            GUILayout.Label("You Lost!");
            if (GUILayout.Button("Try again"))
            {
                //Application.LoadLevel(Application.loadedLevel);
            }
        }
        else if (gameState == BreakoutGameState.won)
        {
            GUILayout.Label("You won!");
            if (GUILayout.Button("Play again"))
            {
                //Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public void allow_ball_f()
    {
        if (allow_ball)
        {
            deaddead++;
            allow_ball = false;
            StartCoroutine(waitaSec() );
        }

    }

    IEnumerator waitaSec()
    {
        yield return new WaitForSeconds(deadTime);
        deadTime = deadTime + deaddead*1.2f;
        allow_ball = true;
    }

    public void HitBlock()
    {
        blocksHit++;

        //For fun:
        /*if (blocksHit % 10 == 0) //10second新增球
        {
            SpawnBall(0);
        }
        */


        if (blocksHit >= 10)
        {
            blocksHit = 0;
            switch (playerPaddleType)
            {
                case paddleType.left:
                    addNewLine(new Vector3(0.3957386f, 3.750214f, 21.09168f), "right"); // spawn new line
                    LeftBlockMove();
                    break;
                case paddleType.right:
                    addNewLine(new Vector3(0.3957386f, 3.750214f, 15.49168f), "left"); // spawn new line
                    RightBlockMove();
                    break;
                default:
                    break;
            }
            
            //WonGame();
        }
    }

    public void SpawnNewRoll()
    {
        //GameObject left_new_roll_Clone = Instantiate(left_new_roll, gameObject.transform.position, Quaternion.identity) as GameObject;
    }

    public void WonGame()
    {
        Time.timeScale = 0.0f; //Pause game
        gameState = BreakoutGameState.won;
    }

    public void LostBall()
    {
        int ballsLeft = GameObject.FindGameObjectsWithTag("Player").Length;
        if(ballsLeft<=1){
            Debug.Log("left_hit");

            //Was the last ball..如果球碰到kill zone就gameover
            //SetGameOver();
        }
    }
    public void LostBall_right()
    {
        int ballright = GameObject.FindGameObjectsWithTag("right_boxcollider").Length;
        if (ballright <= 1)
        {
            //Was the last ball..
            Debug.Log("right_hit", gameObject);
            //SpawnBall();
            //SetGameOver();
        }
    }


        public void redline()//紅線
        {
            int left_red = GameObject.FindGameObjectsWithTag("left_red").Length;

                Debug.Log("leftred");
        }
        public void SetGameOver()
    {
        Time.timeScale = 0.0f; //Pause game
        gameState = BreakoutGameState.lost;
    }
}
