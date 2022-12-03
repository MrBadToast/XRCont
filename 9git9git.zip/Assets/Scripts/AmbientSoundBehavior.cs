using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTF;
    [SerializeField] GameObject triggerObject;

    private void Update()
    {
        if (ClapsManager.Instance.IsClapZone(playerTF.position))
        {
            triggerObject.SetActive(false);
        }
        else
        {
            triggerObject.SetActive(false);
            triggerObject.SetActive(true);
        }
    }
}
