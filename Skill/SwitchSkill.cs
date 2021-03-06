using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SkillCoolDown skillCoolDown;
    private Vector3 beginPosition;

    void Start()
    {
        skillCoolDown = GetComponent<SkillCoolDown>();
        beginPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        transform.position = beginPosition;
        transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (go != null && go.tag == "Skill")
        {
            SkillCoolDown goSkillCoolDown = go.GetComponent<SkillCoolDown>();
            if (skillCoolDown.coolDownComplete && goSkillCoolDown.coolDownComplete)
            {
                Skill tSkill = skillCoolDown.skill;
                skillCoolDown.skill = goSkillCoolDown.skill;
                skillCoolDown.Initialize();
                goSkillCoolDown.skill = tSkill;
                goSkillCoolDown.Initialize();
            }
        }
    }
}
