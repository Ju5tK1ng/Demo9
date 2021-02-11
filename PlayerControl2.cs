using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayState
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
	private PlayState playState;
    private Rigidbody2D myRigidbody2D;
	private RaycastHit2D downBox;
	private RaycastHit2D upBox;
	private RaycastHit2D leftBox;
	private RaycastHit2D rightBox;
	private BoxCollider2D myCollider;
	public int groundLayerMask;
    public float MoveSpeed = 10f;
	public float g = 20f;
	private bool isGround { get { return downBox.collider != null ? true : false; } }

    void Start()
    {
        input = InputManager2.Instance;
        myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        HorizontalMove();
        myRigidbody2D.MovePosition(transform.position + myVelocity * Time.fixedDeltaTime);
    }

    void Update()
    {
		RayCastBox();
        switch (playState)
        {
            case PlayState.Normal:
                Normal();
                break;
            case PlayState.Fall:
                Fall();
                break;
        }
    }

	// 陆地状态
	void Normal()
	{
		if(!isGround)
		{
			playState = PlayState.Fall;
			return;
		}
		myVelocity.y = 0;
	}

	// 落下状态
	void Fall()
	{
		if(isGround)
		{
			playState = PlayState.Normal;
			return;
		}
		if(IsCanFall())
		{
			myVelocity.y -= g * Time.deltaTime;
			myVelocity.y = Mathf.Clamp(myVelocity.y, -200, 200);
		}
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
					if (Mathf.Abs(myVelocity.x) < MoveSpeed / 3)
					{
						myVelocity.x = 0;
					}
					else
					{
						myVelocity.x -= MoveSpeed / 3 * introDir;
					}
				}
				else
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
				Debug.Log(myVelocity.x);
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
