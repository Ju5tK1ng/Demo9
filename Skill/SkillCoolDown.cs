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
    private Image ButtonImage;
    // private AudioSource SkillSource;
    private float coolDownTime;
    private float nextReadyTime;
    private float coolDownTimer;


    void Start () 
    {
        Initialize ();    
    }

    public void Initialize()
    {
        ButtonImage = GetComponent<Image>();
        // SkillSource = GetComponent<AudioSource> ();
        ButtonImage.sprite = skill.sSprite;
        coolDownTime = skill.sCoolDown;
        skill.Initialize ();
        SkillReady ();
    }

    void Update () 
    {
        bool coolDownComplete = (Time.time > nextReadyTime);
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