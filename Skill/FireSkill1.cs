using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skill/FireSkill1")]
public class FireSkill1 : Skill
{
    public float damage = 1f;

    private GameObject fire1;
    private Fire1Trigger fire1Trigger;
    public GameObject prefabFire1;
    private Transform player;
    private float skillDir;

    public override void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void TriggerSkill()
    {
        fire1 = Instantiate(prefabFire1, player.transform.position, Quaternion.identity);
        fire1Trigger = fire1.GetComponent<Fire1Trigger>();
        fire1Trigger.damage = damage;
        fire1Trigger.skillDir = Mathf.Sign(player.localScale.x);;
    }
}
