using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggerActions : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private LayerMask layer;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (collision.gameObject.layer | (1 << layer)))
        {
            action.Invoke();
        }
    }

}
