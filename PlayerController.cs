using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 10;
    public float jumpSpeed = 10;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private BoxCollider2D myFeet;
    private bool isGround;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Run();
        Jump();
        CheckGrounded();
        SwitchAnimation();
    }
    
    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasXAxisSpeed)
        {
            if(myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if(myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Run", playerHasXAxisSpeed); 
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(isGround)
            {
                myAnimator.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
            }
        }
    }

    void SwitchAnimation()
    {
        myAnimator.SetBool("Idle", false);
        if(myAnimator.GetBool("Jump"))
        {
            if(myRigidbody.velocity.y < 0.0f)
            {
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("Fall", true);
            }
        }
        else if(isGround)
        {
            myAnimator.SetBool("Fall", false);
            myAnimator.SetBool("Idle", true);
        }
    }
}
