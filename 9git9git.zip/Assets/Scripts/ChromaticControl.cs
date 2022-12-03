using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticControl : MonoBehaviour
{
    public bool toggle = false;
    public float c_value;
    public float d_value;
    public Volume vol;

    private ChromaticAberration chroma;
    private LensDistortion disto;

    private void Awake()
    {
        vol.profile.TryGet(out chroma);
        vol.profile.TryGet(out disto);
    }

    private void Update()
    {
        if(toggle)
        {
            chroma.intensity.value = c_value;
            disto.intensity.value = d_value;
        }
    }
}
