using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/Dash")]
public class Dash : Skill
{
    public override void Initialize()
    {
    }

    public override void TriggerSkill()
    {
    }
    
    // 检测能否学习技能
    public override bool CheckSkill()
    {
        return false;
    }

    // 学习新技能
    public override void GetSkill()
    {
    }
}
