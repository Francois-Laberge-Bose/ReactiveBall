using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using System;
 
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPosition;
    public GameObject placementIndicator;
    private ARSessionOrigin arOrigin;
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool placed = false;
 
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }
 
    void Update()
    {
       UpdatePlacementPose();
       UpdatePlacementIndicator();
 
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }
 
    private void PlaceObject()
    {
        objectToPosition.transform.position = placementPose.position;
        placementIndicator.SetActive(false);
        placed = true;
    }
 
    private void UpdatePlacementIndicator()
    {
        if(placed){
            return;
        }
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
 
    private void UpdatePlacementPose()
    {
        
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        Debug.Log("hits: " + hits.Count);
        placementPoseIsValid = hits.Count > 0;

        
 
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            
        }
    }
 
}
 