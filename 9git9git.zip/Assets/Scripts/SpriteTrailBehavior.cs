using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTrailBehavior : MonoBehaviour
{
    private SpriteRenderer spriteRen;
    private float fadeTime =1.0f;

    public void OnSpawned(Sprite spr,Transform trs)
    {
        spriteRen.sprite = spr;
        transform.position = trs.position;
        transform.rotation = trs.rotation;
        transform.localScale = trs.localScale;
        StartCoroutine(Cor_Fadeout());
    }

    private void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();
    }

    IEnumerator Cor_Fadeout()
    {
        for(float t = fadeTime; t > 0; t -= Time.fixedDeltaTime)
        {
            Color c = spriteRen.color;
            spriteRen.color = new Color(c.r, c.g, c.b, t / fadeTime);
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
