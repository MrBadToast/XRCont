using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerSpritesUI : MonoBehaviour
{
    [SerializeField] private bool initialState;
    [SerializeField] private GameObject UI_Fill;
    [SerializeField] private GameObject UI_Empty;

    private void Start()
    {
        if(initialState)
        {
            UI_Fill.SetActive(true);
            UI_Empty.SetActive(false);
        }
        else
        {
            UI_Fill.SetActive(false);
            UI_Empty.SetActive(true);
        }
    }

    public void SetUI(bool var)
    {
        if(var)
        {
            UI_Fill.SetActive(true);
            UI_Empty.SetActive(false);
        }
        else
        {
            UI_Fill.SetActive(false);
            UI_Empty.SetActive(true);
        }
    }

    public bool IsUIActive()
    {
        return UI_Fill.activeInHierarchy;
    }
}
