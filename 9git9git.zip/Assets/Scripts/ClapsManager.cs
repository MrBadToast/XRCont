using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClapsManager : MonoBehaviour
{
    static private ClapsManager instance;
    static public ClapsManager Instance { get { return instance; } }

    [SerializeField] private GameObject ClapObject;

    private List<ClapBehavior> claps;

    private int Render_queue = 2000;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        claps = new List<ClapBehavior>();
    }

    public void AddNewClap(Vector2 position)
    {
        RenderFeaturePositionControl.Instance.SpreadDistortion(position, 1.0f, 1.0f);
        GameObject newClap = Instantiate(ClapObject, position, Quaternion.identity);
        ClapBehavior newClapComp = newClap.GetComponent<ClapBehavior>();
        claps.Add(newClapComp);
        newClapComp.SetRenderQueue(Render_queue,Render_queue+1);
        Render_queue+=2;
    }

    public void RemoveClap(ClapBehavior clap)
    {
        claps.Remove(clap);
        if (claps.Count == 0) Render_queue = 2000;
    }

    public bool IsClapZone(Vector2 point)
    {
        bool validPoint = false; bool validPoint2 = false;

        for(int i = 0; i < claps.Count; i++)
        {
            if (Vector2.Distance(point, claps[i].transform.position) < claps[i].BcRadius)
            {
                validPoint = true;
            }

            if (Vector2.Distance(point, claps[i].transform.position) < claps[i].RcRadius)
            {
                validPoint = false;
            }
        }

            return validPoint;
    }
}
