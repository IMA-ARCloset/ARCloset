using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kinect = Windows.Kinect;

public class ArmatureController : MonoBehaviour
{
    private GameObject bView;
    public Transform Neck;
    private BodySourceView bViewScript;
    // Use this for initialization
    void Start()
    {
        bView = GameObject.Find("BodyView");
        bViewScript = bView.GetComponent<BodySourceView>();
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject bodyObject;
        // //Debug.Log(bViewScript.l_bodysTracked.Count);
        // if (bViewScript.l_bodysTracked.Any())
        // {
        //     //Debug.Log("CUERPOS EN ESCENA -> " + bViewScript.l_bodysTracked.Count);
        //     Kinect.Body body = bViewScript.l_bodysTracked[0];
        //     bodyObject = bViewScript._Bodies[body.TrackingId];

        //     for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        //     {
        //         if (jt == Kinect.JointType.Neck)
        //         {
        //             Transform jointObj = bodyObject.transform.Find(jt.ToString());
        //             //Debug.Log(transform.Find("Neck").position);
        //             //Neck = transform.Find("Neck");
        //             //if (Neck != null)
        //             //{   
        //             //    Neck.rotation = jointObj.rotation;
        //             //}
        //             //else
        //             //    Debug.Log("NO");
        //         }
        //     }
        // }
    }
}
