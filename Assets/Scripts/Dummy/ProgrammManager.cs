using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgrammManager : MonoBehaviour
{

    [SerializeField] private GameObject PlaneMarkerPrefab;

    public GameObject ScrollView; 
    public GameObject ObjectToSpawn;
    public bool ChooseObject = false; 


    private ARRaycastManager ARRaycastManagerScript;

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        
        PlaneMarkerPrefab.SetActive(false);
        ScrollView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChooseObject) {
            ShowMarker(); 
        }
    }

    void ShowMarker()
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
