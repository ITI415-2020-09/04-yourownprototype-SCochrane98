using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
       [Header("Set in Inspector")]                                   
    public GameObject prefabPinball;
    public float velocityMult = 8f;
       [Header("Set Dynamically")]                                        
    public GameObject launchPoint;
    public Vector3 launchPos;                                  
    public GameObject Pinball;                                 
    public bool aimingMode;
    public int lives = 3;
    private Rigidbody PinballRigidbody;
    static public Vector3 LAUNCH_POS
    {                                       
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
       void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");              
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    
    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
    void OnMouseDown()
    {                                                    
        aimingMode = true;
        Pinball = Instantiate(prefabPinball) as GameObject;

        Pinball.transform.position = launchPos;
        PinballRigidbody = Pinball.GetComponent<Rigidbody>();
        PinballRigidbody.isKinematic = true;
    }
    void Update()
    {
        if (!aimingMode) return; 
        Vector3 mousePos2D = Input.mousePosition;                               
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        Pinball.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {                                        
            aimingMode = false;
            PinballRigidbody.isKinematic = false;
            PinballRigidbody.velocity = -mouseDelta * velocityMult;
            Pinball = null;
            PinpointPinball.LivesLost();
            PinballProjection.S.poi = Pinball;
        }
    }
}
