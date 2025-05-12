using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections),typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 30f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    private bool fireInputLocked = false;
    private float fireInputLockDuration = 0.5f;
    private bool isDead = false;

    Vector2 MoveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed
    {
        get
        {
          if(CanMove)
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
                    // Air Move
                    return airWalkSpeed;
                }
            }
            else
            {
                // Idle speed is 0
                return 0;
            }
        } else 
        {    
            //Movement locked
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
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);

        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
{
    get { return _isFacingRight; }
    private set
    {
        if (_isFacingRight != value)
        {
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(value ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
        }

        _isFacingRight = value;
    }
}
    public bool CanMove {  get 
      { 
        return animator.GetBool(AnimationStrings.canMove);
      } 
    }

    public bool IsAlive {  
        get 
        {
            return animator.GetBool(AnimationStrings.isAlive);
        } 
    }
    public bool LockVelocity {  get
        {
          return  animator.GetBool(AnimationStrings.lockVelocity);
        } 
          
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }


    Rigidbody2D rb;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable=GetComponent<Damageable>();
    }



    private void Update()
    {
   

        if (!IsAlive && !isDead)
        {
            isDead = true;

            Invoke(nameof(LoadGameOver), 1f);
        }

        if (transform.position.y < -20f && !isDead)
        {
            isDead = true;

            Invoke(nameof(LoadGameOver), 0f);
        }
    }


    private void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }


    private void FixedUpdate()
    {
        if(!damageable.LockVelocity) 
        rb.linearVelocity = new Vector2(MoveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
                                                                                                                                                                                                                                        
        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        if(IsAlive) 
        {
            IsMoving = MoveInput != Vector2.zero;

            SetFacingDirection(MoveInput);
        }
        else
        {
            IsMoving = false;
        }
       

    }

    private void SetFacingDirection(Vector2 moveInput)
    {

       if(moveInput.x>0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
       else if(moveInput.x<0 && IsFacingRight) 
        {
            IsFacingRight = false;
        }
    
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
        IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started && !fireInputLocked)
        {
            fireInputLocked = true;
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
            Invoke(nameof(UnlockFireInput), fireInputLockDuration);
        }
    }
    private void UnlockFireInput()
    {
        fireInputLocked = false;
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity= new Vector2(knockback.x,rb.linearVelocity.y + knockback.y);
    }


}


