using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillDisplay : MonoBehaviour
{
    public Skill skill;
    public Image darkMask;
    public Text skillLevelText;
    private Text skillDescription;
    private Image skillIcon;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        skill.Initialize();
        skillLevelText.text = "Lv:" + skill.skillLevel.ToString();
    }

    public void GetSkill()
    {
        if (skill.CheckSkill())
        {
            skill.GetSkill();
            skillLevelText.text = "Lv:" + skill.skillLevel.ToString();
            darkMask.gameObject.SetActive(false);
        }
    }
}
