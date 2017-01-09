using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
    public enum twoside { left, right };
    public string twosideball;
    Transform left_block_add;



    private void Update()
    {
        if (GetComponent<NetworkView>().isMine)
        {
            GetComponent<NetworkView>().RPC("sendTypeBlock", RPCMode.Others, twosideball);
            GetComponent<NetworkView>().RPC("sendMovementBlock", RPCMode.Others, transform.position);

        }
        else
        {

        }
        
    }

    [RPC]
    void sendMovementBlock(Vector3 v3)
    {
        transform.position = v3;
    }

    [RPC]
    void sendTypeBlock(string type)
    {
        twosideball = type;
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (gameObject.GetComponent<NetworkView>().isMine && collision.gameObject.tag == "Player" )
        {
            BreakoutGame.SP.HitBlock();
            Network.Destroy(gameObject);
            //Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);


            //Destroy(gameObject);
        }

        
    }

    public void move_block()
    {
        //transform.Translate(new Vector3(0, 5, 0));
        //24.85168


        switch (twosideball)//磚塊每次移動的位置
        {
            case "left":
                transform.Translate(new Vector3(0, 3, 0));
                break;
            case "right":
                transform.Translate(new Vector3(0, -3, 0));
                break;
            default:
                break;
            }

    }

    }

