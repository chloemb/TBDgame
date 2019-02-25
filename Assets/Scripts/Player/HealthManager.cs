using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public int InitialHealth;
    public int Health;
    public float GracePeriodLength;
    public TextMeshProUGUI HealthDisplay;
    
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
        
        // Move player to Respawn point
//        switch (gameObject.name)
//        {
//            case "Player 1":
//                RespawnPoint = GameObject.Find("P1Respawn").transform;
//                break;
//            case "Player 2":
//                RespawnPoint = GameObject.Find("P2Respawn").transform;
//                break;
//            default:
//                RespawnPoint = GameObject.Find("P1Respawn").transform;
//                break;
//        }
//        gameObject.transform.position = RespawnPoint.position;
        _rb.velocity = new Vector2(0f, 0f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        // Reset health
        Health = InitialHealth;
        UpdateHealthDisplay();
        
        // Reset controls
        _pc.SetUpControls();
        //_pc.TouchWallToLeft = _pc.TouchWallToRight = false;
        //_pc.RefreshCooldown();
        
        // Reset material
        //GetComponent<SpriteRenderer>().material = GetComponent<AnimationController>().DefaultMaterial;
        
        // Start and end grace period
        InGracePeriod = true;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        IEnumerator GraceIFrames = gameObject.GetComponent<AnimationController>().IFrameAnim(GracePeriodLength-.5f);
        StartCoroutine(GraceIFrames);
        MakeInvincible(GracePeriodLength);
        Invoke("EndGracePeriod", GracePeriodLength);
    }

    private void FixedUpdate()
    {
        if (Health == 0)
        {
            SendMessageUpwards("Killed");
            // Instantiate(PlayerPrefab, RespawnPoint.position, Quaternion.identity, PlayerHolder);
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int dmg)
    {
        Health -= dmg;
        if (dmg >= 0)
        {
            KBIFrames = gameObject.GetComponent<AnimationController>().IFrameAnim(LastHitIFrames);
            StartCoroutine(KBIFrames);
        }

        if (Health > InitialHealth) Health = InitialHealth;
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
    
    private void EndGracePeriod()
    {
        InGracePeriod = false;
        _rb.gravityScale = _pc.GravityScale;
        gameObject.GetComponent<Collider2D>().enabled = true;
        _pc.ControlDisabled = false;
    }
}
