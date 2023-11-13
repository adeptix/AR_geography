using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    private Button button; 
    //private ProgrammManager ProgrammManagerScript; 
    private bool clicked = false;

    void Start()
    {
        button = GetComponent<Button>(); 
        button.onClick.AddListener(SomeFunction); 

    }

    void SomeFunction() 
    {
        if (clicked) 
        {
            clicked = false; 
            GetComponent<Image>().color = Color.white; 
            
            Debug.LogFormat("ADEPT - unclick in world canvas {0}", gameObject.name);
        }
        else 
        {
            clicked = true; 
            GetComponent<Image>().color = Color.grey;
            Debug.LogFormat("ADEPT - click in world canvas {0}", gameObject.name);
        }
    }
}
