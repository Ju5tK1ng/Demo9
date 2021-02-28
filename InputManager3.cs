using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager3 : MonoBehaviour
{
    public static InputManager3 Instance;
    public bool keyIsSet;
    public KeyCode LeftMoveKey;
    public KeyCode RightMoveKey;
    public KeyCode Jump;
    public KeyCode Dash;
    public KeyCode Skill1;
    public KeyCode Skill3;
    public KeyCode Skill4;
    public bool JumpKeyDown { get{ return Input.GetKeyDown(Jump); } }
    public bool JumpKey { get { return Input.GetKey(Jump); } }
    public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }
    public bool Skill1KeyDown { get { return Input.GetKeyDown(Skill1); } }
    public bool Skill3KeyDown { get { return Input.GetKeyDown(Skill3); } }
    public bool Skill4KeyDown { get { return Input.GetKeyDown(Skill4); } }
    public float v = 0;
    public float h = 0;
    public int moveDir;
    
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
            Skill1 = KeyCode.X;
            Skill3 = KeyCode.A;
            Skill4 = KeyCode.S;
        }
    }

    void Start()
    {

    }

    private void Update()
    {
        CheckHorizontalMove();
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
    }

    private void CheckHorizontalMove()
    {
		if (Input.GetKeyDown(RightMoveKey) && h <= 0)
		{
				moveDir = 1;
		}
		else if (Input.GetKeyDown(LeftMoveKey) && h >= 0)
		{
		
				moveDir = -1;
		}
		else if (Input.GetKeyUp(RightMoveKey))
		{
			if (Input.GetKey(LeftMoveKey))
			{
				moveDir = -1;
			}
			else
			{
				moveDir = 0;
			}
		}
		else if (Input.GetKeyUp(LeftMoveKey))
		{
			if (Input.GetKey(RightMoveKey))
			{
				moveDir = 1;
			}
			else
			{
				moveDir = 0;
			}
		}
	}
}

