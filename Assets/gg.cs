using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (iswin)
        {
            if (leftwin)
            {
                GUILayout.Label("left won!");

            }
            else
            {
                GUILayout.Label("right won!");
            }

        }

    }

    bool iswin = false;
    bool leftwin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup" )
        {
            iswin = true;
            if (other.gameObject.GetComponent<Block>().twosideball == "left")
            {
                leftwin = true;
                Debug.Log("left win");
            }else
            {
                leftwin = false;
                Debug.Log("Right win");
            }
            Time.timeScale = 0.0f;
        }
        
    }
}
