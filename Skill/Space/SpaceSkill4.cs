using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Skill/SpaceSkill4")]
public class SpaceSkill4 : Skill
{
    private GameObject playerGameObject;
    private Transform playerTransform;
    private PlayerControl3 player;
	public Transform prefabSpace4;
	private Transform space4Transform1;
    private Transform space4Transform2;
    private Space4 space4_1;
    private Space4 space4_2;
    private float speed;
    

    public override void Initialize()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;
        player = playerGameObject.GetComponent<PlayerControl3>();
        skillLevel = 0;
        isAdded = 0;
        skillStatus = 0;  // 0表示释放第一个，1表示瞄准，2表示释放第二个
        speed = 0.4f;
    }

    public override void TriggerSkill()
    {
        if (skillStatus == 0)
		{
			skillStatus = 1;
			space4Transform1 = Instantiate(prefabSpace4, playerTransform.position + Vector3.right * Mathf.Sign(playerTransform.localScale.x) * 8f, Quaternion.identity);
            space4_1 = space4Transform1.GetComponent<Space4>();
            player.aiming = true;
            player.myVelocity = Vector3.zero;
		}
		else if (skillStatus == 1)
		{
            space4Transform1.position += new Vector3(player.h, player.v, 0).normalized * speed;
		}
        else
        {
            space4Transform2 = Instantiate(prefabSpace4, playerTransform.position, Quaternion.identity);
            space4_2 = space4Transform2.GetComponent<Space4>();
            space4_1.anotherSpace4 = space4Transform2;
            space4_2.anotherSpace4 = space4Transform1;
            space4_1.RemoveLater(skillTime);
            space4_2.RemoveLater(skillTime);
            player.aiming = false;
            skillStatus = 0;
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
