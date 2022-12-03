using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaterialManager : MonoBehaviour
{
    static private SoundMaterialManager instance;
    static public SoundMaterialManager Instance { get { return instance; } }   

    [SerializeField]private string[] soundMaterialIndex;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public int FindMatIndex(string name)
    {
        for(int i = 0; i < soundMaterialIndex.Length; i++)
        {
            if(soundMaterialIndex[i] == name) return i;
        }

        return 0;
    }
}
