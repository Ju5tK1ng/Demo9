using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager3 : MonoBehaviour
{
    public static InputManager3 Instance;
    public bool keyIsSet;
    public KeyCode LeftMoveKey;
    public KeyCode RightMoveKey;
    public KeyCode Jump;
    public KeyCode Dash;
    public KeyCode SkillTreeKey;
    public Canvas skillTree;
    public Image dash;
    private bool seeSkillTree;
    // public KeyCode Skill4;
    public KeyCode Skill5;
    public bool JumpKeyDown { get{ return Input.GetKeyDown(Jump); } }
    public bool JumpKey { get { return Input.GetKey(Jump); } }
    public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }
    // public bool Skill4KeyDown { get { return Input.GetKeyDown(Skill4); } }
    public bool Skill5KeyDown { get { return Input.GetKeyDown(Skill5); } }
    public float v = 0;
    public float h = 0;
    public float moveDir;
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        KeyInit();
    }

    public void KeyInit()
    {
        if(!keyIsSet)
        {
            LeftMoveKey = KeyCode.LeftArrow;
            RightMoveKey = KeyCode.RightArrow;
            Jump = KeyCode.C;
            Dash = KeyCode.Z;
            SkillTreeKey = KeyCode.Tab;
            // Skill4 = KeyCode.S;
            Skill5 = KeyCode.D;
        }
        dash.GetComponent<SkillCoolDown>().skillButton = Dash;
    }

    void Start()
    {

    }

    private void Update()
    {
        CheckHorizontalMove();
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(SkillTreeKey))
        {
            if(skillTree)
            {
                seeSkillTree = !seeSkillTree;
                skillTree.gameObject.SetActive(seeSkillTree);
            }
        }
    }

    private void CheckHorizontalMove()
    {
        if (v != 0)
        {
            moveDir = 0;
        }
		else if (Input.GetKeyDown(RightMoveKey) && h <= 0)
		{
			moveDir = 1;
		}
		else if (Input.GetKeyDown(LeftMoveKey) && h >= 0)
		{
		
			moveDir = -1;
		}
        else if (Input.GetKey(LeftMoveKey) && Input.GetKey(RightMoveKey))
        {
        
        }
        else
        {
            moveDir = h;
        }
		// else if (Input.GetKeyUp(RightMoveKey))
		// {
		// 	if (Input.GetKey(LeftMoveKey))
		// 	{
		// 		moveDir = -1;
		// 	}
		// 	else
		// 	{
		// 		moveDir = 0;
		// 	}
		// }
		// else if (Input.GetKeyUp(LeftMoveKey))
		// {
		// 	if (Input.GetKey(RightMoveKey))
		// 	{
		// 		moveDir = 1;
		// 	}
		// 	else
		// 	{
		// 		moveDir = 0;
		// 	}
		// }
	}
}

