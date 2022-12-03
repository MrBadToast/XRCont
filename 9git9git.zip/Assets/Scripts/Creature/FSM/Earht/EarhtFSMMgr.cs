using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarhtFSMMgr : EnemyBaseFSMMgr
{
    public GameObject Senot;

    public GameObject[] pos;


    bool isspawn;
    private new void Awake()
    {
        base.Awake();
        isspawn = false;
    }

    public override void Die()
    {
        animator.SetTrigger("dead");
        if (!isspawn)
        {
            Invoke("DestroyObj", 2f);
            isspawn = true;
        }
       
    }

    void DestroyObj()
    {
        Instantiate(Senot, pos[0].transform.position, Quaternion.identity);
        Instantiate(Senot, pos[1].transform.position, Quaternion.identity);
        Instantiate(Senot, pos[2].transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
    public void spawn()
    {
        currentState = null;
        animator.SetTrigger("idle");
    }
}
