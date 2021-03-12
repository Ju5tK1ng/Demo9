using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    public enum PlayState
    {
        Normal,
		Fall,
    }
    public Vector3 shadowVelocity;
	public PlayState playState;
    private Rigidbody2D shadowRigidbody2D;
	private RaycastHit2D downBox;
	private RaycastHit2D upBox;
	private RaycastHit2D leftBox;
	private RaycastHit2D rightBox;
	private RaycastHit2D downEnemyBox;
	private BoxCollider2D shadowCollider;
	private Animator shadowAnimator;
	private int groundLayerMask;

	// 常量
	private float G = 135f;
	private float MaxFallSpeed = 24f;
	

    void Start()
    {
        shadowRigidbody2D = GetComponent<Rigidbody2D>();
		shadowCollider = GetComponent<BoxCollider2D>();
		shadowAnimator = GetComponent<Animator>();
		groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
		RayCastBox(1);
		SwitchAnimation();
        shadowRigidbody2D.MovePosition(transform.position + shadowVelocity * Time.deltaTime);
        switch (playState)
        {
            case PlayState.Normal:
                NormalState();
                break;
            case PlayState.Fall:
                FallState();
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
		shadowVelocity.y = 0;
	}

	// 落下状态
	void FallState()
	{
		if (onGround)
		{
			playState = PlayState.Normal;
			return;
		}
		Fall();
	}
	# endregion

	# region 动作
	// 落下
    void Fall()
	{
		if (CanFall)
		{
			shadowVelocity.y = Approach(shadowVelocity.y, -MaxFallSpeed, G * Time.deltaTime);
		}
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
	bool CanFall
	{
		get
		{
			return true;
		}
	}
	float shadowFaceDir
	{
		get
		{
			return Mathf.Sign(transform.localScale.x);
		}
	}
	
	void RayCastBox(int HDir)
    {
        rightBox = Physics2D.BoxCast(transform.position, shadowCollider.size * 1f, 0, Vector2.right, 0.1f, groundLayerMask);
        leftBox = Physics2D.BoxCast(transform.position, shadowCollider.size * 1f, 0, Vector2.left, 0.1f, groundLayerMask);
        upBox = Physics2D.BoxCast(transform.position, shadowCollider.size * 1f, 0, Vector2.up * HDir, 0.1f, groundLayerMask);
        downBox = Physics2D.BoxCast(transform.position, shadowCollider.size * 1f, 0, Vector2.down * HDir, 0.1f, groundLayerMask);
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
		if (shadowFaceDir * shadowVelocity.x < 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
		switch (playState)
        {
            case PlayState.Normal:
				shadowAnimator.SetInteger("state", 0);
				break;
			case PlayState.Fall:
                shadowAnimator.SetInteger("state", 2);
                break;
        }
	}
}