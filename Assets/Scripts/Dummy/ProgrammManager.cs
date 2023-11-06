using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgrammManager : MonoBehaviour
{

    [SerializeField] private GameObject PlaneMarkerPrefab;

    private ARRaycastManager ARRaycastManagerScript;

    void Start()
    {
        PlaneMarkerPrefab.SetActive(false);
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowMarker(); 
    }

    void ShowMarker()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


        if (hits.Count > 0)
        {
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);
        }
    }
}
