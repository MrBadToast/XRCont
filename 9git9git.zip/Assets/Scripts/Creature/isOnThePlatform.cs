using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isOnThePlatform : MonoBehaviour
{
    public GameObject left;
    public GameObject right;

    private RaycastHit2D hit;
    // Update is called once per frame
    void Update()
    {
        if (!Physics2D.Raycast(left.transform.position, Vector2.down, 1))
        {
            Debug.Log("충돌 없음");
        }
        else
        {
            Debug.Log("충돌 잇음");
        }
    }
}
