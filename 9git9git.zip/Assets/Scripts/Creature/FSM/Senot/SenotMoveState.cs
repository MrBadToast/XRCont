using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenotMoveState : EnemyBaseState
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

    bool isRight;
    float IdleTime;
    float curTime;
    public override void Begin(EnemyBaseFSMMgr mgr)
    {
        isRight = (Random.value > 0.5f);
        IdleTime = Random.Range(0, 10);
        curTime = 0;

        mgr.rig.constraints = RigidbodyConstraints2D.None;
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        curTime += Time.deltaTime;

        if (IdleTime < curTime)
        {
            mgr.ChangeState(new SenotIdleState().Instance());
        }

        if (isRight)
        {
            mgr.rig.velocity = new Vector2((mgr.Status.Speed * 100.0f * Time.deltaTime), mgr.rig.velocity.y);
        }
        else
        {
            mgr.rig.velocity = new Vector2(-(mgr.Status.Speed * 100.0f * Time.deltaTime), mgr.rig.velocity.y);
        }
    }
    public override void End(EnemyBaseFSMMgr mgr)
    {

        mgr.rig.constraints = RigidbodyConstraints2D.FreezePositionX
              | RigidbodyConstraints2D.FreezeRotation;
    }
}
