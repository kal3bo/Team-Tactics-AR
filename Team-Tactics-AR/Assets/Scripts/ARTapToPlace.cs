using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{
    // Private Variables:
    [SerializeField] private GameObject placementIndicator = null;
    [SerializeField] private GameObject[] players = null;
    [SerializeField] private GameObject yellowTeamActive = null;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementIsValid;
    private int currentIndexAnimation = 0;


    // Private Methods:
    
    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    /// <summary>
    /// Every frame checks if the player can place an object by touching the screen.
    /// </summary>
    private void Update()
    {
        PlaceIndicator();

        if (placementIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    /// <summary>
    /// Spawn a raycast from the camera to the next plane in which the AR system is capable
    /// of recognize. Place a gameobject at the end of the raycast in case is correct and
    /// uses the transform of the phone to place the gameobjects with the same orientation.
    /// </summary>
    private void PlaceIndicator()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementIsValid = hits.Count > 0;
        if (placementIsValid)
        { 
            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            

            placementPose = hits[0].pose;
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }

    private void PlaceObject()
    {
        Instantiate(players[currentIndexAnimation], placementPose.position, placementPose.rotation);
    }

    public void SwitchAnimation(int indexOfAnimation)
    {
        if(indexOfAnimation <= 3)
        {
            if (!yellowTeamActive.activeSelf)
            {
                currentIndexAnimation = indexOfAnimation + 4;
            }
            else
            {
                currentIndexAnimation = indexOfAnimation;
            }
        }
        else
        {
            currentIndexAnimation = indexOfAnimation;
        }
    }
}