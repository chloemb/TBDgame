using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int InitialHealth;
    public int Health;
    public TextMeshProUGUI HealthDisplay;
    public float GracePeriodLength;
    
    [HideInInspector] public bool InGracePeriod, CurrentlyInvincible;

    private Transform RespawnPoint;
    private Rigidbody2D _rb;
    private PlayerController _pc;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = InitialHealth;
        UpdateHealthDisplay();
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();
        
        switch (gameObject.name)
        {
            case "Player 1":
                RespawnPoint = GameObject.Find("P1Respawn").transform;
                break;
            case "Player 2":
                RespawnPoint = GameObject.Find("P2Respawn").transform;
                break;
            default:
                RespawnPoint = GameObject.Find("P1Respawn").transform;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Health == 0)
        {
            Respawn();
        }
    }

    public void DamagePlayer(int dmg)
    {
        Health -= dmg;
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        HealthDisplay.text = Health.ToString();
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
        UpdateHealthDisplay();
    }

    public void MakeInvincible(float seconds)
    {
        CurrentlyInvincible = true;
        Invoke("MakeVincible", seconds);
    }

    public void MakeVincible()
    {
        CurrentlyInvincible = false;
    }
    
    private void Respawn()
    {
        InGracePeriod = true;
        gameObject.transform.position = RespawnPoint.position;
        ResetHealth();
        _rb.velocity = new Vector2(0f, 0f);
        _rb.gravityScale = 0;
        _pc.RefreshCooldown();
        IEnumerator GraceIFrames = gameObject.GetComponent<AnimationController>().IFrameAnim(GracePeriodLength-.5f);
        StartCoroutine(GraceIFrames);
        MakeInvincible(GracePeriodLength);
        
        Invoke("EndGracePeriod", GracePeriodLength);
    }

    private void EndGracePeriod()
    {
        InGracePeriod = false;
        _rb.gravityScale = _pc.GravityScale;
    }
}
