﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Note: The reason the animator sometimes clings in the wrong direction is because of how TouchWallToRight and
    // TouchWallToLeft are determined.
    
    private Animator _anim;
    private PlayerController _pc;
    private Rigidbody2D _rb;
    public SpriteRenderer _sr;
    private Reactor _rea;

    private const int STATE_IDLE = 0;
    private const int STATE_RUNNING = 1;
    private const int STATE_FLYING = 2;
    private const int STATE_HANGING = 3;
    private const int STATE_SHOOTING = 4;

    public bool SpriteFacingRight;
    public int AnimationState = STATE_IDLE;
    public bool GrayedOut, TrailOn;

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
        _rea = gameObject.GetComponent<Reactor>();
        
        _rea.KnockingBack = false;
        _sr.material = DefaultMaterial;
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<HealthManager>().InGracePeriod) changeState(0);
        else
        {
            // flipping
            if (!_rea.KnockingBack && _pc.FacingRight != SpriteFacingRight)
            {
                Flip180();
                SpriteFacingRight = _pc.FacingRight;
            }

            if (_rea.Floating) changeState(STATE_FLYING);
            else if (_pc.IsGrounded)
            {
                if (_rb.velocity.magnitude > 0) changeState(STATE_RUNNING);
                else changeState(STATE_IDLE);
            }
            else if (_pc.TouchWallToLeft || _pc.TouchWallToRight)
            {
                if (_pc.TouchWallToLeft == SpriteFacingRight)
                {
                    Flip180();
                    SpriteFacingRight = !SpriteFacingRight;
                }
                changeState(STATE_HANGING);
            }
            else
            {
                changeState(STATE_FLYING);
            }
        }
        
        if (AnimationState == STATE_IDLE && GetComponent<FireWeapon>().CurrentlyFiring) changeState(STATE_SHOOTING);

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


    private IEnumerator DashRefresh(float cooldown)
    {
        float flashlength = .2f;
        float cdpassed = 0;
        while (cdpassed <= cooldown - flashlength)
        {
            _sr.color = Color.Lerp(Color.gray, Color.white, cdpassed / (cooldown - flashlength));
            cdpassed += Time.deltaTime;
            yield return null;
        }

        Color pinkyellow = new Color(255/255f, 255/255f, 50/255f);
        Color halftransparent = Color.Lerp(Color.clear, pinkyellow, .85f);

        float flashingpassed = 0;
        while (flashingpassed < flashlength)
        {
            if (_sr.color == Color.white)
            {
                _sr.material = FlashWhiteMaterial;
                _sr.color = halftransparent;
            }
            else
            {
                _sr.material = DefaultMaterial;
                _sr.color = Color.white;
            }

            flashingpassed += .1f;
            yield return new WaitForSeconds(.1f);
        }

        _sr.material = DefaultMaterial;
        _sr.color = Color.white;
    }

    public IEnumerator IFrameAnim(float ifr)
    {
        float ifpassed = 0;
        float timeincre = .15f;
        while (ifpassed <= ifr)
        {
            _sr.color = _sr.color == Color.white ? Color.Lerp(Color.clear, Color.white, .5f) : Color.white;
            ifpassed += timeincre;
            yield return new WaitForSeconds(timeincre);
        }

        _sr.color = Color.white;
    }

    private void DestroyTrail()
    {
        Destroy(CurTrail);
        TrailOn = false;
    }

    private void Flip180()
    {
        transform.Rotate(0, 180, 0);
        gameObject.GetComponentInChildren<Canvas>().transform.Rotate(0, 180, 0);
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

            case STATE_SHOOTING:
                _anim.SetInteger("state", STATE_SHOOTING);
                break;

            default:
                _anim.SetInteger("state", STATE_IDLE);
                break;
        }

        AnimationState = n;
    }
}
