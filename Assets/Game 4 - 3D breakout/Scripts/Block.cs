using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
    public enum twoside { left, right };
    public twoside twosideball;
    Transform left_block_add;


    float timeCounter = 0.0f;
    int countdown = 10;
    private void Update()
    {
        if ((timeCounter += Time.deltaTime) >= countdown && countdown > 4 && BreakoutGame.SP.gameStart)
        {
            timeCounter = 0.0f;
            move_block();
            countdown--;
            Debug.Log(countdown);
            left_block_add= GameObject.FindGameObjectsWithTag("Pickup")[0].transform;
        }
    }

    void OnTriggerEnter () {
        BreakoutGame.SP.HitBlock();
        Destroy(gameObject);
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        BreakoutGame.SP.HitBlock();
        Destroy(gameObject);
        
    }

    public void move_block()
    {
        //transform.Translate(new Vector3(0, 5, 0));
        //24.85168


        switch (twosideball)//磚塊每次移動的位置
        {
            case twoside.left:
                transform.Translate(new Vector3(0, 3, 0));
                break;
            case twoside.right:
                transform.Translate(new Vector3(0, -3, 0));
                break;
            default:
                break;
            }

        }

    }

