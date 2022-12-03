using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorieIdleState : EnemyBaseState
{
    static private EnemyBaseState FSMState = null;
    static public EnemyBaseState fSMState { get { return FSMState; } }
    public override EnemyBaseState Instance()
    {
        if (FSMState == null)
        {
            FSMState = this;
        }
        return fSMState;
    }

    float attackTime;
    float idleTime;

    public override void Begin(EnemyBaseFSMMgr mgr)
    {
        mgr.animator.SetTrigger("idle");
        attackTime = 0;
        idleTime = Random.Range(3, 10);
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        attackTime += Time.deltaTime;
        Debug.DrawRay(mgr.transform.position, Vector3.left * 10);
        Debug.DrawRay(mgr.transform.position, Vector3.right * 10);

        if (attackTime >= mgr.Status.AttackSpeed)
        {
            if (Physics2D.Raycast(mgr.transform.position, Vector3.right, 10, 1 << 7)
       || Physics2D.Raycast(mgr.transform.position, Vector3.left, 10, 1 << 7))
            {
                mgr.ChangeState(new CorieAttackState().Instance());
            }
        }

        if (attackTime > idleTime)
        {
            mgr.animator.SetTrigger("dig");
            idleTime = attackTime + Random.Range(3, 10);
        }
           


    }
    public override void End(EnemyBaseFSMMgr mgr)
    {

    }
}
