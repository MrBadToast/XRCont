using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenotFSMMgr : EnemyBaseFSMMgr
{
    private new void Awake()
    {
        base.Awake();
        currentState = new SenotIdleState().Instance();
        currentState.Begin(this);
    }

    public override void Die()
    {
        animator.SetTrigger("dead");
        Invoke("DestroyObj", 0.7f);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
