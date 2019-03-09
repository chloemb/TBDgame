using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncletAnimController : MonoBehaviour
{
    private Animator bouncletAnim;
    private PointEffector2D _pe;

    void Start()
    {
        bouncletAnim = GetComponent<Animator>();
        _pe = GetComponent<PointEffector2D>();
        Explode();
    }

    void Explode()
    {
        bouncletAnim.SetTrigger("Explode");
        Invoke("DisablePE", .1f);
    }

    void DisablePE()
    {
        _pe.enabled = false;
    }
}
