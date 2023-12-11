using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSingleton
{
    private Stack<string> sceneHistory = new Stack<string>();

    public static void LoadScene(string name, bool clear = false)
    {
        if (clear)
        {
            Instance.sceneHistory.Clear();
            SceneManager.LoadScene(name);
            return;
        }

        var current = SceneManager.GetActiveScene().name;
        Instance.sceneHistory.Push(current);
        SceneManager.LoadScene(name);
    }

    public static void LoadPreviousScene()
    {
        if (Instance.sceneHistory.Count == 0)
        {
            Quit();
            return;
        }

        var sceneName = Instance.sceneHistory.Pop();
        SceneManager.LoadScene(sceneName);
    }

    public static void Quit() {
        Application.Quit();
    }



    static SceneSingleton _instance = null;
    static readonly object _padlock = new object();

    public static SceneSingleton Instance
    {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = new SceneSingleton();
                }

                return _instance;
            }
        }
    }
}
