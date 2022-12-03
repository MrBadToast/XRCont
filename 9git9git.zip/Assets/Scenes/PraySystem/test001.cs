using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test001 : MonoBehaviour
{
    private Rigidbody2D rig;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //rig.AddForce(Vector2.left*0.1f, ForceMode2D.Force);

        if (time < 3f)
        {
            rig.velocity = new Vector2(-3.0f, 0.0f);
        }
        else
        {
            rig.velocity = Vector2.zero;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 150 * Time.deltaTime);
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
            //transform.rotation = Quaternion.Euler(0,0,0);
        }
    }



}