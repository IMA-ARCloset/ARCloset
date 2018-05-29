using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Kinect.VisualGestureBuilder;
using Windows.Kinect;

public class CustomGestureManagerExample : MonoBehaviour
{
    VisualGestureBuilderDatabase _gestureDatabase; // La base de datos 
    VisualGestureBuilderFrameSource _gestureFrameSource; // stream 
    VisualGestureBuilderFrameReader _gestureFrameReader; // stream
    KinectSensor _kinect;
    Gesture _salute;
    Gesture _saluteProgress;
    Gesture _gesture1, _gesture2, _gesture3, _gesture4;
    Gesture _gesture1Progress, _gesture2Progress, _gesture3Progress, _gesture4Progress;
    DiscreteGestureResult gesture1, gesture2, gesture3, gesture4;

    Renderer cubeColor;
    public GameObject AttachedObject; // El objeto que cambiaremos en funcion de nuestros
    public Animator settingsIconAnim;
    public Animator settingsPanelAnim;

    public bool settingsOpened, transition;

    public void SetTrackingId(ulong id)
    {
        _gestureFrameReader.IsPaused = false;
        _gestureFrameSource.TrackingId = id;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }

    // Use this for initialization
    void Start()
    {
        if (AttachedObject != null) // Si existe el objeto a interactuar inicializa sus valores
        {
            // _ps = GetComponent<ParticleSystem>();//AttachedObject.particleSystem;
            // _ps.emissionRate = 4;
            // _ps.startColor = Color.blue;
            cubeColor = AttachedObject.GetComponent<Renderer>();
        }

        settingsOpened = false;
        transition = false;

        _kinect = KinectSensor.GetDefault(); // Recogemos el kinect por defecto

        _gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/ImaGestures.gbd"); // Recoge la base de datos
        _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0); // Array de gestos ????

        if (_gestureDatabase != null)
            Debug.Log("Base de datos cargada con éxito");
        else
            Debug.LogError("Error al cargar la base de datos");
        /*
            Anyade los gestos
            TODO: DEFINIR LOS NOMBRES DE ESTOS GESTOS DE MIERDA
        */
        foreach (var gesture in _gestureDatabase.AvailableGestures)
        {
            _gestureFrameSource.AddGesture(gesture);

            switch (gesture.Name)
            {
                case "Menu_Right":
                    _gesture1 = gesture;
                    break;
                case "Alas":
                    _gesture2 = gesture;
                    break;
                default:
                    Debug.Log(gesture.Name);
                break;
            }
        }

        _gestureFrameReader = _gestureFrameSource.OpenReader();
        _gestureFrameReader.IsPaused = true;
    }

    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {

        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {

            if (frame != null && frame.DiscreteGestureResults != null)
            {

                if (AttachedObject == null) // TODO: Modificar esto para tener varios objetos actuadores
                    return;


                /*
                    Desclaramos los gestos que vayamos a usar y los metemos en una lista
                */

                gesture1 = gesture2 = gesture3 = gesture4 = null;
                //DiscreteGestureResult[] l_discreteGestures = new DiscreteGestureResult[] { gesture1, gesture2, gesture3, gesture4 };

                /*
                    Rellenamos los gestos
                */
                
                gesture1 = frame.DiscreteGestureResults[_gesture1];
                // gesture2 = frame.DiscreteGestureResults[_gesture2];
                // gesture3 = frame.DiscreteGestureResults[_gesture3];
                gesture2 = frame.DiscreteGestureResults[_gesture2];
               
                Do_things(frame);

            }
        }
    }

    void Do_things(VisualGestureBuilderFrame frame)
    {
        if (gesture1.Detected == true)
        {
            if (!transition)
            {
                if (!settingsOpened)
                {
                    transition = true;
                    settingsIconAnim.Play("I_InitTopOptions");
                    settingsPanelAnim.Play("P_Open");
                    StartCoroutine(WaitTransition());
                }
                else
                {
                    transition = true;
                    settingsIconAnim.Play("I_OptionsToInit");
                    settingsPanelAnim.Play("P_Close");
                    StartCoroutine(WaitTransition());
                }
            }
        } 
        else if (gesture2.Detected == true && gesture2.Confidence > 0.9f)
        {
            if (AttachedObject != null)
            {
                cubeColor.material.color = new Color(0, 0, 0);
            }
        } 
        else
            cubeColor.material.color = new Color(1,1,1);
    }

    IEnumerator WaitTransition()
    {
        yield return new WaitForSeconds(2);
        transition = false;
        settingsOpened = !settingsOpened;
    }
}
