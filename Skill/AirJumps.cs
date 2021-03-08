using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirJumps : MonoBehaviour {

    public Text airJumpsText;
    public GameObject playerGameObject;
    private PlayerControl3 player;
    private Image skillIcon;
    // private AudioSource SkillSource;

    void Start () 
    {
        player = playerGameObject.GetComponent<PlayerControl3>();
    }

    void Update () 
    {
        airJumpsText.text = player.airJumps.ToString();
    }
}