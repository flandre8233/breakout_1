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
            GetComponent<NetworkView>().RPC("sendType", RPCMode.Others, twosideball);

        }
        else
        {

        }
        
    }

    [RPC]
    void sendMovement(Vector3 v3)
    {
        transform.position = v3;
    }

    [RPC]
    void sendType(string type)
    {
        twosideball = type;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<NetworkView>().isMine)
        {
            BreakoutGame.SP.HitBlock();
            Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
            Network.Destroy(gameObject);

            //Destroy(gameObject);
        }

        
    }

    public void move_block()
    {
        //transform.Translate(new Vector3(0, 5, 0));
        //24.85168


        switch (twosideball)//�j���C�����ʪ���m
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
        GetComponent<NetworkView>().RPC("sendMovement", RPCMode.Others, transform.position);
    }

    }

