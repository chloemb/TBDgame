using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public int InitialHealth;
    public int Health;
    public float GracePeriodLength;
    public float ExtendedGracePeriodLength;
    
    // public TextMeshProUGUI HealthDisplay;
    
    public bool InGracePeriod, CurrentlyInvincible;

    public Transform RespawnPoint;
    private Rigidbody2D _rb;
    private PlayerController _pc;

    [HideInInspector] public float LastHitIFrames;
    private IEnumerator KBIFrames;
    public Transform PlayerHolder;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Set up proper name and get important componenets
        gameObject.name = gameObject.name.Replace(" Variant(Clone)", "");
        PlayerHolder = GameObject.Find("Players").transform;
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();

        _rb.velocity = new Vector2(0f, 0f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.Find("Aura").GetComponent<Collider2D>().enabled = false;
        
        // Reset health
        Health = InitialHealth;
        // UpdateHealthDisplay();
        
        // Reset controls
        _pc.SetUpControls();
        
        // Start and end grace period
        InGracePeriod = true;
        gameObject.layer = 5;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        IEnumerator GraceIFrames = gameObject.GetComponent<AnimationController>().IFrameAnim(GracePeriodLength
                                                                                             + ExtendedGracePeriodLength
                                                                                             - .5f);
        StartCoroutine(GraceIFrames);
        MakeInvincible(GracePeriodLength + ExtendedGracePeriodLength);
        Invoke("EndGracePeriod", GracePeriodLength);
    }

    private void FixedUpdate()
    {
        if (Health <= 0 && name == "Player 1")
        {
            SendMessageUpwards("Player1Killed");
            // Instantiate(PlayerPrefab, RespawnPoint.position, Quaternion.identity, PlayerHolder);
            Destroy(gameObject);
        }
        else if (Health <= 0 && name == "Player 2")
        {
            SendMessageUpwards("Player2Killed");
            // Instantiate(PlayerPrefab, RespawnPoint.position, Quaternion.identity, PlayerHolder);
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int dmg)
    {
        Health -= dmg;
        GetComponent<Reactor>().Floating = false;
        if (dmg >= 0)
        {
            KBIFrames = gameObject.GetComponent<AnimationController>().IFrameAnim(LastHitIFrames);
            StartCoroutine(KBIFrames);
        }

        if (Health > InitialHealth) Health = InitialHealth;
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
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
    
    private void EndGracePeriod()
    {
        gameObject.layer = 8;
        InGracePeriod = false;
        _rb.gravityScale = _pc.GravityScale;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.transform.Find("Aura").GetComponent<Collider2D>().enabled = true;
        _pc.ControlDisabled = false;
    }
}
