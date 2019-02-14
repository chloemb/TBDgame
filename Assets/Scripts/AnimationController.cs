using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _pc;
    private Rigidbody2D _rb;
    private Color SpriteColor;

    private const int STATE_IDLE = 0;
    private const int STATE_RUNNING = 1;
    private const int STATE_FLYING = 2;

    private bool FacingLeft = true;
    public int AnimationState = STATE_IDLE;
    private bool GrayedOut;

    public GameObject DashTrail;
    private GameObject CurTrail;
    private bool TrailOn;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _pc = gameObject.GetComponent<PlayerController>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        SpriteColor = GetComponent<SpriteRenderer>().color;
    }

    void FixedUpdate()
    {
        // flipping
        if (_rb.velocity.x > 0 && FacingLeft)
        {
            transform.Rotate(0, 180, 0);
            gameObject.GetComponent<HealthManager>().HealthDisplay.transform.Rotate(0, 180, 0);
            FacingLeft = false;
        }
        else if (_rb.velocity.x < 0 && !FacingLeft)
        {
            transform.Rotate(0, 180, 0);
            gameObject.GetComponent<HealthManager>().HealthDisplay.transform.Rotate(0, 180, 0);
            FacingLeft = true;
        }

        if (_pc.IsGrounded)
        {
            if (_rb.velocity.x == 0)
            {
                if (_pc.TouchWallToLeft || _pc.TouchWallToRight)
                {
                    changeState(2);
                    AnimationState = STATE_FLYING;
                }
                else
                {
                    changeState(0);
                    AnimationState = STATE_IDLE;
                }
            }
            else
            {
                changeState(1);
                AnimationState = STATE_RUNNING;
            }
        }
        else
        {
            changeState(2);
            AnimationState = STATE_FLYING;
        }
        
        // Manages dash cooldown indicator
        if (GrayedOut != _pc.DashOnCooldown)
        {
            GrayedOut = _pc.DashOnCooldown;
            if (GrayedOut)
            {
                IEnumerator ColorLerper = DashRefresh(_pc.DashCooldown);
                StartCoroutine(ColorLerper);
            }    
        }
        
        // Dash trail
        if (_pc.CurrentlyDashing && !TrailOn)
        {
            CurTrail = Instantiate(DashTrail, _rb.transform);
            CurTrail.GetComponent<TrailRenderer>().time = _pc.DashLength;
            Invoke("DestroyTrail", _pc.DashLength);
            TrailOn = true;
        }
    }

    private IEnumerator DashRefresh(float cooldown)
    {
        float cdpassed = 0;
        while (cdpassed <= cooldown)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.gray, Color.white, cdpassed/cooldown);
            cdpassed += Time.deltaTime;
            yield return null;
        }
    }

    private void DestroyTrail()
    {
        TrailOn = false;
        Destroy(CurTrail);
    }

    private void changeState(int n)
    {
        if (AnimationState == n)
            return;

        switch (n)
        {
            case STATE_IDLE:
                _anim.SetInteger("state", STATE_IDLE);
                break;

            case STATE_RUNNING:
                _anim.SetInteger("state", STATE_RUNNING);
                break;

            case STATE_FLYING:
                _anim.SetInteger("state", STATE_FLYING);
                break;
        }
    }
}