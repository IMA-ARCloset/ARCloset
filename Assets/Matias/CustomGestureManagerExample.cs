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
    Gesture _gesture1, _gesture2;
    DiscreteGestureResult gesture1, gesture2, gesture3, gesture4;

    public CanvasController canvasController;

    public void SetTrackingId(ulong id)
    {
        _gestureFrameReader.IsPaused = false;
        _gestureFrameSource.TrackingId = id;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }

    // Use this for initialization
    void Start()
    {
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
                gesture1 = gesture2 = gesture3 = gesture4 = null;
                
                gesture1 = frame.DiscreteGestureResults[_gesture1];
                gesture2 = frame.DiscreteGestureResults[_gesture2];
               
                Do_things(frame);
            }
        }
    }

    void Do_things(VisualGestureBuilderFrame frame)
    {
        if (gesture1.Detected == true)
        {
            canvasController.OptionGesture();
        } 
        else if (gesture2.Detected == true && gesture2.Confidence > 0.9f)
        {
            //EFECTOS ESPECIALES
        }
    }
}
