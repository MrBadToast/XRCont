using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    static private GamePlayUI instance;
    static public GamePlayUI Instance { get { return instance; } }

    [SerializeField] private float IntSpriteUISpace = 150f;
    [SerializeField] private Transform IntSpriteUICenter;

    [SerializeField] private GameObject UI_Health;
    [SerializeField] private GameObject UI_Skill;
    [SerializeField] private Image UI_RadialCharger;
    [SerializeField] private GameObject UI_RadialHalo;

    private List<IntegerSpritesUI> Healths;
    private List<IntegerSpritesUI> Skills;

    private Stack<IntegerSpritesUI> ActiveHealths;
    private Stack<IntegerSpritesUI> ActiveSkills;

    private int CurrentHealth {  get { return ActiveHealths.Count; } }
    private int CurrentSkill { get { return ActiveSkills.Count; } }

    private int MaxHealth { get { return Healths.Count; } }
    private int MaxSkill { get { return Skills.Count; } }

    private void Awake()
    {
        Healths = new List<IntegerSpritesUI>();
        Skills = new List<IntegerSpritesUI>();

        ActiveHealths = new Stack<IntegerSpritesUI>();
        ActiveSkills = new Stack<IntegerSpritesUI>();

        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeHealthSkill(int healthMax, int skillMax, int healthCurrent, int skillCurrent)
    {
        IntegerSpritesUI he, sk;
        he = UI_Health.GetComponent<IntegerSpritesUI>(); sk = UI_Skill.GetComponent<IntegerSpritesUI>();
        Healths.Add(he);
        Skills.Add(sk);
        ActiveHealths.Push(he);
        ActiveSkills.Push(sk);

        for(int i = 1; i < healthMax; i++)
        {
            GameObject newGO = Instantiate(UI_Health, IntSpriteUICenter);
            newGO.GetComponent<RectTransform>().localPosition = new Vector2(IntSpriteUISpace * i + UI_Health.transform.localPosition.x, 0f);
            IntegerSpritesUI newComp = newGO.GetComponent<IntegerSpritesUI>();
            Healths.Add(newComp);
            ActiveHealths.Push(newComp);
        }
        for(int i = 1; i < skillMax; i++)
        {
            GameObject newGO = Instantiate(UI_Skill, IntSpriteUICenter);
            newGO.GetComponent<RectTransform>().localPosition = new Vector2(IntSpriteUISpace * -i + UI_Skill.transform.localPosition.x, 0f);
            IntegerSpritesUI newComp = newGO.GetComponent<IntegerSpritesUI>();
            Skills.Add(newComp);
            ActiveSkills.Push(newComp);
        }

        for(int i = healthMax; i > healthCurrent; i--)
        {
            ActiveHealths.Peek().SetUI(false);
            ActiveHealths.Pop();
        }
        for (int i = skillMax; i > skillCurrent; i--)
        {
            ActiveSkills.Peek().SetUI(false);
            ActiveSkills.Pop();
        }

    }

    public void SetHealth(int var)
    {
        if (var < 0) return;

        if(var < CurrentHealth)
        {
            for(int i=0; i < CurrentHealth- var; i++)
            {
                ActiveHealths.Peek().SetUI(false);
                ActiveHealths.Pop();
                if (CurrentHealth == 0) return; 
            }
        }
        else if(var > CurrentHealth)
        {
            for(int i = 0; i < var -CurrentHealth; i++)
            {
                Healths[CurrentHealth].SetUI(true);
                ActiveHealths.Push(Healths[CurrentHealth]);
                if (CurrentHealth == MaxHealth) return;
            }
        }
    }
    public void SetSkill(int var)
    {
        if (var < 0) return;

        if (var < CurrentSkill)
        {
            for (int i = 0; i < CurrentSkill- var; i++)
            {
                ActiveSkills.Peek().SetUI(false);
                ActiveSkills.Pop();
                if (CurrentSkill == 0) return;
            }
        }
        else if (var > CurrentSkill)
        {
            for (int i = 0; i < var -CurrentSkill; i++)
            {
                Skills[CurrentSkill].SetUI(true);
                ActiveSkills.Push(Skills[CurrentSkill]);
                if (CurrentSkill == MaxSkill) return;
            }
        }
    }

    public void SetSkillChargerRadial(float var)
    {
        float prog = Mathf.Clamp01(var);

        UI_RadialCharger.fillAmount = prog;

    }

    public void FlashChargerHalo()
    {
        UI_RadialHalo.SetActive(true);
    }

}
