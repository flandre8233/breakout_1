using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myNetworkManager : MonoBehaviour {
    public Transform playerPaddle;
    [SerializeField] Transform leftPaddleSpawnPoint;
    [SerializeField] Transform RightPaddleSpawnPoint;

    List<GameObject> playerData = new List<GameObject>();
    NetworkView networkView;
    void Start()
    {
        networkView = GetComponent<NetworkView>();
        
    }

    void OnServerInitialized()
    {
        //Debug.Log("gggff");
        SpawnplayerPaddle(leftPaddleSpawnPoint.position, paddleType.left);
    }

    void OnConnectedToServer()
    {
        SpawnplayerPaddle(RightPaddleSpawnPoint.position, paddleType.right);
        // maybe onstart here , no not here

    }

    [RPC]
    void PrintText(NetworkMessageInfo info)
    {
        BreakoutGame.SP.gameStart = true;
        Debug.Log("rpc calls");
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("PrintText", RPCMode.All);
    }

    void SpawnplayerPaddle(Vector3 spawnPoint,paddleType Type)
    {
        GameObject clone =  (Network.Instantiate(playerPaddle,spawnPoint,Quaternion.identity,0) as Transform).gameObject;
        clone.GetComponent<Paddle>().thisPaddleType = Type;
        clone.tag = "playerPaddle";

        BreakoutGame.SP.playerPaddleType = Type;
        
        switch (Type)
        {
            case paddleType.left:
                BreakoutGame.SP.paddleleftspawn = clone.GetComponentInChildren<Transform>();
                break;
            case paddleType.right:
                clone.transform.FindChild("spawnPoint").transform.localPosition -= new Vector3(0, 0, 4);
                Debug.Log(clone.transform.FindChild("spawnPoint").name) ;
                BreakoutGame.SP.paddlerightspawn = clone.GetComponentInChildren<Transform>();
                break;
            default:
                break;
        }
        BreakoutGame.SP.paddleBallSpawnPoint = clone.transform.FindChild("spawnPoint").transform;

    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

}
