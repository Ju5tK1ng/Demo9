using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager2 : MonoBehaviour
{
    public static InputManager2 Instance;
    private PlayerControl2 myPlayerControl2;
    public bool keyIsSet;
    public KeyCode LeftMoveKey;
    public KeyCode RightMoveKey;
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
        myPlayerControl2 = GetComponent<PlayerControl2>();
        KeyInit();
    }

    public void KeyInit()
    {
        if(!keyIsSet)
        {
            LeftMoveKey = KeyCode.LeftArrow;
            RightMoveKey = KeyCode.RightArrow;
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

