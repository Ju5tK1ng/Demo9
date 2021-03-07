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
                ButtonTriggered ();
            }
        } else 
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

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownTime + Time.time;
        coolDownTimer = coolDownTime;
        darkMask.enabled = true;
        coolDownText.enabled = true;
        // SkillSource.clip = Skill.aSound;
        // SkillSource.Play ();
        skill.TriggerSkill ();
    }
}