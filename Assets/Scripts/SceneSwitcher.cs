using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private const string BasicSceneName = "BasicScene";
    private const string ARSceneName = "ARScene";
    private const string StartSceneName = "StartScene";
        
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
    
    // void Awake() {
    //     DontDestroyOnLoad(gameObject);
    // }
    
    public void GotoBasicScene()
    {
        SceneManager.LoadScene(BasicSceneName);
    }

    public void GotoARScene()
    {
        SceneManager.LoadScene(ARSceneName);
    }
    
    public void GotoStartScene()
    {
        Console.WriteLine("here");
        SceneManager.LoadScene(StartSceneName);
    }
}
