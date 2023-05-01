using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
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
        Hit(10);
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }
}
