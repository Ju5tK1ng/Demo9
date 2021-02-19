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
    public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }
    public bool JumpKeyDown {
        get
        {
            if(Input.GetKeyDown(Jump))
            {
                jumpFrame = 0;
				return true;
            }
            else if(jumpFrame > 0)
            {
                jumpFrame = 0;
				return true;
            }
            return false;
        }
    }
    public float v = 0;
    public float h = 0;
    public int moveDir;
    public int jumpFrame;
    
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
        }
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        if(jumpFrame > 0)
        {
            jumpFrame--;
        }
    }

    private void Update()
    {
        CheckHorizontalMove();
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(Jump))
        {
            jumpFrame = 3;  // 缓存跳跃键3帧
        }
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

