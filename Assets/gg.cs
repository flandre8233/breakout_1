using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gg : MonoBehaviour {

	// Use this for initialization
	void Start () {
        smallFont = new GUIStyle();
        largeFont = new GUIStyle();

        smallFont.fontSize = 10;
        largeFont.fontSize = 32;
        largeFont.normal.textColor = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    GUIStyle smallFont;
    GUIStyle largeFont;

    void OnGUI()
    {
        

        if (iswin)
        {
            if (leftwin)
            {

                GUI.Label(new Rect(100, 100, 200, 200), "Right Win", largeFont);

            }
            else
            {
                GUI.Label(new Rect(100, 100, 200, 200), "Left Win", largeFont);
                
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
                leftwin = false;
                Debug.Log("Right win");
            }else
            {
                leftwin = true;
                Debug.Log("Left win");
            }
            Time.timeScale = 0.0f;
        }
        
    }
}
