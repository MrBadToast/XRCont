using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegFSMMgr : EnemyBaseFSMMgr
{
    public GameObject firePos;
    public GameObject rollCollider;
    public GameObject idleCollider;
    public GameObject standCollider;

    public bool isground;

    private new void Awake()
    {
        base.Awake();
       
    }
    public override void Die()
    {
        animator.SetTrigger("death");
        Invoke("DestroyObj", 2f);
    }

    private void DestroyObj()
    {
        Destroy(this.gameObject);
    }

    public void fire(GameObject bullet)
    {
        Instantiate(bullet, firePos.transform.position, Quaternion.identity);
    }

    public void spawn()
    {
        currentState = new MegIdleState().Instance();
        currentState.Begin(this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isground = true;
    }
}
