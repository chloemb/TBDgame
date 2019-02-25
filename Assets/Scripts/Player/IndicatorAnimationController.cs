using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class IndicatorAnimationController : MonoBehaviour
{
    private int BubbleStage, UnstBulStage;
    private Animator _bubbleanim, _bigbubbleanim, _unstbulanim;

    private FireWeapon _fw;
    private Reactor _rea;

    private bool ShowingBubbles, ShowingBigBubble, ShowingUnstBul;
    
    // Start is called before the first frame update
    void Start()
    {
        _fw = GetComponentInParent<FireWeapon>();
        _rea = GetComponentInParent<Reactor>();
        _bubbleanim = gameObject.transform.Find("Bubbles").GetComponent<Animator>();
        _bigbubbleanim = gameObject.transform.Find("BigBubble").GetComponent<Animator>();
        _unstbulanim = gameObject.transform.Find("UnstBulIndicator").GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string Current = _fw.CurrentOffhandWeapon;
        int Remaining = _fw.RemainingUses;
        bool floating = _rea.Floating;
        
        if (Current == "Bubble Gun")
        {
            if (ShowingUnstBul)
            {
                _unstbulanim.ResetTrigger("Reset");
                _unstbulanim.SetTrigger("End");
                ShowingUnstBul = false;
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
    }
}
