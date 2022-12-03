using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp001 : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    public GameObject title;
    public GameObject title2;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void end()
    {
        title.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetTrigger("end");
            
            title2.SetActive(false);
        }
    }

}
