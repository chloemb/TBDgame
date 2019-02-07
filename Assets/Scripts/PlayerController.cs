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
    private bool IsGrounded;

    public string[] PlayerAxes;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.name)
        {
            case "Player 1":
                PlayerAxes[0] = "P1Horizontal";
                break;
            case "Player 2":
                PlayerAxes[0] = "P2Horizontal";
                break;
        }
        _rb = GetComponent<Rigidbody2D>();
        IsGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis(PlayerAxes[0]);
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        { 
            Vector2 movement = new Vector2(Speed * horizontal, JumpHeight);
            _rb.velocity = new Vector2(0, 0);
            _rb.AddForce(movement, ForceMode2D.Impulse);
            IsGrounded = false;
        }
        else
        {
            _rb.velocity = new Vector2(Speed * horizontal, _rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Surface")
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Surface")
        {
            IsGrounded = false;
        }
    }
}
