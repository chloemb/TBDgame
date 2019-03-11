using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrap : MonoBehaviour
{
    public GameObject Trap;
    private static GameObject _trap;
    public float Cooldown;
    public float SetTime;
    public bool SettingTrap;
    private bool _onCooldown;
    
    public void Set ()
    {
        if (_onCooldown)
            return;
        if (_trap)
        {
            Destroy(_trap);
            MakeTrap();
        }
        else
        {
            MakeTrap();
        }
    }

    private void MakeTrap()
    {
        var distanceFromGround = new Vector3(0, GetComponent<Renderer>().bounds.size.y / 2.5f, 0);
        _trap = Instantiate(Trap, gameObject.transform.position - distanceFromGround, transform.rotation);
        var origin = _trap.gameObject.GetComponent<Trap>().playerOrigin = gameObject;
        var trapParticleSystem = _trap.gameObject.GetComponent<ParticleSystem>().main;
        if (origin.name == "Player 1")
            trapParticleSystem.startColor = new Color(93f / 255f, 96f / 255f, 244f / 255f);
        else if (origin.name == "Player 2")
            trapParticleSystem.startColor = new Color(255f / 255f, 144f / 255f, 0);

        SettingTrap = true;
        _onCooldown = true;
        Invoke("NoLongerSetting", SetTime);
        Invoke("RefreshTrapCooldown", Cooldown);
    }
    
    private void NoLongerSetting()
    {
        SettingTrap = false;
    }
    
    public void RefreshTrapCooldown()
    {
        _onCooldown = false;
    }
}
