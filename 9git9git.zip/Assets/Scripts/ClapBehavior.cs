using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapBehavior : MonoBehaviour
{
    [SerializeField] private float startRadius;
    [SerializeField] private float redClapDelay;

    [SerializeField] private Transform RC_Visual;
    [SerializeField] private Transform BC_Visual;

    [SerializeField] private float BC_SpreadTime = 5.0f;
    [SerializeField] private float RC_SpreadTime = 10.0f;
    [SerializeField] private float Max_Radius = 10.0f;
    [SerializeField] private AnimationCurve BC_SpreadCurve;
    [SerializeField] private AnimationCurve RC_SpreadCurve;

    private float RC_radius;
    public float RcRadius { get { return RC_radius/2f; } }
    private float BC_radius;
    public float BcRadius { get { return BC_radius/2f; } }

    private float RC_stanbyTime;

    private void OnDestroy()
    {
        ClapsManager.Instance.RemoveClap(this);
    }

    private void Start()
    {
        InitiateClap();
    }


    private void Update()
    {
        RC_stanbyTime -= Time.deltaTime;
    }

    public void SetRenderQueue(int BC_value,int RC_value)
    {
        BC_Visual.gameObject.GetComponent<Renderer>().material.renderQueue = BC_value;
        RC_Visual.gameObject.GetComponent<Renderer>().material.renderQueue = RC_value;
    }

    public void InitiateClap()
    {
        StartCoroutine("Cor_RCSpread");
        StartCoroutine("Cor_BCSpread");
    }

    IEnumerator Cor_RCSpread()
    {
        yield return new WaitForSeconds(redClapDelay);

        for (float i = 0; i < RC_SpreadTime; i += Time.fixedDeltaTime)
        {
            float cur = RC_SpreadCurve.Evaluate(i / RC_SpreadTime)*Max_Radius;

            yield return new WaitForFixedUpdate();

            RC_radius = (i / RC_SpreadTime) * Max_Radius + cur;
            RC_Visual.localScale = Vector3.one * RC_radius;
        }

        yield return Cor_KillClap();
    }

    IEnumerator Cor_BCSpread()
    {
        for (float j = 0; j < BC_SpreadTime; j += Time.fixedDeltaTime)
        {
            float cur = BC_SpreadCurve.Evaluate(j / BC_SpreadTime) * Max_Radius;

            yield return new WaitForFixedUpdate();

            BC_radius = (j / BC_SpreadTime) * Max_Radius + cur;
            BC_Visual.localScale = Vector3.one * BC_radius;
        }
    }

    IEnumerator Cor_KillClap()
    {
        StopCoroutine("Cor_RCSpread");
        StopCoroutine("Cor_BCSpread");
        ClapsManager.Instance.RemoveClap(this);       
        yield return null;
        Destroy(gameObject);
    }

    int circSides = 72;

    private void SetCircle(Vector2 origin,float radius,ref LineRenderer line)
    {
        Vector3[] points = new Vector3[circSides + 1];
        for(int i = 0; i < circSides; i++)
        {
            points[i] = new Vector2(origin.x + Mathf.Cos(2*Mathf.PI/circSides*i)*radius, origin.y + Mathf.Sin(2 * Mathf.PI / circSides * i)*radius);
        }

        points[circSides] = new Vector2(origin.x + radius, origin.y);
        line.positionCount = circSides + 1;
        line.SetPositions(points);
        
    }
}
