﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float Speed;
    public float JumpHeight;
    public float WallJumpStrength;
    public float MaxVelocity;
    public Vector2 CurrentVelocity;
    
    // player axes array. Currently: [Horizontal, Jump]
    public string[] PlayerAxes;
    
    // variables for managing movement and walls
    // [HideInInspector]
    public bool IsGrounded;
    private bool TouchWallToRight;
    private bool TouchWallToLeft;
    public bool IgnoreLeft;
    public bool IgnoreRight;
    public Vector2 _prevVelocity;
    private bool UsedWallJump;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();

        switch (gameObject.name)
        {
            case "Player 1":
                PlayerAxes[0] = "P1Horizontal";
                PlayerAxes[1] = "P1Jump";
                break;
            case "Player 2":
                PlayerAxes[0] = "P2Horizontal";
                PlayerAxes[1] = "P2Jump";
                break;
        }

        IsGrounded = false;
        _prevVelocity = new Vector2(1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentVelocity = _rb.velocity;
        
        var horizontal = Input.GetAxis(PlayerAxes[0]);
        var jump = Input.GetAxis(PlayerAxes[1]);

        if (IsGrounded)
        {
            if (jump > 0)
            {
                Vector2 errorVector2 = new Vector2(Speed * horizontal, JumpHeight) - new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                IsGrounded = false;
            }
        }

        if (TouchWallToLeft || TouchWallToRight)
        {
            if (jump > 0 && !UsedWallJump)
            {
                Vector2 errorVector2 = new Vector2(0, JumpHeight);
                _rb.velocity = new Vector2(0, 0);
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);

                if (TouchWallToLeft)
                {
                    IgnoreLeft = true;
                    errorVector2 = errorVector2 + new Vector2(WallJumpStrength * Speed, 0);
                }
                else
                {
                    IgnoreRight = true;
                    errorVector2 = errorVector2 - new Vector2(WallJumpStrength * Speed, 0);
                }
                
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                UsedWallJump = true;
                
                _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, MaxVelocity);
            }
        }

        Vector2 forcetoapply = new Vector2(horizontal * Speed, 0) - new Vector2(_rb.velocity.x, 0);
        if (IgnoreLeft && forcetoapply.x < 0 || IgnoreRight && forcetoapply.x > 0)
        {
            Debug.Log("set x forcetoapply to 0");
            forcetoapply.x = 0;
        }

        _rb.AddForce(forcetoapply, ForceMode2D.Impulse);

        if (_rb.velocity != Vector2.zero)
        {
            _prevVelocity = _rb.velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Floors")
        {
            IsGrounded = true;
        }

        if (col.gameObject.tag == "Walls")
        {
            if (_prevVelocity.x < 0)
            {
                TouchWallToLeft = true;
            }
            else
            {
                TouchWallToRight = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Floors")
        {
            IsGrounded = false;
        }

        if (col.gameObject.tag == "Walls")
        {
            TouchWallToLeft = false;
            TouchWallToRight = false;
            IgnoreLeft = false;
            IgnoreRight = false;
            UsedWallJump = false;
        }
    }
}