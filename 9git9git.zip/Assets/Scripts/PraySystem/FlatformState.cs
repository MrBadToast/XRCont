using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatformState : MonoBehaviour
{
    public enum flatformState
    {
        original, selected, copied, fired
    }

    public flatformState FState;

    private SelectableVisual visual;

    private void Start()
    {
        FState = flatformState.original;
        visual = GetComponent<SelectableVisual>();
    }


    private void Update()
    {
        if (FState == flatformState.copied)
        {
            StartCoroutine(SetDestroyTimer(10));
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (FState == flatformState.fired)
        {


            if (collision.gameObject.layer == 15)
            {
                collision.gameObject.GetComponent<EnemyBaseFSMMgr>().Damaged(1);

            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (collision.gameObject.GetComponent<FlatformState>())
                {
                    Destroy(this.gameObject, 5f);
                    return;
                }

            }
            Destroy(this.gameObject);

        }
        else if (FState == flatformState.copied)
        {
            if (collision.gameObject.layer == 8)
            {
                Destroy(this.gameObject);
            }

        }

    }

    IEnumerator SetDestroyTimer(float time)
    {
        yield return new WaitForSeconds(time / 2f);
        visual.SetVanishAlertVisual(true, 1.0f);
        yield return new WaitForSeconds(time / 4f);
        visual.SetVanishAlertVisual(true, 2.0f);
        yield return new WaitForSeconds(time / 8f);
        visual.SetVanishAlertVisual(true, 3.0f);
        yield return new WaitForSeconds(time / 8f);


        Destroy(gameObject);
    }

}
