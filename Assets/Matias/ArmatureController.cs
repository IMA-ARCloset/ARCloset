using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kinect = Windows.Kinect;

public class ArmatureController : MonoBehaviour
{

    private GameObject bView;
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
        //Debug.Log(bViewScript.l_bodysTracked.Count);
        if (bViewScript.l_bodysTracked.Any())
        {
            Debug.Log("CUERPOS EN ESCENA -> " + bViewScript.l_bodysTracked.Count);
            Kinect.Body body = bViewScript.l_bodysTracked[0];


            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                Kinect.Joint sourceJoint = body.Joints[jt];
                Kinect.Joint? targetJoint = null;

                if (_BoneMap.ContainsKey(jt))
                {
                    targetJoint = body.Joints[_BoneMap[jt]];
                }
            }


        }
    }
}
