using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndicatorAnimationController : MonoBehaviour
{
    private int BubbleStage, UnstBulStage, BouncletStage;
    private Animator _bubbleanim, _bigbubbleanim, _unstbulanim, _bouncletanim;
    private SpriteRenderer _shield, _aimindic;

    private FireWeapon _fw;
    private Reactor _rea;
    private HealthManager _hm;
    private PlayerController _pc;

    private bool ShowingBubbles, ShowingBigBubble, ShowingUnstBul, ShowingShield, ShowingBounclet;
    
    // Start is called before the first frame update
    void Start()
    {
        _fw = GetComponentInParent<FireWeapon>();
        _rea = GetComponentInParent<Reactor>();
        _hm = GetComponentInParent<HealthManager>();
        _pc = GetComponentInParent<PlayerController>();

        _shield = gameObject.transform.Find("Shield").GetComponent<SpriteRenderer>();
        _aimindic = gameObject.transform.Find("AimIndicator").GetComponent<SpriteRenderer>();
        _bubbleanim = gameObject.transform.Find("Bubbles").GetComponent<Animator>();
        _bigbubbleanim = gameObject.transform.Find("BigBubble").GetComponent<Animator>();
        _unstbulanim = gameObject.transform.Find("UnstBulIndicator").GetComponent<Animator>();
        _bouncletanim = gameObject.transform.Find("BouncletIndicator").GetComponent<Animator>();

        _shield.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string Current = _fw.CurrentOffhandWeapon;
        int Remaining = _fw.RemainingUses;
        bool floating = _rea.Floating;
        int CurHealth = _hm.Health;
        bool ShowAimIndic = _pc.ShowAimIndicator;
        
        if (Current == "Bubble Gun")
        {
            if (ShowingUnstBul)
            {
                _unstbulanim.ResetTrigger("Reset");
                _unstbulanim.SetTrigger("End");
                ShowingUnstBul = false;
            }
            
            if (ShowingBounclet)
            {
                _bouncletanim.ResetTrigger("Reset");
                _bouncletanim.SetTrigger("End");
                ShowingBounclet = false;
            }
            
            _bubbleanim.ResetTrigger("End");
            
            // When a Bubblegun Powerup is picked up
            if (!ShowingBubbles)
            {
                ShowingBubbles = true;
                _bubbleanim.SetTrigger("Reset");
                BubbleStage = Remaining;
            }
            
            // If a bubblet has been shot
            else if (BubbleStage > Remaining)
            {
                _bubbleanim.SetTrigger("Decrease");
                BubbleStage = Remaining;
            }

            // If another Bubblegun Powerup is picked up
            else if (BubbleStage < Remaining)
            {
                _bubbleanim.SetTrigger("Reset");
                BubbleStage = Remaining;
            }
        }
        else if (Current == "Gunstoppable")
        {
            if (ShowingBubbles)
            {
                _bubbleanim.ResetTrigger("Reset");
                _bubbleanim.SetTrigger("End");
                ShowingBubbles = false;
            }

            if (ShowingBounclet)
            {
                _bouncletanim.ResetTrigger("Reset");
                _bouncletanim.SetTrigger("End");
                ShowingBounclet = false;
            }
            
            _unstbulanim.ResetTrigger("End");
            
            // When a Gunstoppable Powerup is picked up
            if (!ShowingUnstBul)
            {
                ShowingUnstBul = true;
                _unstbulanim.SetTrigger("Reset");
                UnstBulStage = Remaining;
            }
            
            // If an unstoppabullet has been shot
            else if (UnstBulStage > Remaining)
            {
                _unstbulanim.SetTrigger("Decrease");
                UnstBulStage = Remaining;
            }

            // If another Gunstoppable Powerup is picked up
            else if (UnstBulStage < Remaining)
            {
                _unstbulanim.SetTrigger("Reset");
                UnstBulStage = Remaining;
            }
        } else if (Current == "Bouncing Bomb")
        {
            if (ShowingBubbles)
            {
                _bubbleanim.ResetTrigger("Reset");
                _bubbleanim.SetTrigger("End");
                ShowingBubbles = false;
            }
            
            if (ShowingUnstBul)
            {
                _unstbulanim.ResetTrigger("Reset");
                _unstbulanim.SetTrigger("End");
                ShowingUnstBul = false;
            }
            
            _bouncletanim.ResetTrigger("End");
            
            // When a Gunstoppable Powerup is picked up
            if (!ShowingBounclet)
            {
                ShowingBounclet = true;
                _bouncletanim.SetTrigger("Reset");
                BouncletStage = Remaining;
            }
            
            // If an unstoppabullet has been shot
            else if (BouncletStage > Remaining)
            {
                _bouncletanim.SetTrigger("Decrease");
                BouncletStage = Remaining;
            }

            // If another Gunstoppable Powerup is picked up
            else if (BouncletStage < Remaining)
            {
                _bouncletanim.SetTrigger("Reset");
                BouncletStage = Remaining;
            }
        }
        else
        {
            if (ShowingBubbles)
            {
                _bubbleanim.ResetTrigger("Reset");
                _bubbleanim.SetTrigger("End");
                ShowingBubbles = false;
            }
            
            if (ShowingUnstBul)
            {
                _unstbulanim.ResetTrigger("Reset");
                _unstbulanim.SetTrigger("End");
                ShowingUnstBul = false;
            }
        }

        if (ShowingBigBubble != floating)
        {
            ShowingBigBubble = floating;
            if (ShowingBigBubble)
            {
                _bigbubbleanim.SetTrigger("FloatHit");
            }
            else
            {
                _bigbubbleanim.SetTrigger("FloatEnd");
            }
        }

        if (CurHealth == 2 && !ShowingShield)
        {
            _shield.enabled = true;
            ShowingShield = true;
        } else if (CurHealth == 1 && ShowingShield)
        {
            _shield.enabled = false;
            ShowingShield = false;
        }

        if (ShowAimIndic)
        {
            _aimindic.enabled = true;
            _aimindic.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.down, _pc.RSA));
            _aimindic.transform.localPosition = new Vector3(_pc.RSA.x, _pc.RSA.y, 0) * _shield.transform.localScale.x / 2f;
        }
        else
        {
            _aimindic.enabled = false;
        }
    }
}
