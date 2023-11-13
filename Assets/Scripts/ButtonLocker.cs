using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLocker : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    [SerializeField] private List<Button> buttonsForObjectOnly;

    public bool HasObject = false;

    public void Pressed(string buttonName) {
        foreach (Button b in buttons) {
            if (b.name == buttonName) {
                continue;
            }

            DisableButton(b);
        }
    }

    public void UnPressed(string buttonName) {
        Debuger.Log("Unpressed");
        foreach (Button b in buttons) {
            if (b.name == buttonName) {
                continue;
            }

            Debuger.LogFormat("{0} - {1} {2}", buttonName, b.name, !HasObject && ForObjectOnly(b));
            

            if (!HasObject && ForObjectOnly(b)) {
                continue;
            }

            EnableButton(b);
        }
    }

    private bool ForObjectOnly(Button b) {
        return buttonsForObjectOnly.Contains(b);
    }


    void Start() {
        foreach (Button b in buttonsForObjectOnly) {
            DisableButton(b);
        }
    }

    private void DisableButton(Button b) {
         Debuger.LogFormat("Disable {0}", b.name);
        b.enabled = false;
        b.interactable = false;
    }

    private void EnableButton(Button b) {
        Debuger.LogFormat("Enable {0}", b.name);
        b.enabled = true;
        b.interactable = true;
    }
}
