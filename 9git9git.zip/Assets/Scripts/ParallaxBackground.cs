using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundLayer
{
    public Transform backgroundTF;
    [Range(0,1f)]
    public float x_depth;
    [Range(0, 1f)]
    public float y_depth;

    private Vector3 anchorPoint;
    public Vector3 AnchorPoint { get { return anchorPoint; }}

    public void RefreshAnchorPoint() { anchorPoint = backgroundTF.localPosition; }
}

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    [SerializeField] BackgroundLayer[] backgroundLayers;
    [SerializeField] private float x_mult;
    [SerializeField] private  float y_mult;

    private void OnEnable()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        foreach(var bg in backgroundLayers)
        {
            bg.RefreshAnchorPoint();
        }
    }

    private void FixedUpdate()
    {
        Vector3 camPos = targetCamera.transform.position;
        foreach (var bg in backgroundLayers)
        {
            bg.backgroundTF.localPosition = new Vector3(bg.AnchorPoint.x - camPos.x*bg.x_depth*x_mult,bg.AnchorPoint.y - camPos.y * bg.y_depth*y_mult);
        
        }
    }

}
