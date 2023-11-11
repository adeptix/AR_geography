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
    

    public bool Rotation; 
    private Quaternion YRotation; 

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
}