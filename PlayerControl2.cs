using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState
{
    Normal,
    Jump,
    Dash,
    Fall,
}

public class PlayerControl2 : MonoBehaviour
{
    private InputManager2 input;
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
	public int curFrame = 0;

	// 常量
    private const float MoveSpeed = 12f;
	private const float G = 50f;
	private const float MaxFall = 20f;
	private const float MaxSlipSpeed = 4f;
	private const float JumpSpeed = 20f;
	private const float JumpHBoost = 8f;
	private const float WallJumpHSpeed = MoveSpeed + JumpHBoost;

	// 检测
	private bool isGround { get { return downBox.collider != null ? true : false; } }
	private int wallDir
	{
		get
		{
			if(rightBox.collider != null)
			{
				return 1;
			}
			else if(leftBox.collider != null)
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
        input = InputManager2.Instance;
        myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myAnimator = GetComponent<Animator>();
		groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
		curFrame += 1;
        HorizontalMove();
        myRigidbody2D.MovePosition(transform.position + myVelocity * Time.fixedDeltaTime);
    }

    void Update()
    {
		RayCastBox();
		SwitchAnimation();
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
		if(!isGround)
		{
			playState = PlayState.Fall;
			return;
		}
		myVelocity.y = 0;
		if(input.JumpKeyDown)
		{
			Jump();
		}
	}

	// 落下状态
	void FallState()
	{
		if(input.JumpKeyDown)
		{
			if(wallDir != 0)
			{
				Jump(wallDir);
			}
		}
		if(isGround)
		{
			// Debug.Log(frame);
			playState = PlayState.Normal;
			return;
		}
		Fall();
	}

	void Fall()
	{
		if(IsCanFall())
		{
			if(wallDir != 0 && wallDir == input.moveDir)
			{
				myVelocity.y -= G * Time.deltaTime;
				myVelocity.y = Mathf.Clamp(myVelocity.y, -MaxSlipSpeed, MaxSlipSpeed);
			}
			else
			{
				myVelocity.y -= G * Time.deltaTime;
				myVelocity.y = Mathf.Clamp(myVelocity.y, -MaxFall, MaxFall);
			}
		}
	}

	void JumpState()
	{
		if(input.JumpKeyDown)
		{
			if(wallDir != 0)
			{
				Jump(wallDir);
			}
		}
		if(myVelocity.y <= 0)
		{
			playState = isGround ? PlayState.Normal : PlayState.Fall;
		}
		if(IsCanFall())
		{
			myVelocity.y -= G * Time.deltaTime;
			myVelocity.y = Mathf.Clamp(myVelocity.y, -MaxFall, MaxFall);
		}
	}

	// 跳跃
	void Jump(int wallDir = 0)
	{
		// Debug.Log(frame);
		playState = PlayState.Jump;
		if(isGround)
		{
			myVelocity.x += JumpHBoost * input.moveDir;
		}
		else if(input.moveDir == 0 && wallDir != 0)
		{
			myVelocity.x = JumpHBoost * -wallDir;
		}
		else if(input.moveDir != 0 && wallDir != 0)
		{
			myVelocity.x = WallJumpHSpeed * -wallDir;
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

	void SwitchAnimation()
	{
		// Flip
		if(transform.localScale.x * myVelocity.x < 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
		switch (playState)
        {
            case PlayState.Normal:
				if(myVelocity.x == 0)
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
        if (isCanMove())
        {
            // 减速
            if (myVelocity.x != 0 && ((myVelocity.x > 0 && input.moveDir == -1) || (myVelocity.x < 0 && input.moveDir == 1)
				|| input.moveDir == 0 || (isGround && input.v < 0) || Mathf.Abs(myVelocity.x) > MoveSpeed))
            {
                int introDir = myVelocity.x > 0 ? 1 : -1;
				if (isGround)
				{
					if (Mathf.Abs(myVelocity.x) < MoveSpeed / 6)
					{
						myVelocity.x = 0;
					}
					else
					{
						myVelocity.x -= MoveSpeed / 6 * introDir;
					}
				}
				else
				{
					if (Mathf.Abs(myVelocity.x) < MoveSpeed / 12)
					{
						myVelocity.x = 0;
					}
					else
					{
						myVelocity.x -= MoveSpeed / 12 * introDir;
					}
				}
            }
			// 加速
			else
			{
				//蹲下不允许移动
				if (isGround && input.v < 0)
				{
					return;
				}
				if (input.moveDir != 0 && !(isGround && input.v < 0))
				{
					if (isGround)
					{
						myVelocity.x += MoveSpeed / 6 * input.moveDir;
					}
					else
					{
						myVelocity.x += MoveSpeed / 15 * input.moveDir;
					}
					if (myVelocity.x * input.moveDir > MoveSpeed)
					{
						myVelocity.x = MoveSpeed * input.moveDir;
					}
					// Debug.Log(myVelocity.x);
				}
			}
        }
    }
    bool isCanMove()
    {
        return true;
    }
}
