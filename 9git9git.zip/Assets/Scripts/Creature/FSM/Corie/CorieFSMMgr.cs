using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorieFSMMgr : EnemyBaseFSMMgr
{
    public bool startRun;

    public GameObject dashCollider;

    bool isCrash;


    private new void Awake()
    {
        isCrash = false;
        
        base.Awake();
    }

    public override void Damaged(float demage)
    {
        Status.Hp -= demage;

        if (!isCrash)
        {
            isCrash = true;
        }
        isStun = true;
        animator.SetTrigger("stun");
        Invoke("endStun", 2);
        if (Status.Hp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        animator.SetTrigger("dead");
        Invoke("DestroyObj", 2f);
    }
    private void DestroyObj()
    {
        Destroy(this.gameObject);
    }
    public void spawn()
    {
        currentState = new CorieIdleState().Instance();
        currentState.Begin(this);
    }

    public void Run()
    {
        dashCollider.SetActive(true);
        startRun = true;
    }

    public void endStun()
    {
        ChangeState(new CorieAttackState().Instance());
        isStun = false;
    }
}
