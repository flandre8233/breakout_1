using UnityEngine;
using System.Collections;

public enum paddleType { left,right };


public class Paddle : MonoBehaviour {
    [SerializeField]
    public paddleType thisPaddleType;
    public float moveSpeed = 15;


    Vector3 oldV3;

    void Update () {
        
        if (GetComponent<NetworkView>().isMine) {
            float moveInput = 0.0f;
            Debug.Log(thisPaddleType);
            moveInput = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            transform.position += new Vector3(moveInput, 0, 0);
            float max = 16.5f;
            if (transform.position.x <= -max || transform.position.x >= max) {
                float xPos = Mathf.Clamp(transform.position.x, -max, max); //Clamp between min -5 and max 5
                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            }

            if (Vector3.Distance(transform.position, oldV3) >= 0.05) {
                oldV3 = transform.position;
                GetComponent<NetworkView>().RPC("sendMovement", RPCMode.Others, transform.position);
            }

        }
        else {

        }

       
        switch (thisPaddleType)
        {
            case paddleType.left:
                //left板移動
                //Vector3 left_paddle_pos = gameObject.transform.position;//left get 座標
                //Debug.Log(left_paddle_pos);
                break;
            case paddleType.right:
                //right板移動
                //Vector3 right_paddle_pos = gameObject.transform.position;//right get 座標
               //Debug.Log(right_paddle_pos);
                break;
            default:
                break;
        }

       


	}

    [RPC]
    void sendMovement(Vector3 v3) {
        transform.position = v3;
    }

    void OnCollisionExit(Collision collisionInfo ) {
        //Add X velocity..otherwise the ball would only go up&down
        Rigidbody rigid = collisionInfo.rigidbody;
        float xDistance = rigid.position.x - transform.position.x;
        rigid.velocity = new Vector3(rigid.velocity.x + xDistance/2, rigid.velocity.y, rigid.velocity.z);
    }
}
