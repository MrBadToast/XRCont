using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegFlyState : EnemyBaseState
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

    Vector3 Target;
    float firingAngle = 70.0f;
    float gravity = 9.8f;
    float flightDuration;
    float elapse_time;
    float Vx;
    float Vy;
    public override void Begin(EnemyBaseFSMMgr mgr)
    {
        mgr.rig.velocity = Vector2.zero;
        mgr.transform.rotation = Quaternion.Euler(
            mgr.transform.rotation.eulerAngles.x,
            mgr.transform.rotation.eulerAngles.y,
            0.0f);
        mgr.animator.SetTrigger("fly");

        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.idleCollider.SetActive(true);

        mgr.rig.gravityScale = 0;

        Mmgr.isground = false;

        mgr.rig.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; ;

        //////////////////////////////////////////////////

        Target = mgr.transform.position + new Vector3(Random.Range(-20, 20), 0, 0);
        //타겟과 거리를 계산
        float target_Distance = Vector3.Distance(mgr.transform.position, Target);

        // 각도와 중력을 대입하여 포물선 속도르 구함
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        //x,y 값을 구함
        Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // 날아가는 시간 
        flightDuration = target_Distance / Vx;

        // 날아가는 방향으로 바라봄
        mgr.transform.rotation = Quaternion.LookRotation(Target - mgr.transform.position);
        mgr.transform.Rotate(new Vector3(0, 90, 0));
        elapse_time = 0;
        ///////////////////////////////////////////////
    }
    public override void Update(EnemyBaseFSMMgr mgr)
    {
        MegFSMMgr Mmgr = mgr as MegFSMMgr;

        //mgr.rig.AddForce(new Vector2(
        //    Vx * 100 * Time.deltaTime, 
        //    (Vy - (gravity * elapse_time)) * 100 * Time.deltaTime));
        mgr.transform.Translate(Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0.0f);
        elapse_time += Time.deltaTime;

        if (Mmgr.isground|| flightDuration < elapse_time)
        {
            mgr.ChangeState(new MegIdleState().Instance());
        }

    }
    public override void End(EnemyBaseFSMMgr mgr)
    {
        MegFSMMgr Mmgr = mgr as MegFSMMgr;
        Mmgr.idleCollider.SetActive(false);
        mgr.rig.gravityScale = 1;
        mgr.rig.constraints = RigidbodyConstraints2D.FreezePositionX
             | RigidbodyConstraints2D.FreezeRotation;
    }
}
