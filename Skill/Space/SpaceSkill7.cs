using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/SpaceSkill7")]
public class SpaceSkill7 : Skill
{
    private GameObject playerGameObject;
    private Transform playerTransform;
    private PlayerControl3 player;
    

    public override void Initialize()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;
        player = playerGameObject.GetComponent<PlayerControl3>();
        skillLevel = 0;
        isAdded = 0;
    }

    public override void TriggerSkill()
    {
        player.spaceSkill7Timer = skillTime;
    }
    
    // 检测能否学习技能
    public override bool CheckSkill()
    {
        return player.MyLevel >= levelNeeded && player.MySP >= 1 && skillLevel < maxSkillLevel;
    }

    // 学习新技能
    public override void GetSkill()
    {
        player.MySP -= 1;
        skillLevel += 1;
    }
}
