using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetAnyKey : MonoBehaviour
{
    public UnityAction[] actions;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (var action in actions)
            {
                action.Invoke();
            }
        }
    }
}
