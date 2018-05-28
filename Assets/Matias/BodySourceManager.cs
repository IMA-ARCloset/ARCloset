using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Kinect.VisualGestureBuilder;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour
    {
        private KinectSensor _Sensor;
        private BodyFrameReader _Reader;
        private Body[] _Data = null;
        private ulong _trackingId = 0;
     
        public GameObject GestureManagerObject;
        private CustomGestureManagerExample GestureManager;
     
        public Body[] GetData()
        {
            return _Data;
        }
     
        void Start()
        {
            GestureManager = GestureManagerObject.GetComponent<CustomGestureManagerExample>();
            _Sensor = KinectSensor.GetDefault(); // Recoge el kinect

            /*
                En caso de que haya un kinect conectado crea un nuevo stream para recibir
                datos y abre el sensor del kinect en caso de que este no este abierto
            */
            if (_Sensor != null) 
            {
                _Reader = _Sensor.BodyFrameSource.OpenReader(); 
     
                if (!_Sensor.IsOpen)
                {
                    _Sensor.Open();
                }
            }

            
        }
     
        void Update()
        {
            
            if (_Reader != null) // Este reader se debe crear en el Start()
            {
                var frame = _Reader.AcquireLatestFrame(); // Conseguimos los datos del ultimo frame

                if (frame != null)
                {
                    if (_Data == null) // Comprueba si hay un array de cuerpos ya existente y lo crea en caso contrario
                    {
                        _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                    }
     
                    _trackingId = 0; // Inicializamos el id por si las moscas aunque se hace abajo
                    frame.GetAndRefreshBodyData(_Data); // Transfiere los datos contenidos en el frame al array de datos
     
                    frame.Dispose(); // borramos el frame
                    frame = null; 
     
                    foreach (var body in _Data)
                    {
                        if (body != null && body.IsTracked)
                        {
                            _trackingId = body.TrackingId; // Recogemos el id del cuerpo en cuestion
                            if (GestureManager != null) 
                            {
                                GestureManager.SetTrackingId(body.TrackingId);
                            }
                            break;
                        }
                    }
                }
            }
        }
     
        /*
            Cierra todos los streams y los datos que hayamos preparado
        */
        void OnApplicationQuit()
        {
            if (_Reader != null)
            {
                _Reader.Dispose();
                _Reader = null;
            }
     
            if (_Sensor != null)
            {
                if (_Sensor.IsOpen)
                {
                    _Sensor.Close();
                }
     
                _Sensor = null;
            }
        }
    }
