using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public abstract class EnemyBaseFSMMgr : MonoBehaviour
{
    //state °ü¸®¿ë
    protected EnemyBaseState currentState;
    protected EnemyBaseState prevState;
    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }
    public EnemyBaseState PrevState
    {
        get { return prevState; }
    }

    protected EnemyStatus status;
    public EnemyStatus Status
    {
        get { return status; }
    }

    public bool isStun;

    [HideInInspector] public Animator animator;

    [HideInInspector] public Rigidbody2D rig;

    protected void Awake()
    {
        status = GetComponent<EnemyStatus>();
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentState = null;
        isStun = false;
    }
    private void Update()
    {
        if (currentState != null &&  !isStun)
        {
            currentState.Update(this);
        }
        
    }
    public void ChangeState(EnemyBaseState state)
    {
        if (currentState != state)
        {
            currentState.End(this);
            prevState = currentState;
            currentState = state;
            currentState.Begin(this);
        }
    }

    public virtual void Damaged(float demage)
    {
        Status.Hp -= demage;
        if (Status.Hp <= 0)
        {
            Die();
        }
    }
    public abstract void Die();


    public float CalcTargetDistance()
    {
        return Vector3.Distance(enemyManager.Instance.playerPos.position, transform.position);
    }
    public bool CheckInAttackRange()
    {
        return ((CalcTargetDistance() < status.AttackRange) ? true : false);
    }
    public bool CheckInView(float add = 0)
    {
        return ((CalcTargetDistance() < status.ViewDistance + add) ? true : false);
    }
    public bool IsAlive()
    {
        return (status.Hp > 0) ? true : false;
    }
}
