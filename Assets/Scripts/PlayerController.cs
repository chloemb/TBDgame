using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    // Customizable in inspector
    public float GravityScale;
    public float Speed;
    public float JumpHeight;
    public Vector2 WallJumpStrength;
    public float WallJumpLength;
    public float ClingTime;
    public Vector2 DashStrength;
    public float DashLength;
    public float DashCooldown;
    public bool FacingRight;

    // player axes array. Currently: [Horizontal, Jump, Dash, Shoot]
    public string[] PlayerAxes;

    // variables for managing movement and walls
    //[HideInInspector]
    public bool IsGrounded, KnockingBack;
    [HideInInspector] public Vector2 PrevVelocity; // The most recent non-zero velocity

    private Vector2 ClingPosition, LastDashed, PreDashVel;

    // various info about object
    [HideInInspector] public bool TouchWallToRight, TouchWallToLeft, UsedWallJump, WallJumping, 
        UsedDash, CurrentlyDashing, ControlDisabled, DashOnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.gravityScale = GravityScale;

        switch (gameObject.name)
        {
            case "Player 1":
                PlayerAxes[0] = "P1Horizontal";
                PlayerAxes[1] = "P1Jump";
                PlayerAxes[2] = "P1Dash";
                PlayerAxes[3] = "P1Shoot";
                break;
            case "Player 2":
                PlayerAxes[0] = "P2Horizontal";
                PlayerAxes[1] = "P2Jump";
                PlayerAxes[2] = "P2Dash";
                PlayerAxes[3] = "P2Shoot";
                break;
        }

        IsGrounded = false;
        PrevVelocity = new Vector2(1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get Input
        //Debug.Log(PlayerAxes[3]);
        var horizontal = Input.GetAxis(PlayerAxes[0]);
        var jump = Input.GetAxis(PlayerAxes[1]);
        var dash = Input.GetAxis(PlayerAxes[2]);
        var shoot = Input.GetButtonDown(PlayerAxes[3]);

        // Jump from ground if control isn't disabled
        if (IsGrounded && !ControlDisabled)
        {
            if (jump > 0)
            {
                Vector2 errorVector2 = new Vector2(Speed * horizontal, JumpHeight) - new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                IsGrounded = false;
            }
        }

        // Be able to jump off of walls & time amount allowed to cling to wall
        if (TouchWallToLeft || TouchWallToRight && !IsGrounded)
        {
            if (jump > 0 && !UsedWallJump)
            {
                Vector2 errorVector2 = new Vector2(0, WallJumpStrength.y * JumpHeight);
                _rb.velocity = new Vector2(0, 0);

                if (TouchWallToLeft)
                {
                    errorVector2 += new Vector2(WallJumpStrength.x * Speed, 0);
                }
                else
                {
                    errorVector2 -= new Vector2(WallJumpStrength.x * Speed, 0);
                }

                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                WallJumping = true;
                UsedWallJump = true;

                DisableControl();
                Invoke("StopWallJumping", WallJumpLength);
            }

            if (_rb.velocity.magnitude == 0)
            {
                ClingPosition = _rb.position;
                Invoke("WallSlide", ClingTime);
            }
        }

        // Horizontal movement
        if (!ControlDisabled)
        {
            Vector2 forcetoapply = new Vector2(horizontal * Speed, 0) - new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(forcetoapply, ForceMode2D.Impulse);

            if (horizontal > 0)
                FacingRight = true;
            else if (horizontal < 0)
                FacingRight = false;
        }
        
        // Fire Weapon
        if (shoot)
        {
            GetComponent<FireWeapon>().FireDefaultWeapon(FacingRight, gameObject);
        }

        // Dash
        if (dash > 0 && !DashOnCooldown && !UsedDash && !TouchWallToLeft && !TouchWallToRight)
        {
            PreDashVel = _rb.velocity;
            Vector2 dashvel = DashStrength * _rb.velocity.normalized;
            CurrentlyDashing = true;
            UsedDash = true;

            if (TouchWallToLeft || TouchWallToRight)
            {
                if (TouchWallToLeft)
                {
                    dashvel = DashStrength * WallJumpStrength;
                }
                else if (TouchWallToRight)
                {
                    dashvel = new Vector2(-(DashStrength * WallJumpStrength).x,
                        (DashStrength * WallJumpStrength).y);
                }
                Invoke("StopDash", WallJumpLength);
            }
            else
            {
                Invoke("StopDash", DashLength);
            }
            
            _rb.AddForce(dashvel, ForceMode2D.Impulse);
            
            DisableControl();
            PutDashOnCooldown();
            Invoke("RefreshCooldown", DashCooldown);
            
            LastDashed = dashvel;
            _rb.gravityScale = 0;
        }

        // Set PrevVelocity
        if (_rb.velocity != Vector2.zero)
        {
            PrevVelocity = _rb.velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Walls")
        {
            // Detect which side the wall is on
            if (PrevVelocity.x < 0)
            {
                TouchWallToLeft = true;
            }
            else
            {
                TouchWallToRight = true;
            }
        }
        
        // Ground object if on floor
        if (col.gameObject.tag == "Floors")
        {
            IsGrounded = true;

            UsedDash = false;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // Unground object if leaving floor
        if (col.gameObject.tag == "Floors")
        {
            IsGrounded = false;
        }

        if (col.gameObject.tag == "Walls")
        {
            // No longer touching wall
            TouchWallToLeft = false;
            TouchWallToRight = false;

            // Give control back if not wall jumping (i.e. if falling)
            if (!WallJumping && !CurrentlyDashing)
            {
                GiveBackControl();
            }

            // Can only wall jump once (only relevant if WallJumpStrength.x is 0)
            UsedWallJump = false;
        }
    }

    private void StopWallJumping()
    {
        GiveBackControl();
        WallJumping = false;
    }

    private void StopDash()
    {
        GiveBackControl();
        _rb.gravityScale = GravityScale;
        CurrentlyDashing = false;
        if (IsGrounded)
        {
            UsedDash = false;
        }

        _rb.velocity = PreDashVel;
    }

    private void PutDashOnCooldown()
    {
        DashOnCooldown = true;
    }
    
    public void RefreshCooldown()
    {
        DashOnCooldown = false;
    }

    private void WallSlide()
    {
        if (_rb.position == ClingPosition)
        {
            DisableControl();
        }
    }

    public void DisableControl()
    {
        ControlDisabled = true;
    }

    public void GiveBackControl()
    {
        ControlDisabled = false;
    }
}