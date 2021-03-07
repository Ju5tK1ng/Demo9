using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skill/NoneSkill")]
public class NoneSkill : Skill
{
    public override void Initialize()
    {
    }
    
    public override void TriggerSkill()
    {
    }

    public override bool CheckSkill()
    {
        return false;
    }

    public override void GetSkill()
    {
    }
}
