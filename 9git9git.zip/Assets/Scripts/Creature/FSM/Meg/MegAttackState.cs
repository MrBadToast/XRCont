using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegAttackState : EnemyBaseState
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
        
        mgr.transform.rotation = Quaternion.Euler(
            mgr.transform.rotation.eulerAngles.x, 
            mgr.transform.rotation.eulerAngles.y, 
            0.0f);
        mgr.animator.SetTrigger("fire");

        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.standCollider.SetActive(true);

    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
      

        if (mgr.transform.position.x > enemyManager.Instance.playerPos.position.x)
        {
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 180, mgr.transform.rotation.eulerAngles.z);  
        }
        else
        {
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 0, mgr.transform.rotation.eulerAngles.z);
        }

        if (mgr.CheckInView())
        {
            mgr.ChangeState(new MegTraceState().Instance());
        }
        else if (!mgr.CheckInAttackRange())
        {
            mgr.ChangeState(new MegIdleState().Instance());
        }
    }

    public override void End(EnemyBaseFSMMgr mgr)
    {
        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.standCollider.SetActive(false);
    }
}
