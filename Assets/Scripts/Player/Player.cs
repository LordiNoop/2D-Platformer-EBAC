using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public HealthBase healthBase;

    [Header("Setup")]
    public SOPlayerSetup soPlayerSetup;

    //public Animator animator;

    private bool _isRunning = false;
    private bool _isFacingRight = true;

    private Animator _currentPlayer;

    [Header("Jump Collision Check")]
    public Collider2D coll2D;
    public float distToGround;
    public float spaceToGround = .1f;
    public ParticleSystem jumpVFX;

    [Header("Sounds")]
    public AudioSource audioSourceJumpSFX;

    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);

        if (coll2D != null)
        {
            distToGround = coll2D.bounds.extents.y;
        }
    }

    public bool IsGrounded()
    {
        //Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }

    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    private void Update()
    {
        IsGrounded();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        _isRunning = Input.GetKey(KeyCode.LeftControl);

        if (_isRunning && IsGrounded())
        {
            _currentPlayer.speed = 1.2f;
        }
        else if (!_isRunning && IsGrounded())
        {
            _currentPlayer.speed = 1;
        }
        else if (!IsGrounded())
        {
            _currentPlayer.speed = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            myRigidbody.velocity = new Vector2(_isRunning ? -soPlayerSetup.speedRun : -soPlayerSetup.speed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, soPlayerSetup.playerSwipeDuration);
                _isFacingRight = false;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            myRigidbody.velocity = new Vector2(_isRunning ? soPlayerSetup.speedRun : soPlayerSetup.speed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, soPlayerSetup.playerSwipeDuration);
                _isFacingRight = true;
            }
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity -= soPlayerSetup.friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity += soPlayerSetup.friction;
        }

    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            myRigidbody.velocity = Vector2.up * soPlayerSetup.forceJump;
            myRigidbody.transform.localScale = Vector3.one;

            if (_isFacingRight)
            {
                soPlayerSetup.jumpScaleX = (Mathf.Sign(soPlayerSetup.jumpScaleX) == 1) ? soPlayerSetup.jumpScaleX : -soPlayerSetup.jumpScaleX;
                myRigidbody.transform.localScale = Vector3.one;
            }
            else
            {
                soPlayerSetup.jumpScaleX = (Mathf.Sign(soPlayerSetup.jumpScaleX) == -1) ? soPlayerSetup.jumpScaleX : -soPlayerSetup.jumpScaleX;
                myRigidbody.transform.localScale = new Vector3(-1, 1, 1);
            }

                DOTween.Kill(myRigidbody.transform);

            HandleScaleJump();
            PlayJumpVFX();
            audioSourceJumpSFX.Play();
        }
    }

    private void PlayJumpVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position + new Vector3(0f, 2.5f, 0f));
        //if (jumpVFX != null) jumpVFX.Play();
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(soPlayerSetup.jumpScaleY, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        myRigidbody.transform.DOScaleX(soPlayerSetup.jumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
