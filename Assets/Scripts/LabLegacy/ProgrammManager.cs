using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgrammManager : MonoBehaviour
{

    [SerializeField] private GameObject PlaneMarkerPrefab;
    [SerializeField] private Camera ARCamera;

    [SerializeField] private Button AddObjectButton;
    [SerializeField] private GameObject ObjectToSpawn;

    private ARRaycastManager ARRaycastManagerScript;
    private ButtonLocker ButtonLockerScript;
    //public GameObject ScrollView;

    public bool ChooseObject = false;

    // why global ?
    //private Vector2 TouchPosition;
    //private GameObject SelectedObject;
    public bool Moving;

    // -------------- ROTATION --------------------
    public bool Rotation;
    private Quaternion YRotation;

    // -------------- COMMUNICATION --------------------
    public bool Communication;


    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        ButtonLockerScript = FindObjectOfType<ButtonLocker>();

        PlaneMarkerPrefab.SetActive(false);
        //ScrollView.SetActive(false);
    }


    void Update()
    {
        if (ChooseObject)
        {
            ShowMarkerAndSetObject();
        }
        else
        {
            PlaneMarkerPrefab.SetActive(false);
        }

        TouchHandle();
    }

    void TouchHandle()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hitObject))
                {
                    if (hitObject.collider.gameObject.TryGetComponent(out OneEarthManager m))
                    {
                        if (Moving || Rotation) m.Select();      // SELECT OBJECT FOR MOVE / ROTATE
                        if (Communication) m.TogglePanels();
                    }
                }
            }

            var selectedObject = GameObject.FindWithTag(Consts.TAG_SELECTED);
            var hits = new List<ARRaycastHit>();

            if (selectedObject == null) return;

            if (touch.phase == TouchPhase.Moved)
            {
                if (Moving)
                {
                    ARRaycastManagerScript.Raycast(touch.position, hits, TrackableType.Planes);
                    selectedObject.transform.position = hits[0].pose.position;
                }

                if (Rotation)
                {
                    YRotation = Quaternion.Euler(0f, -touch.deltaPosition.x * 0.1f, 0f);
                    selectedObject.transform.rotation = YRotation * selectedObject.transform.rotation;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (selectedObject.CompareTag(Consts.TAG_SELECTED))
                {
                    selectedObject.tag = Consts.TAG_UNSELECTED; // DE-SELECT OBJECT FOR MOVE / ROTATE
                }
            }
        }
    }


    void ShowMarkerAndSetObject()
    {
        var hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                // if (ClickOnUIFix())
                // {
                //     return;
                // }

                Instantiate(ObjectToSpawn, hits[0].pose.position, ObjectToSpawn.transform.rotation);
                PlaneMarkerPrefab.SetActive(false);

                ButtonLockerScript.HasObject = true;
                AddObjectButton.GetComponent<AddObject>().EmulateClick();
            }
        }
    }

    //TODO : maybe later
    // bool ClickOnUIFix()
    // {
    //     if (Input.touchCount == 0) return false;

    //     Ray ray = ARCamera.ScreenPointToRay(Input.GetTouch(0).position);

    //     //EventSystem.current.RaycastAll(Input.GetTouch(0).position, )

    //     if (Physics.Raycast(ray, out RaycastHit hitObject))
    //     {
           
    //         var gameObj = hitObject.collider.gameObject;

    //         Debuger.LogFormat("UI clicked {0}", gameObj.name);
    //         return hitObject.collider.CompareTag(Consts.TAG_UI); // IF CLICK ON UI - DO NOT PLACE OBJECT
    //     }

    //     Debuger.Log("none clicked");

    //     return false;
    // }
}
