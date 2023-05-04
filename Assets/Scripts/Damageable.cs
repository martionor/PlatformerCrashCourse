using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    [SerializeField]
    private float _maxHealth = 100;

    public float MaxHealth
    {
        get
        {
            return (_maxHealth);
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private float _health = 100;

    public float Health
    {
        get 
        { 
            return (_health); 
        }
        set 
        {
            _health = value;

            //If health falls bellow 0 the character is no longer alive
            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive 
    { 
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("Is alive " + value);
        }
    }


    //The velocity should not be changed while this is true but needs to be respected by other physics components
    // like Player Controller
    public bool lockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInvincible) 
        {
            if (timeSinceHit > invincibilityTime)
            {
                //Remove Invincibility
                isInvincible=false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        //Used for testing purposes if animation works
        //Hit(10);
    }

    //Return whether damageable took hit or not
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            lockVelocity = true;
            //Notify other subscribed components that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            //Debug.Log("hit");
            return true;
        }
        //Not possible to hit
        return false;
    }

}
