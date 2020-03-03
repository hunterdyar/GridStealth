using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
public class CameraColorUpdater : MonoBehaviour
{
    public ColorReference lightColor;
    void Start()
    {
        GetComponent<Camera>().backgroundColor = lightColor.Value;
    }

}
