using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Sirenix.OdinInspector;

public class FMODSoundsTrigger : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, StudioEventEmitter> emitterContainer;

    public StudioEventEmitter PlaySound(string key)
    {
        StudioEventEmitter em;

        if (emitterContainer.TryGetValue(key, out em))
        {
            em.Play();
            return em;
        }
        else return null;
    }
}
