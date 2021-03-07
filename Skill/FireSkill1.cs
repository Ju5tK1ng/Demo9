using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/FireSkill1")]
public class FireSkill1 : Skill
{
    public float damage = 1f;

    private GameObject fire1;
    private Fire1Trigger fire1Trigger;
    public GameObject prefabFire1;
    private Transform playerTransform;
    private PlayerControl3 player;
    private float skillDir;

    public override void Initialize()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.gameObject.GetComponent<PlayerControl3>();
        skillLevel = 0;
        isAdded = 0;
    }

    public override void TriggerSkill()
    {
        fire1 = Instantiate(prefabFire1, playerTransform.position, Quaternion.identity);
        fire1Trigger = fire1.GetComponent<Fire1Trigger>();
        fire1Trigger.damage = damage;
        fire1Trigger.skillDir = Mathf.Sign(playerTransform.localScale.x);;
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
