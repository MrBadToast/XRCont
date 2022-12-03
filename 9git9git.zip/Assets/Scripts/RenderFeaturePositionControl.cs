using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFeaturePositionControl : MonoBehaviour
{
    [SerializeField] private Material sharedDistortionMat;
    [SerializeField] private AnimationCurve spreadCurve;
    Camera cam;

    private static RenderFeaturePositionControl instance;
    public static RenderFeaturePositionControl Instance { get { return instance; } }

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        sharedDistortionMat.SetFloat("_Progress", 0f);
        sharedDistortionMat.SetFloat("_Progress", 0.04f);
    }

    public void SpreadDistortion(Vector2 position, float maxSpread, float time)
    {
        StopAllCoroutines();
        StartCoroutine(Cor_SpreadDistortion(position, maxSpread, time));
    }

    IEnumerator Cor_SpreadDistortion(Vector2 position, float maxSpread, float time)
    {
        Vector2 WorldToScreen = cam.WorldToViewportPoint(position);
        sharedDistortionMat.SetVector("_Center", new Vector4(WorldToScreen.x, WorldToScreen.y));

        for (float t = 0.15f; t < time; t += Time.fixedDeltaTime)
        {
            float cur = spreadCurve.Evaluate(t / time);
            sharedDistortionMat.SetFloat("_Progress", (t / time) * maxSpread *cur);
            sharedDistortionMat.SetFloat("_Size", (1-(t / time)) * 0.2f * cur);
            yield return new WaitForFixedUpdate();
        }
        sharedDistortionMat.SetFloat("_Progress", 0f);
        sharedDistortionMat.SetFloat("_Progress", 0.04f);
    }

    //private void Update()
    //{
    //    Vector2 WorldToScreen = cam.WorldToViewportPoint(Vector3.zero);
    //    sharedDistortionMat.SetVector("_Center", new Vector4(WorldToScreen.x, WorldToScreen.y));
    //}
}
