using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public UnityEngine.PostProcessing.PostProcessingProfile pP;

    public void Disable_bloom()
    {
        pP.bloom.enabled = false;
    }

    public void Enable_bloom()
    {
        pP.bloom.enabled = true;
    }

}
