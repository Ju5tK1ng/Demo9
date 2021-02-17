using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl3 : MonoBehaviour
{
    public enum PlayState
    {
        Normal,
        Jump,
        Dash,
        Fall,
    }
    private InputManager3 input;
    public Vector3 myVelocity;
	public PlayState playState;
    private Rigidbody2D myRigidbody2D;
	private RaycastHit2D downBox;
	private RaycastHit2D upBox;
	private RaycastHit2D leftBox;
	private RaycastHit2D rightBox;
	private BoxCollider2D myCollider;
	private Animator myAnimator;
	private int groundLayerMask;
	private int myFaceDir;
	private int curFrame = 0;
	private float curTime = 0;

	// 常量
    private const float MaxRunSpeed = 13.5f;
    private const float RunAccel = 150f;
    private const float RunReduce = 60f;
	private const float G = 100f;
	private const float MaxFallSpeed = 32f;
	private const float MaxSlipSpeed = 6f;
	private const float JumpSpeed = 30f;
	private const float JumpHBoost = 6f;
    private const float AirMult = 0.65f;
	private const float WallJumpHSpeed = MaxRunSpeed + JumpHBoost;

	// 检测
	private bool onGround { get { return downBox.collider != null ? true : false; } }
	private int wallDir
	{
		get
		{
			if (rightBox.collider != null)
			{
				return 1;
			}
			else if (leftBox.collider != null)
			{
				return -1;
			}
			else
			{
				return 0;
		}
		}
	}

    void Start()
    {
        input = InputManager3.Instance;
        myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myAnimator = GetComponent<Animator>();
		groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
		// curFrame += 1;
		// curTime += Time.deltaTime;
		// if (transform.position.x > 16.9 && transform.position.x < 17.1 || transform.position.x > -17.1 && transform.position.x < -16.9)
		// 	Debug.Log(curFrame + " " + curTime + " " + transform.position.x);
		RayCastBox();
		SwitchAnimation();
        HorizontalMove();
        myRigidbody2D.MovePosition(transform.position + myVelocity * Time.deltaTime);
        switch (playState)
        {
            case PlayState.Normal:
                NormalState();
                break;
            case PlayState.Fall:
                FallState();
                break;
			case PlayState.Jump:
				JumpState();
                break;
        }
    }

	// 陆地状态
	void NormalState()
	{
		if (!onGround)
		{
			playState = PlayState.Fall;
			return;
		}
		myVelocity.y = 0;
		if (input.JumpKeyDown)
		{
			Jump();
		}
	}

	// 落下状态
	void FallState()
	{
		if (input.JumpKeyDown)
		{
			if (wallDir != 0)
			{
				Jump(wallDir);
			}
		}
		if (onGround)
		{
			// Debug.Log(frame);
			playState = PlayState.Normal;
			return;
		}
		Fall();
	}

	void JumpState()
	{
		if (input.JumpKeyDown)
		{
			if (wallDir != 0)
			{
				Jump(wallDir);
			}
		}
		if (myVelocity.y <= 0)
		{
			playState = onGround ? PlayState.Normal : PlayState.Fall;
		}
		Fall();
	}

    	void Fall()
	{
		if (IsCanFall())
		{
			if (wallDir != 0 && wallDir == input.moveDir)
			{
                myVelocity.y = Approach(myVelocity.y, -MaxSlipSpeed, G * Time.deltaTime);
			}
			else
			{
				myVelocity.y = Approach(myVelocity.y, -MaxFallSpeed, G * Time.deltaTime);
			}
		}
	}

	// 跳跃
	void Jump(int wallDir = 0)
	{
		// Debug.Log(frame);
		playState = PlayState.Jump;
		if (onGround)
		{
			myVelocity.x += JumpHBoost * input.moveDir;
		}
		else if (input.moveDir == 0 && wallDir != 0)
		{
			myVelocity.x = WallJumpHSpeed * -wallDir;
		}
		else if (input.moveDir != 0 && wallDir != 0)
		{
			myVelocity.x = (WallJumpHSpeed + JumpHBoost) * -wallDir;
		}
		myVelocity.y = JumpSpeed;
	}

	bool IsCanFall()
	{
		return true;
	}
	
	void RayCastBox()
    {
        rightBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.right, 0.1f, groundLayerMask);
        leftBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.left, 0.1f, groundLayerMask);
        upBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.up, 0.1f, groundLayerMask);
        downBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.down, 0.1f, groundLayerMask);
    }

    float Approach(float curValue, float tarValue, float deltaValue)
    {
        if (curValue < tarValue)
        {
            curValue += deltaValue;
            if (curValue > tarValue)
            {
                curValue = tarValue;
            }
        }
        else if (curValue > tarValue)
        {
            curValue -= deltaValue;
            if (curValue < tarValue)
            {
                curValue = tarValue;
            }
        }
        return curValue;
    }

	void SwitchAnimation()
	{
		// Flip
		if (transform.localScale.x * myVelocity.x < 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
		switch (playState)
        {
            case PlayState.Normal:
				if (myVelocity.x == 0)
				{
					myAnimator.SetInteger("state", 0);
				}
                else
				{
					myAnimator.SetInteger("state", 1);
				}
                break;
			case PlayState.Fall:
                myAnimator.SetInteger("state", 2);
                break;
			case PlayState.Jump:
                myAnimator.SetInteger("state", 3);
                break;
        }
	}

	// void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;

    //     //Check if there has been a hit yet
    //     if (downBox)
    //     {
    //         //Draw a Ray forward from GameObject toward the hit
    //         Gizmos.DrawRay(transform.position, transform.forward * 0.1f);
    //         //Draw a cube that extends to where the hit exists
    //         Gizmos.DrawWireCube(transform.position + transform.forward * 0.1f, myCollider.size * 3);
    //     }
    //     //If there hasn't been a hit yet, draw the ray at the maximum distance
    //     else
    //     {
    //         //Draw a Ray forward from GameObject toward the maximum distance
    //         Gizmos.DrawRay(transform.position, transform.forward * 0.1f);
    //         //Draw a cube at the maximum distance
    //         Gizmos.DrawWireCube(transform.position + transform.forward * 0.1f, myCollider.size * 3);
    //     }
    // }

    void HorizontalMove()
    {
        // 蹲下暂未实现
        if (isCanMove())
        {
            float mult = onGround ? 1 : AirMult;
            if (Mathf.Abs(myVelocity.x) > MaxRunSpeed && Mathf.Sign(myVelocity.x) == input.moveDir)
                myVelocity.x = Approach(myVelocity.x, MaxRunSpeed * input.moveDir, RunReduce * mult * Time.deltaTime);  //Reduce back from beyond the max speed
            else
                myVelocity.x = Approach(myVelocity.x, MaxRunSpeed * input.moveDir, RunAccel * mult * Time.deltaTime);   //Approach the max speed
		}
    }
    bool isCanMove()
    {
        return true;
    }
}
