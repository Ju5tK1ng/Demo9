using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string sName;
    public Sprite sSprite;
    // public AudioClip sSound;
    public float sCoolDown;
    public abstract void Initialize();
    public abstract void TriggerSkill();
}
