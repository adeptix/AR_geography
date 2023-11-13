using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingObject : AbstractButtonHandler
{
    private ButtonLocker ButtonLockerScript;

    protected override void TurnOn()
    {
        mainManager.Moving = true;
        ButtonLockerScript.Pressed(gameObject.name);
    }
    
    protected override void TurnOff()
    {
        mainManager.Moving = false;
        ButtonLockerScript.UnPressed(gameObject.name);
    }

    public override void Start()
    {
        base.Start();
        ButtonLockerScript = FindObjectOfType<ButtonLocker>();
    }

}
