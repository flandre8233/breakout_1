using UnityEngine;
using System.Collections;

public enum BreakoutGameState { playing, won, lost };

public class BreakoutGame : MonoBehaviour
{
    public static BreakoutGame SP;
    public bool gameStart;

    public Transform ballPrefab;
    public Transform paddleleftspawn;
    public Transform paddlerightspawn;

    private int totalBlocks;
    private int blocksHit;
    private BreakoutGameState gameState;
    public ballType thisball_type;

    [SerializeField]Transform first_Row;
    public float firstPositionZ;
    public GameObject left_new_roll;
    bool allow_ball_left = true;
    bool allow_ball_right = true;

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

        firstPositionZ = first_Row.position.z;
    }

    void SpawnBall(int ballSpawnMethod)
    {
        #region SpawnFunction
        GameObject cloneObjectleft;
        GameObject cloneObjectright;
        switch (ballSpawnMethod)
        {
            case 1:
                cloneObjectleft = (Instantiate(ballPrefab, paddleleftspawn.position, Quaternion.identity) as Transform).gameObject; // left球射出的座標
                //Debug.Log(paddleleft.position);
                cloneObjectleft.GetComponent<Ball>().thisball_type = ballType.left;

                break;
            case 2:
                cloneObjectright = (Instantiate(ballPrefab, paddlerightspawn.position, Quaternion.identity) as Transform).gameObject;
                cloneObjectright.GetComponent<Ball>().thisball_type = ballType.right;
                break;
            case 0:
                cloneObjectleft = (Instantiate(ballPrefab, paddleleftspawn.position, Quaternion.identity) as Transform).gameObject; // right球射出的座標
                cloneObjectleft.GetComponent<Ball>().thisball_type = ballType.left;
                cloneObjectright = (Instantiate(ballPrefab, paddlerightspawn.position, Quaternion.identity) as Transform).gameObject;
                cloneObjectright.GetComponent<Ball>().thisball_type = ballType.right;
                break;
            default:
                break;
        }
        #endregion
    }


    void Update()
    {
        if (SP.gameStart)
        {
            if (Input.GetMouseButtonDown(2) && allow_ball_left)
                SpawnBall(1);
            //if()
            /*if(Input.GetMouseButtonDown(0))
                SpawnBall(2);*/
            if (Input.GetMouseButtonDown(1) && allow_ball_right)//連線時更改，left射波，right道具
                SpawnBall(2);

        }

    }
        void OnGUI(){
    
        GUILayout.Space(10);
        GUILayout.Label("  Hit: " + blocksHit + "/" + totalBlocks);
        wonOrLostSetting();

    }

    void wonOrLostSetting()
    {
        if (gameState == BreakoutGameState.lost)
        {
            GUILayout.Label("You Lost!");
            if (GUILayout.Button("Try again"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        else if (gameState == BreakoutGameState.won)
        {
            GUILayout.Label("You won!");
            if (GUILayout.Button("Play again"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public void allow_ball()
    {
        allow_ball_left = false;
        Debug.Log(allow_ball_left);
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
        if (blocksHit >= totalBlocks)
        {
            WonGame();
        }
    }

    public void SpawnNewRoll()
    {
        GameObject left_new_roll_Clone = Instantiate(left_new_roll, gameObject.transform.position, Quaternion.identity) as GameObject;
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
