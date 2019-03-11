using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Assets.Scripts;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    public bool Paused;

    // Customizable in inspector
    public float GravityScale;
    public float Speed;
    public float JumpHeight;
    public Vector2 WallJumpStrength;
    public float WallJumpLength;
    public float ClingTime;
    public Vector2 DashStrength;
    public float DashLength;
    public float DashCooldown;
    public PhysicsMaterial2D NoFriction;


    // player axes array.
    // Currently: [LHorizontal, LVertical, Jump, Dash, Shoot, Offhand, RHorizontal, RVertical, SetTrap, Pause]
    public string[] PlayerAxes;
    private Collider2D _col;
    private FireWeapon _fw;
    private SetTrap _st;
    public Transform _pm;
    private GameObject _controlsScreen;

    // variables for managing movement and walls
    public bool IsGrounded;
    [HideInInspector] public Vector2 PrevVelocity; // The most recent non-zero velocity

    private Vector2 ClingPosition, LastDashed, PreDashVel, LastFired;
    public Vector2 RSA;
    private PlatformType _platform;

    // various info about object
    // [HideInInspector]
    public bool
        UsedWallJump,
        WallJumping,
        UsedDash,
        CurrentlyDashing,
        DashOnCooldown,
        FacingRight,
        ControlDisabled,
        TouchWallToRight,
        TouchWallToLeft,
        ShowAimIndicator,
        Pausable;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _fw = GetComponent<FireWeapon>();
        _st = GetComponent<SetTrap>();
        _pm = GameObject.Find("Menu").transform.Find("Pause");
        _controlsScreen = GameObject.Find("Controller Display");
    }

    private void Start()
    {
        if (GlobalControl.Instance.seenControls)
        {
            Pausable = true;
        }
    }

    public void SetUpControls()
    {
        _platform = Platform.GetPlatform();

        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.gravityScale = GravityScale;

        switch (gameObject.name + " " + _platform.ToString())
        {
            case "Player 1 Mac":
                PlayerAxes[0] = "P1LHorizontal";
                PlayerAxes[1] = "P1LVertical";
                PlayerAxes[2] = "P1Jump";
                PlayerAxes[3] = "P1Dash";
                PlayerAxes[4] = "P1Shoot";
                PlayerAxes[5] = "P1Offhand";
                PlayerAxes[6] = "P1RHorizontal";
                PlayerAxes[7] = "P1RVertical";
                PlayerAxes[8] = "P1Trap";
                PlayerAxes[9] = "P1Pause";
                break;
            case "Player 2 Mac":
                PlayerAxes[0] = "P2LHorizontal";
                PlayerAxes[1] = "P2LVertical";
                PlayerAxes[2] = "P2Jump";
                PlayerAxes[3] = "P2Dash";
                PlayerAxes[4] = "P2Shoot";
                PlayerAxes[5] = "P2Offhand";
                PlayerAxes[6] = "P2RHorizontal";
                PlayerAxes[7] = "P2RVertical";
                PlayerAxes[8] = "P2Trap";
                PlayerAxes[9] = "P2Pause";
                break;
            case "Player 1 Windows":
                PlayerAxes[0] = "P1LHorizontal_Windows";
                PlayerAxes[1] = "P1LVertical_Windows";
                PlayerAxes[2] = "P1Jump_Windows";
                PlayerAxes[3] = "P1Dash_Windows";
                PlayerAxes[4] = "P1Shoot_Windows";
                PlayerAxes[5] = "P1Offhand_Windows";
                PlayerAxes[6] = "P1RHorizontal_Windows";
                PlayerAxes[7] = "P1RVertical_Windows";
                PlayerAxes[8] = "P1Trap_Windows";
                PlayerAxes[9] = "P1Pause_Windows";
                break;
            case "Player 2 Windows":
                PlayerAxes[0] = "P2LHorizontal_Windows";
                PlayerAxes[1] = "P2LVertical_Windows";
                PlayerAxes[2] = "P2Jump_Windows";
                PlayerAxes[3] = "P2Dash_Windows";
                PlayerAxes[4] = "P2Shoot_Windows";
                PlayerAxes[5] = "P2Offhand_Windows";
                PlayerAxes[6] = "P2RHorizontal_Windows";
                PlayerAxes[7] = "P2RVertical_Windows";
                PlayerAxes[8] = "P2Trap_Windows";
                PlayerAxes[9] = "P2Pause_Windows";
                break;
        }

        IsGrounded = false;
        PrevVelocity = new Vector2(1, 1);
    }

    private void Update()
    {
        if (_controlsScreen.transform.GetChild(0).gameObject.activeSelf)
        {
            if (Input.GetButtonDown(PlayerAxes[9]) || Input.GetButtonDown(PlayerAxes[2]))
            {
                _controlsScreen.transform.GetChild(0).gameObject.SetActive(false);
                Time.timeScale = 1f;
                GlobalControl.Instance.GetComponent<AudioSource>().volume = 1f;
                GlobalControl.Instance.GetComponent<AudioSource>().Play();
                Invoke("CanPause", .05f);
                if (gameObject.name == "Player 1")
                    GameObject.Find("Player 2").GetComponent<PlayerController>().Invoke("CanPause", 0.05f);
                else if (gameObject.name == "Player 2")
                    GameObject.Find("Player 1").GetComponent<PlayerController>().Invoke("CanPause", 0.05f);
            }
        }
        else
        {
            if (Input.GetButtonDown(PlayerAxes[9]) && Pausable)
            {
                if (!Paused)
                {
                    Paused = true;
                    Pausable = false;
                    StartCoroutine(CanPauseRoutine());
                    _pm.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                }
                else
                {
                    Paused = false;
                    _pm.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
        
    }

    void FixedUpdate()
    {
        GroundedRay();
        LeftWallRay();
        RightWallRay();

        if (!gameObject.GetComponent<HealthManager>().InGracePeriod && !Paused)
        {
            // Get Input
            var lhorizontal = Input.GetAxis(PlayerAxes[0]);
            var lvertical = Input.GetAxis(PlayerAxes[1]);
            var jump = Input.GetAxis(PlayerAxes[2]);
            var dash = Input.GetAxis(PlayerAxes[3]);
            var shoot = Input.GetAxis(PlayerAxes[4]);
            var offhand = Input.GetAxis(PlayerAxes[5]);
            var rhorizontal = Input.GetAxis(PlayerAxes[6]);
            var rvertical = Input.GetAxis(PlayerAxes[7]);
            var setTrap = Input.GetAxis(PlayerAxes[8]);

            // Be able to jump off of walls & time amount allowed to cling to wall
            if ((TouchWallToLeft || TouchWallToRight) && !IsGrounded)
            {
                if (jump > 0 && !UsedWallJump)
                {
                    Vector2 errorVector2 = new Vector2(0, WallJumpStrength.y * JumpHeight);
                    _rb.velocity = new Vector2(0, 0);
                    errorVector2 = TouchWallToLeft
                        ? errorVector2 + new Vector2(WallJumpStrength.x * Speed, 0)
                        : errorVector2 - new Vector2(WallJumpStrength.x * Speed, 0);
                    _rb.AddForce(errorVector2, ForceMode2D.Impulse);

                    WallJumping = UsedWallJump = true;
                    DisableControl();
                    Invoke("StopWallJumping", WallJumpLength);
                }

                if (_rb.velocity.magnitude < .001f)
                {
                    ClingPosition = _rb.position;
                    Invoke("WallSlide", ClingTime);
                }
            }

            if (!ControlDisabled)
            {
                // Jump from ground if control isn't disabled
                if (IsGrounded)
                {
                    if (jump > 0)
                    {
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        Vector2 errorVector2 =
                            new Vector2(Speed * lhorizontal, JumpHeight) - new Vector2(_rb.velocity.x, 0);
                        _rb.AddForce(errorVector2, ForceMode2D.Impulse);
                        //IsGrounded = false;
                    }
                }

                // Horizontal movement
                Vector2 forcetoapply = new Vector2(lhorizontal * Speed, 0) - new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(forcetoapply, ForceMode2D.Impulse);

                if (lhorizontal > 0 || lhorizontal == 0 && RSA.x > 0)
                {
                    FacingRight = true;
                }
                else if (lhorizontal < 0 || lhorizontal == 0 && RSA.x < 0)
                {
                    FacingRight = false;
                }

                // Determine left stick angle (LSA) and snap to certain angle; if none, the direction the player is facing. Magnitude is 1.
                Vector2 LSA = new Vector2(lhorizontal, lvertical).normalized;
                if (LSA.magnitude == 0)
                    LSA = FacingRight ? new Vector2(1f, 0f) : new Vector2(-1f, 0f);

                LSA = SnapAngle(LSA).normalized;

                // Same as LSA but for the right stick
                RSA = new Vector2(rhorizontal, rvertical).normalized;
                if (RSA.magnitude == 0)
                    RSA = FacingRight ? new Vector2(1f, 0f) : new Vector2(-1f, 0f);

                //RSA = SnapAngle(RSA).normalized;

                if (FacingRight && RSA.x >= 0 || !FacingRight && RSA.x <= 0)
                {
                    // Show indicator
                    ShowAimIndicator = true;

                    // Fire Weapon
                    if (shoot > 0)
                    {
                        _fw.Fire(RSA);
                        LastFired = RSA;
                    }

                    // Fire offhand weapon
                    if (offhand > 0)
                    {
                        _fw.FireOffhand(RSA);
                        LastFired = RSA;
                    }
                }
                else
                {
                    ShowAimIndicator = false;
                }

                // Set trap
                if (setTrap > 0 && IsGrounded)
                {
                    _st.Set();
                }

                // Dash
                if (dash > 0 && !DashOnCooldown && !UsedDash)
                {
                    if (!IsGrounded && _rb.velocity.x == 0 && _rb.velocity.y > 0)
                    {
                        LSA = new Vector2(0f, 1f);
                    }

                    PreDashVel = _rb.velocity;

                    //Increase strength if the player dashed from stationary
                    if ((LSA.x.Equals(1) || LSA.x.Equals(-1)) && LSA.y.Equals(0))
                        LSA = FacingRight ? new Vector2(1.5f, 0) : new Vector2(-1.5f, 0);

                    Vector2 dashvel = DashStrength * LSA;

                    if (TouchWallToLeft || TouchWallToRight)
                    {
                        if (TouchWallToLeft)
                            dashvel = DashStrength * WallJumpStrength;
                        else if (TouchWallToRight)
                            dashvel = new Vector2(-(DashStrength * WallJumpStrength).x,
                                (DashStrength * WallJumpStrength).y);

                        Invoke("StopDash", DashLength);
                    }
                    else
                    {
                        Invoke("StopDash", DashLength);
                    }

                    _rb.velocity = new Vector2(0, 0);
                    _rb.AddForce(dashvel, ForceMode2D.Impulse);

                    DisableControl();
                    PutDashOnCooldown();
                    Invoke("RefreshCooldown", DashCooldown);

                    CurrentlyDashing = UsedDash = true;
                    LastDashed = dashvel;
                    _rb.gravityScale = 0;
                }
            }

            // Set PrevVelocity
            PrevVelocity = (_rb.velocity != Vector2.zero) ? _rb.velocity : PrevVelocity;
        }
    }

    private void GroundedRay()
    {
        // Get leftmost and rightmost positions on player collider
        Vector2 leftbound = transform.position - new Vector3(_col.bounds.extents.x, 0);
        Vector2 rightbound = transform.position + new Vector3(_col.bounds.extents.x, 0);

        // Raycast down from those positions
        bool GroundLeft = Physics2D.Raycast(leftbound, Vector2.down, _col.bounds.extents.y * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool GroundRight = Physics2D.Raycast(rightbound, Vector2.down, _col.bounds.extents.y * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));

        // If either of those raycasts hit floor, ground player. Else, unground.
        bool TouchingGround = GroundLeft || GroundRight;

        if (IsGrounded != TouchingGround)
        {
            IsGrounded = TouchingGround;
            if (IsGrounded)
            {
                GiveBackControl();
                UsedDash = UsedWallJump = false;
            }
        }
    }

    private void LeftWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool LeftDown = Physics2D.Raycast(downbound, Vector2.left, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool LeftMid = Physics2D.Raycast(transform.position, Vector2.left, +_col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool LeftUp = Physics2D.Raycast(upbound, Vector2.left, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool TouchingLeft = LeftDown || LeftMid || LeftUp;

        if (TouchWallToLeft != TouchingLeft)
        {
            TouchWallToLeft = TouchingLeft;
            if (!TouchWallToLeft)
            {
                UsedWallJump = false;
                if (!WallJumping && !CurrentlyDashing)
                {
                    GiveBackControl();
                }
            }
        }
    }

    private void RightWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool RightDown = Physics2D.Raycast(downbound, Vector2.right, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool RightMid = Physics2D.Raycast(transform.position, Vector2.right, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool RightUp = Physics2D.Raycast(upbound, Vector2.right, _col.bounds.extents.x * 1.1f,
            LayerMask.GetMask("Surfaces", "Moving Platforms"));
        bool TouchingRight = RightDown || RightMid || RightUp;

        if (TouchWallToRight != TouchingRight)
        {
            TouchWallToRight = TouchingRight;
            if (!TouchWallToRight)
            {
                UsedWallJump = false;
                if (!WallJumping && !CurrentlyDashing)
                {
                    GiveBackControl();
                }
            }
        }
    }

    private void StopWallJumping()
    {
        GiveBackControl();
        WallJumping = false;
    }

    private void StopDash()
    {
        GiveBackControl();
        _rb.gravityScale = GravityScale;
        CurrentlyDashing = false;
        if (IsGrounded)
            UsedDash = false;

        _rb.velocity = PreDashVel;
    }

    private void PutDashOnCooldown()
    {
        DashOnCooldown = true;
    }

    public void RefreshCooldown()
    {
        DashOnCooldown = false;
    }

    private void WallSlide()
    {
        if (_rb.position == ClingPosition)
        {
            DisableControl();
        }
    }

    public void DisableControl()
    {
        ControlDisabled = true;
    }

    public void GiveBackControl()
    {
        ControlDisabled = false;
    }

    private Vector2 SnapAngle(Vector2 angle)
    {
        Vector2 returnangle;
        returnangle.x = angle.x >= .3f && angle.x <= .7f ? .5f :
            angle.x < .3f && angle.x > -.3f ? 0f :
            angle.x < -.3f && angle.x >= -.7f ? -.5f :
            angle.x < -.7f ? -1f : 1f;

        returnangle.y = angle.y >= .3f && angle.y <= .7f ? .5f :
            angle.y < .3f && angle.y > -.3f ? 0f :
            angle.y < -.3f && angle.y >= -.7f ? -.5f :
            angle.y < -.7f ? -1f : 1f;

        return angle;
    }

    private void CanPause()
    {
        Pausable = true;
    }

    private IEnumerator CanPauseRoutine()
    {
        yield return new WaitForSecondsRealtime(.05f);
        Pausable = true;
    }
}