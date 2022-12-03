using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light2DNoise : MonoBehaviour
{
    [SerializeField] private float intensity = 1.0f;
    [SerializeField] private float range = 1.0f;
    [SerializeField] private float ScrollSpeed = 1.0f;

    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        light2D.intensity = (Mathf.PerlinNoise(Time.time * ScrollSpeed, 10f)-0.5f)*range + intensity; 
    }

}
