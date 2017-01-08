using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myNetworkManager : MonoBehaviour {
    public Transform playerPaddle;
    [SerializeField] Transform leftPaddleSpawnPoint;
    [SerializeField] Transform RightPaddleSpawnPoint;


    void OnServerInitialized()
    {
        //Debug.Log("gggff");
        SpawnplayerPaddle(leftPaddleSpawnPoint.position, paddleType.left);

    }

    void OnConnectedToServer()
    {
        SpawnplayerPaddle(RightPaddleSpawnPoint.position, paddleType.right);

        // maybe onstart here
        BreakoutGame.SP.gameStart = true;
    }

    void SpawnplayerPaddle(Vector3 spawnPoint,paddleType Type)
    {
        GameObject clone =  (Network.Instantiate(playerPaddle,spawnPoint,Quaternion.identity,0) as Transform).gameObject;
        clone.GetComponent<Paddle>().thisPaddleType = Type;

        switch (Type)
        {
            case paddleType.left:
                BreakoutGame.SP.paddleleftspawn = GetComponentInChildren<Transform>();
                break;
            case paddleType.right:
                BreakoutGame.SP.paddlerightspawn = GetComponentInChildren<Transform>();
                break;
            default:
                break;
        }

    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

}
