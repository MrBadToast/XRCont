using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenotIdleState : EnemyBaseState
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
    float IdleTime;
    float curTime;
    public override void Begin(EnemyBaseFSMMgr mgr)
    {

        IdleTime = Random.Range(0,10);
        curTime = 0;
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        curTime += Time.deltaTime;

        mgr.transform.rotation = Quaternion.RotateTowards(
    mgr.transform.rotation, Quaternion.Euler(0, 0, 0), 150 * Time.deltaTime);

        if (IdleTime < curTime)
        {
            mgr.ChangeState(new SenotMoveState().Instance());
        }
    }
    public override void End(EnemyBaseFSMMgr mgr)
    {

    }
}
