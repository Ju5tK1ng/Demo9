using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skill/WoodSkill4")]
public class WoodSkill4 : Skill
{
    private GameObject playerGameObject;
    private Transform playerTransform;
    private PlayerControl3 player;
    private RaycastHit2D wood4Hit;
    public GameObject prefabWood4;
    private GameObject wood4Gameobject;
    private Wood4 wood4;
    private int groundLayerMask;
    private float speed = 36f;

    public override void Initialize()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGameObject.transform;
        player = playerGameObject.GetComponent<PlayerControl3>();
        groundLayerMask = LayerMask.GetMask("Ground");
        skillLevel = 0;
        isAdded = 0;
    }

    public override void TriggerSkill()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        if (player.h == 0 && player.v == 0)
		{
			direction.x += Mathf.Sign(playerTransform.localScale.x);
		}
		else
		{
			direction.x += player.h;
			direction.y += player.v;
		}
        player.woodSkill4 = true;
        player.playState = PlayerControl3.PlayState.Jump;
        player.myVelocity = Vector3.right * player.h / 1000f;
        wood4Hit = Physics2D.Raycast(playerTransform.position, direction, 36f, groundLayerMask);
        wood4Gameobject = Instantiate(prefabWood4, playerTransform.position, Quaternion.identity);
        wood4 = wood4Gameobject.GetComponent<Wood4>();
        wood4.playerTransform = playerTransform;
        wood4.player = player; 
        if (wood4Hit.collider != null)
        {
            wood4.endPosition = wood4Hit.point;
            wood4.pullTime = wood4Hit.distance / speed;
            wood4.pushTime = wood4.pullTime / 2;
            player.airJumps = 1;
            player.dashes = 1;
        }
        else
        {
            wood4.endPosition = playerTransform.position + direction.normalized * 36f;
            wood4.pushTime = 0.5f;
            wood4.pullTime = 0f;
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
