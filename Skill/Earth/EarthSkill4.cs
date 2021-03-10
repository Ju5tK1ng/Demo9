using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (menuName = "Skill/EarthSkill4")]
public class EarthSkill4 : Skill
{
    private GameObject createTilemap;
    private GameObject playerGameObject;
    private Transform playerTransform;
    private PlayerControl3 player;
    private float skillDir;

    public override void Initialize()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;
        player = playerGameObject.GetComponent<PlayerControl3>();
        createTilemap = GameObject.FindGameObjectWithTag("CreateTilemap");
        skillLevel = 0;
        isAdded = 0;
    }

    public override void TriggerSkill()
    {
        Vector3 tPosition = playerTransform.position;
		tPosition.y -= 0.3f;
		if (player.h == 0 && player.v == 0)
		{
			tPosition.x += Mathf.Sign(playerTransform.localScale.x);
		}
		else if (player.h == 0 && player.v == 1)
		{
			tPosition.y += 2;
		}
		else
		{
			tPosition.x += player.h;
			tPosition.y += player.v;
		}
		tPosition.x = Mathf.Round(tPosition.x + 0.5f) - 0.5f;
		tPosition.y = Mathf.Round(tPosition.y + 0.5f) - 0.5f;
        createTilemap.GetComponent<CreateTile>().CreateOneTile(tPosition);
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
