using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public float jumpImpulse = 10f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airWalkSpeed = 3f;

    Vector2 moveInput;
    TouchingDirections touchingDirections;

    public float CurrentMoveSpeed
    {
        get
        {
            if(IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    //Air walk move speed
                    return airWalkSpeed;
                }
               
            }
            else
            {
                //Idle speed is 0
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);

        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight {
        //Flip only if value is new
        get {
            return _isFacingRight; 
        } 
        private set { 
            if(_isFacingRight != value)
                {
                //Flip the local scale to make player face opposite direction
                transform.localScale *= new Vector2(-1, 1);
                }
            _isFacingRight = value;
        }}


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections=GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(onMove);
    }

    private void SetFacingDirection(Action<InputAction.CallbackContext> onMove)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //Face right
            IsFacingRight = true;
        }else if (moveInput.x < 0 && IsFacingRight)
        {
            //Face left
            IsFacingRight= false;
        }
    }

    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        //TODO check if alive as well
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}
