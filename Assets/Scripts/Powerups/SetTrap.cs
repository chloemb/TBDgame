using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrap : MonoBehaviour
{
    public GameObject Trap;
    private GameObject _trap;
    private GameObject _trap2;
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
            MakeTrap("P1");
        }
        else
        {
            MakeTrap("P1");
        }
        
        if (_trap2)
        {
            Destroy(_trap2);
            MakeTrap("P2");
        }
        else
        {
            MakeTrap("P2");
        }
    }

    private void MakeTrap(string player)
    {
        var distanceFromGround = new Vector3(0, GetComponent<Renderer>().bounds.size.y / 2.5f, 0);
        String origin = "";
        ParticleSystem.MainModule trapParticleSystem;
        
        switch (player)
        {
            case "P1":
                var existingTrapsP1 = FindObjectsOfType<Trap>();
                foreach (Trap t in existingTrapsP1)
                    if (t.playerOrigin == "Player 1")
                        Destroy(t.gameObject);
                _trap = Instantiate(Trap, gameObject.transform.position - distanceFromGround, transform.rotation);
                origin = _trap.gameObject.GetComponent<Trap>().playerOrigin = gameObject.name;
                trapParticleSystem = _trap.gameObject.GetComponent<ParticleSystem>().main;
                break;
            case "P2":
                var existingTrapsP2 = FindObjectsOfType<Trap>();
                foreach (Trap t in existingTrapsP2)
                    if (t.playerOrigin == "Player 2")
                        Destroy(t.gameObject);
                _trap2 = Instantiate(Trap, gameObject.transform.position - distanceFromGround, transform.rotation);
                origin = _trap2.gameObject.GetComponent<Trap>().playerOrigin = gameObject.name;
                trapParticleSystem = _trap2.gameObject.GetComponent<ParticleSystem>().main;
                break;
        }
        
        switch (origin)
        {
            case "Player 1":
                trapParticleSystem.startColor = new Color(93f / 255f, 96f / 255f, 244f / 255f);
                break;
            case "Player 2":
                trapParticleSystem.startColor = new Color(255f / 255f, 144f / 255f, 0);
                break;
        }

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
