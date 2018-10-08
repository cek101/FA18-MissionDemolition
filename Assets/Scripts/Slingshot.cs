using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    static private Slingshot S;
    // fields set in the unity inspector
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    // fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingmode;

    //private Rigidbody       projectileRigidbody;

    void Awake() {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
    }

    void OnMouseEnter() {
        //print("Slingshot: OnMouseEnter()"); 
        launchPoint.SetActive(true);

    }

    void OnMouseExit() {
        //print("Slingshot: OnMouseExit");
        launchPoint.SetActive(false);
    }
    // Use this for initialization
    void OnMouseDown () {
        // the player has pressed the mouse button while over Slingshot
        aimingmode = true;
        // instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        // start it at the launchPoint
        projectile.transform.position = launchPos;
        // set it to isKinematic for now
        //projectile.GetComponent<Rigidbody>().isKinematic = true; //replace by the next two following lines
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
        // if Slingshot is not in aimingMode, don't run this code
        if (!aimingmode) return;

        // get the current mouse position in 2d screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        // find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude> maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if ( Input.GetMouseButtonUp (0) )   {
            // the mouse has been released
            aimingmode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
	}
}
