using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitcher : MonoBehaviour
{
    private const string StartSceneName = "StartScene";
    private const string BasicSceneName = "BasicScene";
    private const string ARSceneName = "ARScene";

    public void GotoBasicScene()
    {
        SceneSingleton.LoadScene(BasicSceneName);
    }

    public void GotoARScene()
    {
        SceneSingleton.LoadScene(ARSceneName);
    }

    public void GotoStartScene()
    {
        SceneSingleton.LoadScene(StartSceneName, clear: true);
    }

    public void Quit() {
        SceneSingleton.Quit();
    }

    void Update()
    {
        // back button on android
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneSingleton.LoadPreviousScene();
            return;
        }
    }
}
