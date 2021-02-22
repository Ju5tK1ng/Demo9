using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl3 : MonoBehaviour
{
    public enum PlayState
    {
        Normal,
		Fall,
        Jump,
        Dash,
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
	private int curFrame = 0;
	private float curTime = 0;
	private float dashes;
	private float dashRemainingTime;
	private float dashReaminingCooldown;
	private float skill1CoolDown = 1f;
	private float skill1ReaminingCooldown;
	public Transform prefabSkill1;

	// 常量
    private const float MaxRunSpeed = 13.5f;
    private const float RunAccel = 150f;
    private const float RunReduce = 60f;
	private const float G = 100f;
	private const float MaxFallSpeed = 32f;
	private const float MaxSlipSpeed = 6f;
	private const float JumpSpeed = 30f;
	private const float JumpHBoost = 6f;
	private const float DashSpeed = 36f;
	private const float EndDashSpeed = 24f;
	private const float DashTime = 0.15f;
	private const float DashCooldown = 0.2f;
    private const float AirMult = 0.65f;
	private const float WallJumpHSpeed = MaxRunSpeed + JumpHBoost;
	private const float KickWallJumpHSpeed = WallJumpHSpeed + JumpHBoost;
	

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
		// if (playState != PlayState.Dash)
		// 	Debug.Log(curFrame + " " + curTime + " " + transform.position.x);
		RayCastBox();
		SwitchAnimation();
        HorizontalMove();
        myRigidbody2D.MovePosition(transform.position + myVelocity * Time.deltaTime);
		if (dashReaminingCooldown > 0)
		{
			dashReaminingCooldown -= Time.deltaTime;
		}
		if (skill1ReaminingCooldown > 0)
		{
			skill1ReaminingCooldown -= Time.deltaTime;
		}
		if (CanDash)
		{
			Dash();
		}
		if (CanSkill1)
		{
			Skill1();
		}
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
			case PlayState.Dash:
				DashState();
                break;
        }
    }
	
	# region 状态
	// 陆地状态
	void NormalState()
	{
		if (!onGround)
		{
			playState = PlayState.Fall;
			return;
		}
		dashes = 1;
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

	// 跳跃状态
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

	// 冲刺状态
	void DashState()
	{
		if(dashRemainingTime > 0)
		{
			dashRemainingTime -= Time.deltaTime;
		}
		else 
		{
			myVelocity.x = EndDashSpeed * myFaceDir;
			if(onGround)
			{
				playState = PlayState.Normal;
			}
			else
			{
				playState = PlayState.Fall;
			}
		}
	}
	# endregion

	# region 动作
	// 落下
    void Fall()
	{
		if (CanFall)
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
			myVelocity.x = KickWallJumpHSpeed * -wallDir;
		}
		myVelocity.y = JumpSpeed;
	}

	// 冲刺
	void Dash()
	{
		dashes -= 1;
		dashRemainingTime = DashTime;
		dashReaminingCooldown = DashCooldown;
		if(input.moveDir != 0)
		{
			myVelocity = Vector2.right * DashSpeed * input.moveDir;
		}
		else
		{
			myVelocity = Vector2.right * DashSpeed * myFaceDir;
		}
		playState = PlayState.Dash;
	}

	void Skill1()
	{
		skill1ReaminingCooldown = skill1CoolDown;
		Transform skill1 = Instantiate(prefabSkill1, transform.position, Quaternion.identity);
		skill1.rotation = Quaternion.Euler(0, 0, -myFaceDir * 90f + 90f);
	}
	#endregion

	#region 检测
	private bool onGround
	{
		get
		{
			return downBox.collider != null ? true : false; 
		} 
	}
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
	bool CanMove
    {
        get
		{
			return playState != PlayState.Dash;
		}
    }
	bool CanFall
	{
		get
		{
			return playState != PlayState.Dash;
		}
	}
	bool CanDash
	{
		get
		{
			return input.DashKeyDown && dashReaminingCooldown <= 0 && dashes > 0;
		}
	}
	bool CanSkill1
	{
		get
		{
			return input.Skill1KeyDown && skill1ReaminingCooldown <= 0;
		}
	}
	float myFaceDir
	{
		get
		{
			return Mathf.Sign(transform.localScale.x);
		}
	}
	#endregion
	
	void RayCastBox()
    {
        rightBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.right, 0.1f, groundLayerMask);
        leftBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.left, 0.1f, groundLayerMask);
        upBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.up, 0.1f, groundLayerMask);
        downBox = Physics2D.BoxCast(transform.position, myCollider.size * 3, 0, Vector2.down, 0.1f, groundLayerMask);
    }

    public static float Approach(float curValue, float tarValue, float deltaValue)
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
		if (myFaceDir * myVelocity.x < 0)
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
			case PlayState.Dash:
                myAnimator.SetInteger("state", 4);
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
        if (CanMove)
        {
            float mult = onGround ? 1 : AirMult;
            if (Mathf.Abs(myVelocity.x) > MaxRunSpeed && Mathf.Sign(myVelocity.x) == input.moveDir)
                myVelocity.x = Approach(myVelocity.x, MaxRunSpeed * input.moveDir, RunReduce * mult * Time.deltaTime);  //Reduce back from beyond the max speed
            else
                myVelocity.x = Approach(myVelocity.x, MaxRunSpeed * input.moveDir, RunAccel * mult * Time.deltaTime);   //Approach the max speed
		}
    }
}
