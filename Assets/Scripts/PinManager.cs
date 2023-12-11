using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PinManager : MonoBehaviour
{
    [SerializeField] private OneEarthManager oneEarthManagerScript;
    [SerializeField] private int countryID;

    private bool isSelected;
    private Color defaultColor;
    private Material material;

    void Start()
    {
        isSelected = false;
        material = gameObject.GetComponent<Renderer>().material;
        defaultColor = material.GetColor("_Color");
    }

    public void ClickOnPin()
    {
        isSelected = true;
        ChangeColor();
        oneEarthManagerScript.CountrySelected(countryID, gameObject);

    }

    private void ChangeColor()
    {
        if (isSelected)
        {
            material.SetColor("_Color", Color.green);
        }
        else
        {
            material.SetColor("_Color", defaultColor);
        }
    }

    public void Deselect()
    {
        isSelected = false;
        ChangeColor();
    }
}
