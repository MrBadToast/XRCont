using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (select.Instance.MState == select.MouseState.lasso||
            select.Instance.MState == select.MouseState.shot)
        {
            DrawLine();
        }
        else
        {
            EndLine();
        }
    }

    private void DrawLine()
    {
        List<Vector3> temp = new List<Vector3>();
        foreach (var item in select.Instance.points)
        {
            temp.Add((Vector3)item);
        }
        if (temp.Count > 0 && select.Instance.MState == select.MouseState.lasso)
        {
            temp.Add(select.Instance.points[0]);
        }
       
        lr.positionCount = temp.Count;
        lr.SetPositions(temp.ToArray());
    }

    private void EndLine()
    {
        lr.positionCount = 0;
    }
}
    