using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_NPCBehavior : MonoBehaviour
{
    [SerializeField] protected int maxHealth;

    public virtual void Idle() { }
    public virtual void Chase() { }
    public virtual void Damaged() { }
    public virtual void Death() { }
    public virtual bool DetectTarget(string objectTag) { return false; }    
}
