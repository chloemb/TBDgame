using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _pc;
    private Rigidbody2D _rb;
    public SpriteRenderer _sr;

    private const int STATE_IDLE = 0;
    private const int STATE_RUNNING = 1;
    private const int STATE_FLYING = 2;
    private const int STATE_HANGING = 3;

    public bool FacingLeft = true;
    public int AnimationState = STATE_IDLE;
    private bool GrayedOut, TrailOn, CurrentlySliding;
    public bool InKBIFrames;

    public GameObject DashTrail;
    private GameObject CurTrail;

    public Material DefaultMaterial;
    public Material FlashWhiteMaterial;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _pc = gameObject.GetComponent<PlayerController>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(_sr.gameObject);
        DefaultMaterial = GetComponent<SpriteRenderer>().material;
        _pc.KnockingBack = false;
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<HealthManager>().InGracePeriod) changeState(0);
        else
        {
            // flipping
            if (!_pc.KnockingBack && _rb.velocity.x > 0 && FacingLeft)
            {
                Flip180();
                FacingLeft = false;
            }
            else if (!_pc.KnockingBack && _rb.velocity.x < 0 && !FacingLeft)
            {
                Flip180();
                FacingLeft = true;
            }

            if (_pc.IsGrounded)
            {
                if (_pc.KnockingBack) changeState(0);
                else if (_rb.velocity.x == 0)
                {
                    if (_pc.TouchWallToLeft || _pc.TouchWallToRight) changeState(STATE_HANGING);
                    else changeState(STATE_IDLE);
                }
                else changeState(STATE_RUNNING);
            }
            else
            {
                if (_rb.velocity.y.Equals(0f) && (_pc.TouchWallToLeft || _pc.TouchWallToRight))
                {
                    changeState(STATE_HANGING);

                    if (_pc.TouchWallToRight && FacingLeft)
                    {
                        Flip180();
                        FacingLeft = false;
                    }
                    else if (_pc.TouchWallToLeft && !FacingLeft)
                    {
                        Flip180();
                        FacingLeft = true;
                    }
                }
                else if (!_pc.TouchWallToRight && !_pc.TouchWallToLeft) changeState(STATE_FLYING);
            }

            // Manages dash cooldown indicator
            if (_pc.DashOnCooldown != GrayedOut)
            {
                GrayedOut = !GrayedOut;
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
    }

    private IEnumerator DashRefresh(float cooldown)
    {
        float cdpassed = 0;
        while (cdpassed <= cooldown-.3f)
        {
            _sr.color = Color.Lerp(Color.gray, Color.white, cdpassed / (cooldown-.3f));
            cdpassed += Time.deltaTime;
            yield return null;
        }
        
        Color halftransparent = Color.Lerp(Color.clear, Color.white, .75f);
        
        _sr.material = FlashWhiteMaterial;
        _sr.color = halftransparent;
        yield return new WaitForSeconds(.1f);
        _sr.material = DefaultMaterial;
        _sr.color = Color.white;
        yield return new WaitForSeconds(.1f);
        _sr.material = FlashWhiteMaterial;
        _sr.color = halftransparent;
        yield return new WaitForSeconds(.1f);
        _sr.material = DefaultMaterial;
        _sr.color = Color.white;
    }

    public IEnumerator IFrameAnim(float ifr)
    {
        InKBIFrames = !gameObject.GetComponent<HealthManager>().InGracePeriod;
        
        float ifpassed = 0;
        float timeincre = .15f;
        while (ifpassed <= ifr)
        {
            _sr.color = _sr.color == Color.white ? Color.Lerp(Color.clear, Color.white, .5f) : Color.white;
            ifpassed += timeincre;
            yield return new WaitForSeconds(timeincre);
        }

        _sr.color = Color.white;
        InKBIFrames = false;
    }

    private void DestroyTrail()
    {
        Destroy(CurTrail);
        TrailOn = false;
    }

    private void Flip180()
    {
        transform.Rotate(0, 180, 0);
        gameObject.GetComponent<HealthManager>().HealthDisplay.transform.Rotate(0, 180, 0);
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

            case STATE_HANGING:
                _anim.SetInteger("state", STATE_HANGING);
                break;
            
            default:
                _anim.SetInteger("state", STATE_IDLE);
                break;
        }

        AnimationState = n;
    }
}