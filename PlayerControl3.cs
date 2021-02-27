﻿using System.Collections;
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
	private RaycastHit2D downEnemyBox;
	private BoxCollider2D myCollider;
	private Animator myAnimator;
	private int groundLayerMask;
	private int enemyLayerMask;
	private int curFrame = 0;
	private float curTime = 0;
	public float airJumps;
	private float varJumpTimer;
	private float jumpGraceTimer;
    private float jumpBufferTimer;
	private float wallSlideTimer;
	private float dashes;
	private float dashTimer;
	private float dashCoolDownTimer;
	private float skill1CoolDown = 1f;
	private float skill1CoolDownTimer;
	public Transform prefabSkill1;
	private bool skill2 = true;
	private float skill2Damage = 1f;
	private int skill3 = 1;

	// 常量
    private const float MaxRunSpeed = 13.5f;
    private const float RunAccel = 150f;
    private const float RunReduce = 60f;
	private float G;
	// private const float HalfGThreshold = 6f;
	private float MaxFallSpeed;
	private const float WallSlideTime = 1.2f;
	private float WallSlideStartMax;
	private float JumpSpeed;
	private const float VarJumpTime = 0.16f;
	private const float JumpHBoost = 6f;
	private const float JumpGraceTime = 0.05f;
	private const float JumpBufferTime = 0.1f;
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
		enemyLayerMask = LayerMask.GetMask("Enemy");
		G = 135f;
		MaxFallSpeed = 24f;
		WallSlideStartMax = 3f;
		JumpSpeed = 20f;
    }

    void Update()
    {
		// curFrame += 1;
		// curTime += Time.deltaTime;
		// if (transform.position.y > -5.05)
		// 	Debug.Log(curFrame + " " + curTime + " " + transform.position.y);
		// if (onGround)
		// 	Debug.Log(curFrame + " " + curTime + " " + transform.position.y);
		RayCastBox(skill3);
		SwitchAnimation();
        HorizontalMove();
        myRigidbody2D.MovePosition(transform.position + myVelocity * Time.deltaTime);
		Timer();
		if (CanDash)
		{
			Dash();
		}
		if (CanJump)
		{
			Jump();
		}
		if (CanSkill1)
		{
			Skill1();
		}
		if (CanSkill3)
		{
			Skill3();
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

	// 计时器
	private void Timer()
	{
		if (dashCoolDownTimer > 0)
		{
			dashCoolDownTimer -= Time.deltaTime;
		}
		if (skill1CoolDownTimer > 0)
		{
			skill1CoolDownTimer -= Time.deltaTime;
		}
		if (varJumpTimer > 0)
		{
			varJumpTimer -= Time.deltaTime;
		}
		if (onGround)
		{
			jumpGraceTimer = JumpGraceTime;
		}
		else if (jumpGraceTimer > 0)
		{
			jumpGraceTimer -= Time.deltaTime;
		}
		if (input.JumpKeyDown)
        {
            jumpBufferTimer = JumpBufferTime;
        }
        else if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
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
		airJumps = 1;
		myVelocity.y = 0;
	}

	// 落下状态
	void FallState()
	{
		if (CanWallJump)
		{
			Jump(wallDir);
		}
		else if (onEnemy)
		{
			airJumps = 1;
			dashes = 1;
			downEnemyBox.collider.GetComponent<Enemy>().TakeDamage(skill2Damage);
			Jump();
		}
		else if (onGround)
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
		if (CanWallJump)
		{
			Jump(wallDir);
		}
		if (myVelocity.y * skill3 <= 0)
		{
			playState = onGround ? PlayState.Normal : PlayState.Fall;
		}
		if (varJumpTimer > 0 && input.JumpKey)
		{
			myVelocity.y = JumpSpeed;
		}
		else
		{
			Fall();
		}
	}

	// 冲刺状态
	void DashState()
	{
		if(dashTimer > 0)
		{
			dashTimer -= Time.deltaTime;
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
			// float mult = (Mathf.Abs(myVelocity.y) < HalfGThreshold && input.JumpKey) ? 0.5f : 1f;
			if (wallDir != 0 && wallDir == input.moveDir)
			{
				float maxWallSlideSpeed = Mathf.Lerp(MaxFallSpeed, WallSlideStartMax, wallSlideTimer / WallSlideTime);
				myVelocity.y = Approach(myVelocity.y, -maxWallSlideSpeed, G * Time.deltaTime);
				wallSlideTimer = Mathf.Max(wallSlideTimer - Time.deltaTime, 0);
			}
			else
			{
				wallSlideTimer = WallSlideTime;
				myVelocity.y = Approach(myVelocity.y, -MaxFallSpeed, G * Time.deltaTime);
			}
		}
	}

	// 跳跃
	void Jump(int wallDir = 0)
	{
		// Debug.Log(frame);
		varJumpTimer = VarJumpTime;
		jumpBufferTimer = 0;
		wallSlideTimer = WallSlideTime;
		playState = PlayState.Jump;
		if (onGround || onEnemy || jumpGraceTimer > 0)
		{
			myVelocity.x += JumpHBoost * input.moveDir;
		}
		else if (wallDir == 0)
		{
			airJumps -= 1;
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
		jumpGraceTimer = 0;
	}

	// 冲刺
	void Dash()
	{
		dashes -= 1;
		dashTimer = DashTime;
		dashCoolDownTimer = DashCooldown;
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
		skill1CoolDownTimer = skill1CoolDown;
		Transform skill1 = Instantiate(prefabSkill1, transform.position, Quaternion.identity);
		skill1.rotation = Quaternion.Euler(0, 0, -myFaceDir * 90f + 90f);
	}

	void Skill3()
	{
		skill3 = -skill3;
		Vector3 scaleFlipY = transform.localScale;
		scaleFlipY.y = -scaleFlipY.y;
		transform.localScale = scaleFlipY;
		MaxFallSpeed = -MaxFallSpeed;
		WallSlideStartMax = -WallSlideStartMax;
		JumpSpeed = -JumpSpeed;
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
	private bool onEnemy
	{
		get
		{
			return downEnemyBox.collider != null ? true : false;
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
	bool CanJump
	{
		get
		{
			return (input.JumpKeyDown || jumpBufferTimer > 0) && playState != PlayState.Dash && (onGround || jumpGraceTimer > 0 || airJumps > 0 && wallDir == 0);
		}
	}
	bool CanWallJump
	{
		get
		{
			return input.JumpKeyDown && wallDir != 0;
		}
	}
	bool CanDash
	{
		get
		{
			return input.DashKeyDown && dashCoolDownTimer <= 0 && dashes > 0;
		}
	}
	bool CanSkill1
	{
		get
		{
			return input.Skill1KeyDown && skill1CoolDownTimer <= 0;
		}
	}
	bool CanSkill3
	{
		get
		{
			return input.Skill3KeyDown;
		}
	}
	float myFaceDir
	{
		get
		{
			return Mathf.Sign(transform.localScale.x);
		}
	}
	
	void RayCastBox(int HDir)
    {
        rightBox = Physics2D.BoxCast(transform.position, myCollider.size * 1f, 0, Vector2.right, 0.1f, groundLayerMask);
        leftBox = Physics2D.BoxCast(transform.position, myCollider.size * 1f, 0, Vector2.left, 0.1f, groundLayerMask);
        upBox = Physics2D.BoxCast(transform.position, myCollider.size * 1f, 0, Vector2.up * HDir, 0.1f, groundLayerMask);
        downBox = Physics2D.BoxCast(transform.position, myCollider.size * 1f, 0, Vector2.down * HDir, 0.1f, groundLayerMask);
		downEnemyBox = Physics2D.BoxCast(transform.position, myCollider.size * 1f, 0, Vector2.down * HDir, 0.1f, enemyLayerMask);
    }
	#endregion

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
    //         Gizmos.DrawWireCube(transform.position + transform.forward * 0.1f, myCollider.size * 1);
    //     }
    //     //If there hasn't been a hit yet, draw the ray at the maximum distance
    //     else
    //     {
    //         //Draw a Ray forward from GameObject toward the maximum distance
    //         Gizmos.DrawRay(transform.position, transform.forward * 0.1f);
    //         //Draw a cube at the maximum distance
    //         Gizmos.DrawWireCube(transform.position + transform.forward * 0.1f, myCollider.size * 1);
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
