using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countryText;

    [SerializeField] private GameObject infoScrollPanel;
    [SerializeField] private TMP_Text countryInnerText;
    [SerializeField] private Image flagImage;
    [SerializeField] private Image mapImage;

    //todo: add video
    [SerializeField] private TMP_Text description;

    private JsonLoader jsonLoaderScript;

    void Start()
    {
        jsonLoaderScript = FindObjectOfType<JsonLoader>();

        infoScrollPanel.SetActive(false);
    }

    public void CountrySelected(int countryID)
    {
        var info = jsonLoaderScript.GetInfoByID(countryID);
        countryText.text = "Страна: " + info.name;

        infoScrollPanel.SetActive(true);

        countryInnerText.text = info.name;
    }


}
