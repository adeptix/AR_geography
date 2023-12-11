using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class OneEarthManager : MonoBehaviour
{
    private ProgrammManager mainManager;
    private Camera ARCamera;

    [SerializeField] private GameObject Earth;
    [SerializeField] private GameObject EarthGroup;
    [SerializeField] private GameObject QuizPanel;
    [SerializeField] private GameObject InfoPanel;

    [SerializeField] private Vector3 QuizPanelGap;
    
    private QuizManager quizManagerScript;
    private InfoManager infoManagerScript;

    private GameObject lastPin = null;

    void Start()
    {
        mainManager = FindObjectOfType<ProgrammManager>();
        ARCamera = GameObject.FindWithTag(Consts.TAG_CAMERA).GetComponent<Camera>();
        Debuger.LogFormat("camera in earth cords {0}, {1}", ARCamera.transform.position, ARCamera.transform.rotation);

        quizManagerScript = QuizPanel.GetComponent<QuizManager>();
        infoManagerScript = InfoPanel.GetComponent<InfoManager>();
    }

    void Update()
    {
        PanelFollow(QuizPanel);
        PanelFollow(InfoPanel);
    }

    public void Select(bool selectAll)
    {
        if (!selectAll) {
            SelectOnlyEarth();
            return;
        }

        if (gameObject.CompareTag(Consts.TAG_UNSELECTED))
        {
            gameObject.tag = Consts.TAG_SELECTED;
        }
    }

    private void SelectOnlyEarth() {
        if (EarthGroup == null) return;

        if (EarthGroup.CompareTag(Consts.TAG_UNSELECTED))
        {
            EarthGroup.tag = Consts.TAG_SELECTED;
        }
    }

    public void TogglePanels()
    {
        TogglePanelsIfExists();
    }

    private void TogglePanelsIfExists()
    {
        TogglePanel(QuizPanel);
        TogglePanel(InfoPanel);
    }

    private void TogglePanel(GameObject panel)
    {
        if (panel == null)
        {
            return;
        }

        Debuger.LogFormat("panel cords {0}", panel.transform.position);
        Debuger.LogFormat("panel rot {0}", panel.transform.rotation);
        Debuger.LogFormat("panel active {0}", panel.activeSelf);

        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

    private void PanelFollow(GameObject panel)
    {
        if (panel == null)
        {
            return;
        }

        panel.transform.rotation = Quaternion.LookRotation(panel.transform.position - ARCamera.transform.position);
    }

    public void CountrySelected(int countryID, GameObject pin) {
        StartRotateEarth(pin);
        if (lastPin != null && lastPin != pin) {
            lastPin.GetComponent<PinManager>().Deselect();
        }

        lastPin = pin;

        quizManagerScript.CountrySelected(countryID);
        infoManagerScript.CountrySelected(countryID);
    }


    private void StartRotateEarth(GameObject pin) {
        Vector3 planetToCity = (pin.transform.localPosition - Vector3.zero).normalized; //vector that points from planet to city
        Vector3 planetToCamera = (ARCamera.transform.position - EarthGroup.transform.position).normalized; //vector that points from planet to camera
               
        Quaternion a = Quaternion.LookRotation(planetToCamera);
        Quaternion b = Quaternion.LookRotation(planetToCity);
        
        Quaternion newRotation = a * Quaternion.Inverse(b);

        StartCoroutine(RotateOverTime(EarthGroup, newRotation, 1.5f));
    }

    private IEnumerator RotateOverTime(GameObject targetObject, Quaternion end, float durationSeconds) {
        Quaternion start = targetObject.transform.rotation;
        
        float t = 0f;
        while(t < durationSeconds)
        {
            targetObject.transform.rotation = Quaternion.Slerp(start, end, t / durationSeconds);
            yield return null;
            t += Time.deltaTime;
        }
        targetObject.transform.rotation = end;
    }
}
