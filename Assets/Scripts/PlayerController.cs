using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D _rb;
    public float Speed;
    public float JumpHeight;
    private bool IsGrounded;

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
        
        if (jump > 0 && IsGrounded)
        { 
            Vector2 movement = new Vector2(Speed * horizontal, JumpHeight);
            // _rb.velocity = new Vector2(0, 0);
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
