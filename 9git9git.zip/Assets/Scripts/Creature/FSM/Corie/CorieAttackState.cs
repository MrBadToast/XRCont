using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorieAttackState : EnemyBaseState
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
    CorieFSMMgr Mmgr;
    int isRight;

    float curTime;

    public override void Begin(EnemyBaseFSMMgr mgr)
    {
        Mmgr = mgr as CorieFSMMgr;
        Mmgr.startRun = false;

        mgr.animator.SetTrigger("move");
        mgr.rig.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        isRight = (mgr.transform.position.x > enemyManager.Instance.playerPos.position.x) ? -1 : 1;
        if (isRight == -1)
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 180, mgr.transform.rotation.eulerAngles.z);
        else
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 0, mgr.transform.rotation.eulerAngles.z);

        curTime = 0;

     
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        if (!mgr.CheckInView())
        {
            mgr.ChangeState(new CorieIdleState().Instance());
        }


        if (Mmgr.startRun)
        {
            mgr.rig.velocity = new Vector2(isRight * ((mgr.Status.Speed - curTime * 2) * 100.0f * Time.deltaTime), mgr.rig.velocity.y);
        }

        if (mgr.transform.position.x > enemyManager.Instance.playerPos.position.x && isRight == 1)
        {
            curTime += Time.deltaTime;
        }
        else if (mgr.transform.position.x < enemyManager.Instance.playerPos.position.x && isRight == -1)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= 2.0f)
        {
            rotate();
        }
    }
    public override void End(EnemyBaseFSMMgr mgr)
    {
        mgr.rig.constraints = RigidbodyConstraints2D.FreezePositionX
              | RigidbodyConstraints2D.FreezeRotation;

        Mmgr.dashCollider.SetActive(false);
    }

    void rotate()
    {
        curTime = 0;
        if (isRight == -1)
        {
            isRight = 1;
            Mmgr.transform.rotation = Quaternion.Euler(Mmgr.transform.rotation.eulerAngles.x, 0, Mmgr.transform.rotation.eulerAngles.z);
        }
        else
        {
            isRight = -1;
            Mmgr.transform.rotation = Quaternion.Euler(Mmgr.transform.rotation.eulerAngles.x, 180, Mmgr.transform.rotation.eulerAngles.z);

        }

    }

}
