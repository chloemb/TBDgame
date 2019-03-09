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
            //Invoke("MakeTrap", SetTime); Need animation so this doesn't feel like lag
            MakeTrap();
        }
    }

    private void MakeTrap()
    {
        var distanceFromGround = new Vector3(0, GetComponent<Renderer>().bounds.size.y / 2.5f, 0);
        var trapInstance = Instantiate(Trap, gameObject.transform.position - distanceFromGround, transform.rotation);
        trapInstance.gameObject.GetComponent<Trap>().playerOrigin = gameObject;
        
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
