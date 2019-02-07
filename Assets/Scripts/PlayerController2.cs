using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float Speed;
    public float JumpHeight;
    private bool IsGrounded;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        IsGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        var horizontal = Input.GetAxis("P2Horizontal");
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
