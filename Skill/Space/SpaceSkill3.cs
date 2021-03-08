using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/SpaceSkill3")]
public class SpaceSkill3 : Skill
{
    private GameObject playerGameObject;
    private Transform playerTransform;
    private PlayerControl3 player;
	public Transform prefabShadow;
	private Transform playerShadow;
    

    public override void Initialize()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;
        player = playerGameObject.GetComponent<PlayerControl3>();
        skillLevel = 0;
        isAdded = 0;
        skillTime = 0;  // 0表示未创建阴影，1表示已创建阴影
    }

    public override void TriggerSkill()
    {
        if (skillTime == 0)
		{
			skillTime = 1;
			playerShadow = Instantiate(prefabShadow, playerTransform.position, Quaternion.identity);
		}
		else if (skillTime == 1)
		{
			Vector3 tPosition = playerTransform.position;
			playerTransform.position = playerShadow.position;
			playerShadow.position = tPosition;
			Vector3 tLocalScale = playerTransform.localScale;
			playerTransform.localScale = playerShadow.localScale;
            if (player.earthSkill3 == -1)
            {
                Vector3 scaleFlipY = playerTransform.localScale;
		        scaleFlipY.y = -scaleFlipY.y;
                playerTransform.localScale = scaleFlipY;
                tLocalScale.y = -tLocalScale.y;
            }
			playerShadow.localScale = tLocalScale;
		}
        else
        {
            Destroy(playerShadow.gameObject);
            skillTime = 0;
        }
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
