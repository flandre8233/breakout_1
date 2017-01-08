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
    public ballType thisball_type;

    Vector3 oldV3;

    void Start () {
        //breakoutgame = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<BreakoutGame>();
        leftkillzone = GameObject.FindGameObjectsWithTag("leftkillzone")[0].transform;
        right_boxcollider = GameObject.FindGameObjectsWithTag("right_boxcollider")[0].transform;

        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -18);

        switch (thisball_type)
        {
            case ballType.left:
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);//球一開始的動量z橫向移動
                break;
            case ballType.right:
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -10);//球一開始的動量z橫向移動
                break;
            default:
                break;
        }

        switch (thisball_type)
        {
            case ballType.left:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case ballType.right:
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
                GetComponent<NetworkView>().RPC("sendMovement", RPCMode.Others, transform.position);
            }

        }
        else
        {

        }




        //Is the ball below -3? Then we're game over.
        if (transform.position.z <= leftkillzone.position.z )
        {
            BreakoutGame.SP.allow_ball();
            BreakoutGame.SP.LostBall();
            //Destroy(gameObject);
            Network.Destroy(gameObject);

        }

    }

    [RPC]
    void sendMovement(Vector3 v3)
    {
        transform.position = v3;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "right_boxcollider")
        {
            BreakoutGame.SP.LostBall_right();
            Network.Destroy(gameObject);
            //Destroy(gameObject);
        }
    }


}
