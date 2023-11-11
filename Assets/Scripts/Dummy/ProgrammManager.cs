using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgrammManager : MonoBehaviour
{

    [SerializeField] private GameObject PlaneMarkerPrefab;
    [SerializeField] private Camera ARCamera;



    public GameObject ScrollView;
    public GameObject ObjectToSpawn;
    public bool ChooseObject = false;

    private ARRaycastManager ARRaycastManagerScript;

    List<ARRaycastHit> hits = new List<ARRaycastHit>(); // why global ?
    private Vector2 TouchPosition;
    private GameObject SelectedObject;
    public bool Moving;

    // -------------- ROTATION --------------------
    public bool Rotation;
    private Quaternion YRotation;

    // -------------- COMMUNICATION --------------------
    public bool Communication;

    public GameObject VirtualDisplayPrefab;
    private GameObject VirtualDisplay;

    public Transform CameraPos; //todo: this is camera position actually, rename


    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();

        PlaneMarkerPrefab.SetActive(false);
        ScrollView.SetActive(false);
    }


    void Update()
    {
        if (ChooseObject)
        {
            ShowMarkerAndSetObject();
        }

        MoveObjectAndRotation();

        ShowVirtualDisplay();
    }

    void MoveObjectAndRotation()
    {
        if (Input.touchCount > 0)
        {
            // Отслеживание места нажатия пальца на экран
            Touch touch = Input.GetTouch(0);
            TouchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("UnSelected"))
                    {
                        hitObject.collider.gameObject.tag = "Selected";
                    }
                }
            }

            SelectedObject = GameObject.FindWithTag("Selected");

            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                if (Moving)
                {
                    ARRaycastManagerScript.Raycast(TouchPosition, hits, TrackableType.Planes);
                    SelectedObject.transform.position = hits[0].pose.position;
                }

                if (Rotation)
                {
                    YRotation = Quaternion.Euler(0f, -touch.deltaPosition.x * 0.1f, 0f);
                    SelectedObject.transform.rotation = YRotation * SelectedObject.transform.rotation;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (SelectedObject.CompareTag("Selected"))
                {
                    SelectedObject.tag = "UnSelected";
                }
            }
        }
    }

    void ShowMarkerAndSetObject()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);
            Debug.Log("ADEPT - AR raycast hit");

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                Debug.Log("ADEPT - touch begin");
                Instantiate(ObjectToSpawn, hits[0].pose.position, ObjectToSpawn.transform.rotation);
                ChooseObject = false;
                PlaneMarkerPrefab.SetActive(false);
            }
        }
    }

    void ShowVirtualDisplay()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchPosition = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (Communication)
                    {
                        Debug.LogFormat("ADEPT - HIT RAYCAST COMMUNICATION {0}", hitObject.transform.position);
                        
                        //  ARCamera.transform.position + new Vector3(0, 1f, 0)
                        // Появление "летающего" виртуального экрана
                        VirtualDisplay = Instantiate(VirtualDisplayPrefab, hitObject.transform.position + new Vector3(-1f, 1f, 0), ARCamera.transform.rotation);
                    }
                }
            }
        }

        if (VirtualDisplay == null) {
            return;
        }

        DisableDisplayOnClick();

        // Необходимо, чтобы виртуальный экран следовал за пользователем и всегда был в его поле зрения
        // Проверка на дистанцию
        // if (CheckDist() >= 0.1f)
        // {
        //     MoveObjToPos();
        // }

        VirtualDisplay.transform.LookAt(ARCamera.transform);
    }

    public float CheckDist()
    {
        float dist = Vector3.Distance(VirtualDisplay.transform.position, CameraPos.transform.position);
        return dist;
    }

    private void MoveObjToPos()
    {
        VirtualDisplay.transform.position = Vector3.Lerp(VirtualDisplay.transform.position, CameraPos.position, 1f * Time.deltaTime);
    }

    private void DisableDisplayOnClick()
    {
        if (Communication == false)
        {
            VirtualDisplay.SetActive(false);
        }
    }

}
