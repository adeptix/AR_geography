using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractButtonHandler : MonoBehaviour
{
    protected Button button;
    protected ProgrammManager mainManager;

    private bool flag = false;

    public virtual void Start()
    {
        mainManager = FindObjectOfType<ProgrammManager>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        Debuger.Log("call abstarct start");
    }

    private void OnClick()
    {
        AnyClick();

        if (flag)
        {
            flag = false;
            GetComponent<Image>().color = Color.white;
            TurnOff();
        }
        else
        {
            flag = true;
            GetComponent<Image>().color = Color.grey;
            TurnOn();
        }
    }

    public void EmulateClick() {
        OnClick();
    }

    protected abstract void TurnOn();

    protected abstract void TurnOff();

    protected virtual void AnyClick() {}
}
