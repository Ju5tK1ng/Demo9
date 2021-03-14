using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour {

    public KeyCode skillButton;
    public Image darkMask;
    public Text coolDownText;
    public Skill skill;
    // [SerializeField] private GameObject player;
    private Image skillIcon;
    // private AudioSource SkillSource;
    private float coolDownTime;
    private float coolDownTimer;
    private float nextReadyTime;
    private float skillTimer;   // 2型用作持续计时，3型用作按下计时
    public bool coolDownComplete;


    void Start () 
    {
        Initialize ();    
    }

    public void Initialize()
    {
        skillIcon = GetComponent<Image>();
        // SkillSource = GetComponent<AudioSource> ();
        skillIcon.sprite = skill.sSprite;
        coolDownTime = skill.sCoolDown;
        skillTimer = 0;
        SkillReady ();
    }

    void Update () 
    {
        coolDownComplete = (Time.time > nextReadyTime);
        if (coolDownComplete) 
        {
            SkillReady ();
            if (Input.GetKeyDown(skillButton)) 
            {
                switch (skill.skillType)
                {
                // 触发型
                case 1:
                    CDTriggered ();
                    skill.TriggerSkill();
                    break;
                // 切换型
                case 2:
                    if (skillTimer == 0)
                    {
                        skillTimer = skill.skillTime;
                        skill.TriggerSkill();
                    }
                    else if (skillTimer > 0)
                    {
                        skillTimer = 0;
                        CDTriggered();
                        skill.TriggerSkill();
                    }
                    break;
                case 4:
                    skill.TriggerSkill();
                    break;
                default:
                    break;
                }
            }
            else if (Input.GetKeyUp(skillButton))
            {
                switch (skill.skillType)
                {
                    // SpaceSkill3型
                    case 3:
                        skillTimer = 0;
                        if (skill.skillStatus == 1)
                        {
                            CDTriggered();
                        }
                        skill.TriggerSkill();
                        break;
                    // 瞄准型
                    case 4:
                        skill.skillStatus = 2;
                        CDTriggered();
                        skill.TriggerSkill();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetKey(skillButton))
            {
                switch (skill.skillType)
                {
                    // SpaceSkill3型
                    case 3:
                        if (skill.skillStatus == 1)
                        {
                            skillTimer += Time.deltaTime;
                            if (skillTimer > 1f)
                            {
                                skill.skillStatus = 2;
                                skillTimer = 0;
                                skill.TriggerSkill();
                            }
                        }
                        break;
                    case 4:
                        skill.TriggerSkill();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (skill.skillType)
                {
                    case 2:
                        if (skillTimer > 0)
                        {
                            skillTimer -= Time.deltaTime;
                        }
                        else if (skillTimer < 0)
                        {
                            skillTimer = 0;
                            CDTriggered ();
                            skill.TriggerSkill();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else 
        {
            CoolDown();
        }
    }

    private void SkillReady()
    {
        coolDownText.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer > 10)
        {
            coolDownText.text = coolDownTimer.ToString("f0");
        }
        else
        {
            coolDownText.text = coolDownTimer.ToString("f1");
        }
        darkMask.fillAmount = (coolDownTimer / coolDownTime);
    }

    private void CDTriggered()
    {
        nextReadyTime = coolDownTime + Time.time;
        coolDownTimer = coolDownTime;
        darkMask.enabled = true;
        coolDownText.enabled = true;
        // SkillSource.clip = Skill.aSound;
        // SkillSource.Play ();
    }
}