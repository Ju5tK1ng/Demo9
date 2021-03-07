using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image skillIcon;
    private SkillDisplay skillDisplay;
    private Vector3 beginPosition;

    void Start()
    {
        skillDisplay = GetComponent<SkillDisplay>();
        beginPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        skillIcon.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillDisplay.skill.skillLevel > 0 && skillDisplay.skill.isAdded == 0)
        {
            skillIcon.transform.position = Input.mousePosition;
        }
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        skillIcon.transform.position = beginPosition;
        skillIcon.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (go != null && go.tag == "Skill" && skillDisplay.skill.skillLevel > 0 && skillDisplay.skill.isAdded == 0)
        {
            SkillCoolDown goSkillCoolDown = go.GetComponent<SkillCoolDown>();
            if (goSkillCoolDown.coolDownComplete)
            {
                if (goSkillCoolDown.skill.isAdded == 1)
                {
                    goSkillCoolDown.skill.isAdded = 0;
                }
                goSkillCoolDown.skill = skillDisplay.skill;
                goSkillCoolDown.skill.isAdded = 1;
                goSkillCoolDown.Initialize();
            }
        }
    }
}
