using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{

    [SerializeField] private GameObject[] objectsToRotate;
    [SerializeField] private bool needRotate = false;
    [SerializeField] private float degreesPerSecond;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (degreesPerSecond == 0)
        {
            degreesPerSecond = 25;
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (!needRotate)
        {
            return;
        }

        foreach (var obj in objectsToRotate)
        {
            Rotate(obj);
        }
    }

    private void Rotate(GameObject obj)
    {
        try
        {
            obj.transform.Rotate(new Vector3(degreesPerSecond, degreesPerSecond, degreesPerSecond) * Time.deltaTime);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
