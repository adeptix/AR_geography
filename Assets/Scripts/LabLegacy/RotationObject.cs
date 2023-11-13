using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationObject : AbstractButtonHandler
{
    private ButtonLocker ButtonLockerScript;


    protected override void TurnOn()
    {
        mainManager.Rotation = true;
        ButtonLockerScript.Pressed(gameObject.name);
    }
    
    protected override void TurnOff()
    {
        mainManager.Rotation = false;
        ButtonLockerScript.UnPressed(gameObject.name);
    }

    public override void Start()
    {
        base.Start();
        ButtonLockerScript = FindObjectOfType<ButtonLocker>();
    }
}
