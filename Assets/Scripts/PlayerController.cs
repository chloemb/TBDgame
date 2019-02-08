using System;
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

    public bool IsGrounded;
    public bool StuckOnWall;
    public bool IgnoreHorizontal;

    public string[] PlayerAxes;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var horizontal = Input.GetAxis(PlayerAxes[0]);
        var jump = Input.GetAxis(PlayerAxes[1]);

        if (IsGrounded)
        {
            if (jump > 0)
            {
                Vector2 errorVector2 = new Vector2(Speed * horizontal, JumpHeight) - new Vector2(_rb.velocity.x, 0);
                // _rb.velocity = new Vector2(0, 0);
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                IsGrounded = false;
            }
        }

        if (StuckOnWall)
        {
            if (jump > 0)
            {
                Vector2 errorVector2 = new Vector2(0, JumpHeight);
                _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                
                IgnoreHorizontal = true;
                StuckOnWall = false;
            }
        }

        // _rb.velocity = new Vector2(Speed * horizontal, _rb.velocity.y);
        if (!IgnoreHorizontal)
        {
            _rb.AddForce(new Vector2(horizontal * Speed, 0) - new Vector2(_rb.velocity.x, 0), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision detected " + col.gameObject.tag);
        if (col.gameObject.tag == "Floors")
        {
            IsGrounded = true;
        }

        if (col.gameObject.tag == "Walls")
        {
            StuckOnWall = true;
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
            IgnoreHorizontal = false;
            StuckOnWall = false;
        }
    }
}