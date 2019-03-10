using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrap : MonoBehaviour
{
    public GameObject Trap;
    public int Uses;
    public float Cooldown;
    public float SetTime;
    public bool SettingTrap;
    private bool _onCooldown;
    
    public void Set ()
    {
        if (!_onCooldown && Uses > 0)
        {
            Uses--;
            MakeTrap();
        }
    }

    private void MakeTrap()
    {
        var distanceFromGround = new Vector3(0, GetComponent<Renderer>().bounds.size.y / 2.5f, 0);
        var trapInstance = Instantiate(Trap, gameObject.transform.position - distanceFromGround, transform.rotation);
        var origin = trapInstance.gameObject.GetComponent<Trap>().playerOrigin = gameObject;
        var trapParticleSystem = trapInstance.gameObject.GetComponent<ParticleSystem>().main;
        if (origin.name == "Player 1")
            trapParticleSystem.startColor = new Color(93f/255f, 96f/255f, 244f/255f);
        else if (origin.name == "Player 2")
            trapParticleSystem.startColor = new Color(255, 165, 0);

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
