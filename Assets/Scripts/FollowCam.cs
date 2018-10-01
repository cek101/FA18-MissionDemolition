﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public GameObject POI; // the static point of interest
    [Header("Set in inspector")]
    public float        easing = 0.05f;
    public Vector2      minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float            camZ; // the desired Z pos of the camera
    void Awake()    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()  {
        // if there's only one line following an if, it doesn't need braces
        if (POI == null) return; // return if there is no  poi

        // get the position of the poi
        Vector3 destination = POI.transform.position;
        // limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Interpolate from the current camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        // set the camera to the destination
        transform.position = destination;
        // set the orthographicSize of the camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;

    }
    // Use this for initialization
    //void Start () {
		
}
	
	// Update is called once per frame
	//void Update () {
		
	//}
//}
