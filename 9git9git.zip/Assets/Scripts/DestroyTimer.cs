using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private bool autoStart = true;

    private void Start()
    {
        if (autoStart) StartTimer();
    }

    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
