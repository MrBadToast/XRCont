using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoriedashCollider : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 7)
        {
            CorieFSMMgr cMgr = transform.GetComponentInParent<CorieFSMMgr>();
            cMgr.ChangeState(new CorieIdleState().Instance());    
        }
    }
}
