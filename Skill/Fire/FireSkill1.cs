using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/FireSkill1")]
public class FireSkill1 : Skill
{
    public float damage = 1f;

    private GameObject fire1Gameobject;
    private Fire1 fire1;
    public GameObject prefabFire1;
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
        fire1Gameobject = Instantiate(prefabFire1, playerTransform.position, Quaternion.identity);
        fire1 = fire1Gameobject.GetComponent<Fire1>();
        fire1.damage = damage;
        fire1.skillDir = Mathf.Sign(playerTransform.localScale.x);
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
