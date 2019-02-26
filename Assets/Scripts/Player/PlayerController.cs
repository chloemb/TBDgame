﻿using System;
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

    // player axes array. Currently: [Horizontal, Jump, Dash, Shoot, Vertical, Offhand]
    public string[] PlayerAxes;
    private Collider2D _col;

    // variables for managing movement and walls
    public bool IsGrounded;
    [HideInInspector] public Vector2 PrevVelocity; // The most recent non-zero velocity

    private Vector2 ClingPosition, LastDashed, PreDashVel, LastFired;

    // various info about object
    // [HideInInspector]
    public bool
        UsedWallJump,
        WallJumping,
        UsedDash,
        CurrentlyDashing,
        DashOnCooldown,
        FacingRight,
        ControlDisabled,
        TouchWallToRight,
        TouchWallToLeft;

    private void Start()
    {
        _col = GetComponent<Collider2D>();
    }

    public void SetUpControls()
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
                PlayerAxes[4] = "P1Vertical";
                PlayerAxes[5] = "P1Offhand";
                break;
            case "Player 2":
                PlayerAxes[0] = "P2Horizontal";
                PlayerAxes[1] = "P2Jump";
                PlayerAxes[2] = "P2Dash";
                PlayerAxes[3] = "P2Shoot";
                PlayerAxes[4] = "P2Vertical";
                PlayerAxes[5] = "P2Offhand";
                break;
        }

        IsGrounded = false;
        PrevVelocity = new Vector2(1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundedRay();
        LeftWallRay();
        RightWallRay();
        
        if (!gameObject.GetComponent<HealthManager>().InGracePeriod)
        {
            // Get Input
            var horizontal = Input.GetAxis(PlayerAxes[0]);
            var jump = Input.GetAxis(PlayerAxes[1]);
            var dash = Input.GetAxis(PlayerAxes[2]);
            var shoot = Input.GetAxis(PlayerAxes[3]);
            var vertical = Input.GetAxis(PlayerAxes[4]);
            var offhand = Input.GetAxis(PlayerAxes[5]);

            // Be able to jump off of walls & time amount allowed to cling to wall
            if ((TouchWallToLeft || TouchWallToRight) && !IsGrounded)
            {
                if (jump > 0 && !UsedWallJump)
                {
                    Vector2 errorVector2 = new Vector2(0, WallJumpStrength.y * JumpHeight);
                    _rb.velocity = new Vector2(0, 0);
                    errorVector2 = TouchWallToLeft
                        ? errorVector2 + new Vector2(WallJumpStrength.x * Speed, 0)
                        : errorVector2 - new Vector2(WallJumpStrength.x * Speed, 0);
                    _rb.AddForce(errorVector2, ForceMode2D.Impulse);

                    WallJumping = UsedWallJump = true;
                    DisableControl();
                    Invoke("StopWallJumping", WallJumpLength);
                }

                if (_rb.velocity.magnitude == 0)
                {
                    ClingPosition = _rb.position;
                    Invoke("WallSlide", ClingTime);
                }
            }

            if (!ControlDisabled)
            {
                // Jump from ground if control isn't disabled
                if (IsGrounded)
                {
                    if (jump > 0)
                    {
                        Vector2 errorVector2 =
                            new Vector2(Speed * horizontal, JumpHeight) - new Vector2(_rb.velocity.x, 0);
                        _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                        //IsGrounded = false;
                    }
                }

                // Horizontal movement
                Vector2 forcetoapply = new Vector2(horizontal * Speed, 0) - new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(forcetoapply, ForceMode2D.Impulse);

                if (horizontal > 0)
                    FacingRight = true;
                else if (horizontal < 0)
                    FacingRight = false;

                // Determine left stick angle; if none, the direction the player is facing. Magnitude is 1.
                Vector2 LeftStickAngle = new Vector2(horizontal, vertical).normalized;
                if (LeftStickAngle.magnitude == 0)
                    LeftStickAngle = FacingRight ? new Vector2(1f, 0f) : new Vector2(-1f, 0f);
                
                // Remove this section to have different, slightly more frustrating but slightly more precise aiming
                if (LeftStickAngle.x >= .3f && LeftStickAngle.x <= .7f)
                    LeftStickAngle.x = .5f;
                else if (LeftStickAngle.x < .3f && LeftStickAngle.x > -.3f)
                    LeftStickAngle.x = 0;
                else if (LeftStickAngle.x < -.3f && LeftStickAngle.x >= -.7f)
                    LeftStickAngle.x = -0.5f;
                else if (LeftStickAngle.x < -.7f)
                    LeftStickAngle.x = -1;
                else
                    LeftStickAngle.x = 1;

                if (LeftStickAngle.y >= .3f && LeftStickAngle.y <= .7f)
                    LeftStickAngle.y = .5f;
                else if (LeftStickAngle.y < .3f && LeftStickAngle.y > -.3f)
                    LeftStickAngle.y = 0;
                else if (LeftStickAngle.y < -.3f && LeftStickAngle.y >= -.7f)
                    LeftStickAngle.y = -0.5f;
                else if (LeftStickAngle.y < -.7f)
                    LeftStickAngle.y = -1;
                else
                    LeftStickAngle.y = 1;

                // Fire Weapon
                if (shoot > 0 && !TouchWallToLeft && !TouchWallToRight)
                {
                    GetComponent<FireWeapon>().Fire(LeftStickAngle);
                    LastFired = LeftStickAngle;
                }
                
                // Fire offhand weapon
                if (offhand > 0 && !TouchWallToLeft && !TouchWallToRight)
                {
                    GetComponent<FireWeapon>().FireOffhand(LeftStickAngle);
                    LastFired = LeftStickAngle;
                }

                // Dash
                if (dash > 0 && !DashOnCooldown && !UsedDash)
                {
                    if (!IsGrounded && _rb.velocity.x == 0 && _rb.velocity.y > 0)
                    {
                        LeftStickAngle = new Vector2(0f, 1f);
                    }

                    PreDashVel = _rb.velocity;

                    //Increase strength if the player dashed from stationary
                    if ((LeftStickAngle.x.Equals(1) || LeftStickAngle.x.Equals(-1)) && LeftStickAngle.y.Equals(0))
                        LeftStickAngle = FacingRight ? new Vector2(1.5f, 0) : new Vector2(-1.5f, 0);
                    
                    Vector2 dashvel = DashStrength * LeftStickAngle;

                    if (TouchWallToLeft || TouchWallToRight)
                    {
                        if (TouchWallToLeft)
                            dashvel = DashStrength * WallJumpStrength;
                        else if (TouchWallToRight)
                            dashvel = new Vector2(-(DashStrength * WallJumpStrength).x,
                                (DashStrength * WallJumpStrength).y);

                        Invoke("StopDash", DashLength);
                    }
                    else
                    {
                        Invoke("StopDash", DashLength);
                    }

                    _rb.velocity = new Vector2(0, 0);
                    _rb.AddForce(dashvel, ForceMode2D.Impulse);

                    DisableControl();
                    PutDashOnCooldown();
                    Invoke("RefreshCooldown", DashCooldown);

                    CurrentlyDashing = UsedDash = true;
                    LastDashed = dashvel;
                    _rb.gravityScale = 0;
                }
            }

            // Set PrevVelocity
            PrevVelocity = (_rb.velocity != Vector2.zero) ? _rb.velocity : PrevVelocity;
        }
    }

    private void GroundedRay()
    {
        Vector2 leftbound = transform.position - new Vector3(_col.bounds.extents.x, 0);
        Vector2 rightbound = transform.position + new Vector3(_col.bounds.extents.x, 0);
        bool GroundLeft = Physics2D.Raycast(leftbound, Vector2.down, _col.bounds.extents.y * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool GroundRight = Physics2D.Raycast(rightbound, Vector2.down, _col.bounds.extents.y * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool TouchingGround = GroundLeft || GroundRight;

        if (IsGrounded != TouchingGround)
        {
            IsGrounded = TouchingGround;
            if (IsGrounded)
            {
                GiveBackControl();
                UsedDash = UsedWallJump = false;
            }
        }
    }

    private void LeftWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool LeftDown = Physics2D.Raycast(downbound, Vector2.left, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool LeftUp = Physics2D.Raycast(upbound, Vector2.left, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool TouchingLeft = LeftDown || LeftUp;

        if (TouchWallToLeft != TouchingLeft)
        {
            TouchWallToLeft = TouchingLeft;
            if (!TouchWallToLeft)
            {
                UsedWallJump = false;
                if (!WallJumping && !CurrentlyDashing)
                {
                    GiveBackControl();
                }
            }
        }
    }
    
    private void RightWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool RightDown = Physics2D.Raycast(downbound, Vector2.right, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool RightUp = Physics2D.Raycast(upbound, Vector2.right, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces"));
        bool TouchingRight = RightDown || RightUp;

        if (TouchWallToRight != TouchingRight)
        {
            TouchWallToRight = TouchingRight;
            if (!TouchWallToRight)
            {
                UsedWallJump = false;
                if (!WallJumping && !CurrentlyDashing)
                {
                    GiveBackControl();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
//        if (col.gameObject.tag == "Walls")
//        {
//            // Detect which side the wall is on
//            if (PrevVelocity.x < 0)
//                TouchWallToLeft = true;
//            else
//                TouchWallToRight = true;
//        }

//        // Ground object if on floor
//        if (col.gameObject.tag == "Floors")
//        {
//            IsGrounded = true;
//            UsedDash = false;
//        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // Unground object if leaving floor
//        if (col.gameObject.tag == "Floors")
//        {
//            IsGrounded = false;
//        }

//        if (col.gameObject.tag == "Walls")
//        {
//            // No longer touching wall & can only wall jump once (only relevant if WallJumpStrength.x is 0)
//            TouchWallToLeft = TouchWallToRight = UsedWallJump = false;
//
//            // Give control back if not wall jumping (i.e. if falling)
//            if (!WallJumping && !CurrentlyDashing)
//                GiveBackControl();
//        }
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
            UsedDash = false;

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