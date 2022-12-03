using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegTraceState : EnemyBaseState
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
        mgr.animator.SetTrigger("move");
        mgr.rig.constraints = RigidbodyConstraints2D.None;
        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.rollCollider.SetActive(true);
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {

        if (mgr.transform.position.x > enemyManager.Instance.playerPos.position.x)
        {
            //if (!Physics2D.Raycast(mgr.transform.position + new Vector3(3.0f, -0.9f, 0f), new Vector3(0, -1, 0), 2))
            //{
            //    mgr.ChangeState(new MegFlyState().Instance());
            //}
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 0, mgr.transform.rotation.eulerAngles.z);
            mgr.rig.velocity = new Vector2((mgr.Status.Speed * 100.0f * Time.deltaTime), mgr.rig.velocity.y);
        }
        else
        {
            //if (!Physics2D.Raycast(mgr.transform.position + new Vector3(-3.0f, -0.9f, 0f), new Vector3(0, -1, 0), 2))
            //{
            //    mgr.ChangeState(new MegFlyState().Instance());
            //}
            mgr.transform.rotation = Quaternion.Euler(mgr.transform.rotation.eulerAngles.x, 180, mgr.transform.rotation.eulerAngles.z);
            mgr.rig.velocity = new Vector2(-(mgr.Status.Speed * 100.0f * Time.deltaTime), mgr.rig.velocity.y);
        }

        if (!mgr.CheckInAttackRange())
        {
            mgr.ChangeState(new MegIdleState().Instance());

        }

    }

    public override void End(EnemyBaseFSMMgr mgr)
    {
        mgr.rig.velocity = Vector2.zero;
        mgr.transform.rotation = Quaternion.RotateTowards(
            mgr.transform.rotation, Quaternion.Euler(0, 0, 0), 150 * Time.deltaTime);

        mgr.rig.constraints = RigidbodyConstraints2D.FreezePositionX
              | RigidbodyConstraints2D.FreezeRotation;

        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.rollCollider.SetActive(false);
    }
}
