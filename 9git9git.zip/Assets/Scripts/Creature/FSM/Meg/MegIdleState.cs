using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegIdleState : EnemyBaseState
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

    public override void Begin(EnemyBaseFSMMgr mgr)
    {
        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.idleCollider.SetActive(true);
        mgr.animator.SetTrigger("idle");
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        mgr.transform.rotation = Quaternion.RotateTowards(
            mgr.transform.rotation, Quaternion.Euler(0, mgr.transform.rotation.eulerAngles.y, 0), 200 * Time.deltaTime);
        
        if (mgr.CheckInAttackRange())
        {
            mgr.ChangeState(new MegAttackState().Instance());
        }
      
    }

    public override void End(EnemyBaseFSMMgr mgr)
    {
        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.idleCollider.SetActive(false);
    }
}
