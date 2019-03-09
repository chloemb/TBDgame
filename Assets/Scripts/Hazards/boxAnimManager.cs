using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxAnimManager : MonoBehaviour
{
    private Animator _boxAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _boxAnimator = GetComponent<Animator>();
        Kaboom();
    }
    
    //Set explosion
    void Kaboom()
    {
        Destroy(gameObject, _boxAnimator.GetCurrentAnimatorStateInfo(0).length -.5f);
        _boxAnimator.SetTrigger("Explode");
    }
}
