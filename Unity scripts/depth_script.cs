using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class depth_script : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMesh;
    //public string newText = "Hello, World!";

    void Start()
    {
        // Get the TextMesh component on a child GameObject of the Canvas


        ROSConnection.GetOrCreateInstance().Subscribe <FluidPressureMsg>("mavros/imu/diff_pressure", PressureChange);



        void PressureChange(FluidPressureMsg fluidMessage)
        {
            float pressure = (float) fluidMessage.fluid_pressure;
            float gustoca = 1000.0f;
            float g = 9.81f;
            float depth = pressure / (gustoca * g);
            textMesh.text = Mathf.Round(depth * 10.0f) / 10.0f + " m";
            
        }


    }
}