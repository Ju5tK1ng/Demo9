using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string sName;
    public Sprite sSprite;
    // public AudioClip sSound;
    public float sCoolDown;
    public int levelNeeded;
    public int skillLevel;
    public int maxSkillLevel;
    public int isAdded;
    public int skillType;
    public int skillTime;   // 2型表示持续时间
    public int skillStatus;
    public abstract void Initialize();
    public abstract void TriggerSkill();
    public abstract bool CheckSkill();
    public abstract void GetSkill();
}
