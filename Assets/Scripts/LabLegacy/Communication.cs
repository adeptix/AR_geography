using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Communication : AbstractButtonHandler
{
    private ButtonLocker ButtonLockerScript;

    protected override void TurnOn()
    {
        mainManager.Communication = true;
        ButtonLockerScript.Pressed(gameObject.name);
    }
    
    protected override void TurnOff()
    {
        mainManager.Communication = false;
        ButtonLockerScript.UnPressed(gameObject.name);
    }

    public override void Start()
    {
        base.Start();
        ButtonLockerScript = FindObjectOfType<ButtonLocker>();
    }

}
