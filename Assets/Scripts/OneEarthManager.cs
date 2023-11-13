using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class OneEarthManager : MonoBehaviour
{
    private ProgrammManager mainManager;
    private Camera ARCamera;

    //public GameObject QuizPanelPrefab;
    private GameObject QuizPanel;

    [SerializeField] private Vector3 QuizPanelGap;

    public GameObject CountryPanelPrefab;
    private GameObject CountryPanel;

    // public bool isEarth()
    // {
    //     return true;
    // }

    void Start()
    {
        mainManager = FindObjectOfType<ProgrammManager>();
        ARCamera = GameObject.FindWithTag(Consts.TAG_CAMERA).GetComponent<Camera>();
        Debuger.LogFormat("camera in earth cords {0}, {1}", ARCamera.transform.position, ARCamera.transform.rotation);
    }

    void Update()
    {
        PanelFollow(QuizPanel);
        PanelFollow(CountryPanel);


    }

    public void Select()
    {
        if (gameObject.CompareTag(Consts.TAG_UNSELECTED))
        {
            gameObject.tag = Consts.TAG_SELECTED;
        }
    }

    public void TogglePanels()
    {
        FindQuizPanel();
        TogglePanelsIfExists();
    }

    // private void CreateQuizPanel() {
    //     if (QuizPanel == null)
    //     {

    //         Debuger.LogFormat("earth cords {0}", transform.position);

    //         //transform.position + QuizPanelGap
    //         //QuizPanel = Instantiate(QuizPanelPrefab, transform.position + QuizPanelGap, ARCamera.transform.rotation);
    //         return;
    //     }
    // }

    private void FindQuizPanel()
    {
        if (QuizPanel == null)
        {
            QuizPanel = gameObject.transform.Find("QuizPanel").gameObject;
        }
    }

    private void TogglePanelsIfExists()
    {
        TogglePanel(QuizPanel);
        TogglePanel(CountryPanel);
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


}
