using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/SpaceSkill2")]
public class SpaceSkill2 : Skill
{
    public float damage = 1f;
    private GameObject space2Gameobject;
    private Space2 space2;
    public GameObject prefabSpace2;
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
        player.dashTimer = 0.3f;
		if(player.h != 0)
		{
			player.myVelocity = Vector2.right * 24f * player.h;
		}
		else
		{
			player.myVelocity = Vector2.right * 24f * Mathf.Sign(playerTransform.localScale.x);
		}
		player.playState = PlayerControl3.PlayState.Dash;
        space2Gameobject = Instantiate(prefabSpace2, playerTransform.position + Vector3.right * Mathf.Sign(playerTransform.localScale.x) * 4f, Quaternion.identity);
        space2 = space2Gameobject.GetComponent<Space2>();
        space2.playerTransform = playerTransform;
        space2.damage = damage;
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
