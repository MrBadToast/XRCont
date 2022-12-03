using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableVisual : MonoBehaviour
{
    public void ToggleSelectionVisual(bool value)
    {
        if (value)
            GetComponent<Renderer>().material.SetInt("_IsActive", 1);
        else
            GetComponent<Renderer>().material.SetInt("_IsActive", 0);
    }

    public void SetVanishAlertVisual(bool active, float speed)
    {
        if (active)
        {
            GetComponent<Renderer>().material.SetInt("_Alert", 1);
            GetComponent<Renderer>().material.SetFloat("_Alert_speed", speed);
        }
        else
            GetComponent<Renderer>().material.SetInt("_Alert", 0);
    }

    public void reset()
    {
        GetComponent<Renderer>().material = null;
    }
}
