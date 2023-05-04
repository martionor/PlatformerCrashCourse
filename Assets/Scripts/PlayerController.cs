using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float jumpImpulse = 10f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airWalkSpeed = 3f;


    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
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
            else
            {
                //Movement locked (example because of attacking)
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


    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections=GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }


    Vector2 moveInput;

    private void FixedUpdate()
    {
        if (!damageable.lockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive) 
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(OnMove);
        }
        else
        {
            IsMoving = false;
        }
    }

    public bool IsAlive
    {
        get 
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void SetFacingDirection(Action<InputAction.CallbackContext> OnMove)
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


    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        //TODO check if alive as well
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
