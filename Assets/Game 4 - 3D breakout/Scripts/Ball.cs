using UnityEngine;
using System.Collections;

public enum ballType { left, right };
public class Ball: MonoBehaviour {
    public BreakoutGame breakoutgame;
    public float maxVelocity = 16;
    public float minVelocity = 12;
    Transform leftkillzone;
    Transform right_boxcollider;
    //Transform rightkillzone;
    public string thisball_type;

    Vector3 oldV3;

    void Start () {
        //breakoutgame = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<BreakoutGame>();
        leftkillzone = GameObject.FindGameObjectsWithTag("leftkillzone")[0].transform;
        right_boxcollider = GameObject.FindGameObjectsWithTag("right_boxcollider")[0].transform;

        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -18);
        setBall();

        switch (thisball_type) {
            case "left":
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);//球一開始的動量z橫向移動
                break;
            case "right":
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -10);//球一開始的動量z橫向移動
                break;
            default:
                break;
        }

    }

    void setBall()
    {


        switch (thisball_type)
        {
            case "left":
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case "right":
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        //Make sure we stay between the MAX and MIN speed.


        if (GetComponent<NetworkView>().isMine)
        {
            float moveInput = 0.0f;

            float totalVelocity = Vector3.Magnitude(GetComponent<Rigidbody>().velocity);
            if (totalVelocity > maxVelocity)
            {
                float tooHard = totalVelocity / maxVelocity;
                GetComponent<Rigidbody>().velocity /= tooHard;
            }
            else if (totalVelocity < minVelocity)
            {
                float tooSlowRate = totalVelocity / minVelocity;
                GetComponent<Rigidbody>().velocity /= tooSlowRate;
            }

            if (Vector3.Distance(transform.position, oldV3) >= 0.05)
            {
                oldV3 = transform.position;
                GetComponent<NetworkView>().RPC("sendType", RPCMode.Others, thisball_type);
                GetComponent<NetworkView>().RPC("sendMovementBALL", RPCMode.Others, transform.position);
            }

        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }




        //Is the ball below -3? Then we're game over.
        if (transform.position.z <= leftkillzone.position.z && GetComponent<NetworkView>().isMine)
        {
            BreakoutGame.SP.allow_ball_f();
            BreakoutGame.SP.LostBall();
            //Destroy(gameObject);

            Network.Destroy(gameObject);
            Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
        }

    }

    [RPC]
    void sendMovementBALL(Vector3 v3)
    {
        transform.position = v3;
    }

    [RPC]
    void sendType(string type)
    {
        thisball_type = type;
        setBall();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "right_boxcollider" && GetComponent<NetworkView>().isMine)
        {
            BreakoutGame.SP.allow_ball_f();
           

            Network.Destroy(gameObject);
            Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
            //Destroy(gameObject);
        }
    }


}
