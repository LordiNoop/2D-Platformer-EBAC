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


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);
    }

    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        _isRunning = Input.GetKey(KeyCode.LeftControl);

        if (_isRunning)
        {
            _currentPlayer.speed = 1.2f;
        }
        else
        {
            _currentPlayer.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            myRigidbody.velocity = new Vector2(_isRunning ? -soPlayerSetup.speedRun : -soPlayerSetup.speed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, soPlayerSetup.playerSwipeDuration);
                _isFacingRight = false;            }
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
        if (Input.GetKeyDown(KeyCode.Space))
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
        }
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
