using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    [Header("Speed Setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    [Header("Animation Setup")]
    public float jumpScaleY = 1.5f;
    public float jumpScaleX = .7f;
    public float animationDuration = .3f;
    public Ease ease = Ease.OutBack;

    [Header("Animation Player")]
    public string boolRun = "Run";
    public Animator animator;
    public float playerSwipeDuration = .1f;

    private bool _isRunning = false;
    private bool _isFacingRight = true;

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
            animator.speed = 2;
        }
        else
        {
            animator.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool(boolRun, true);
            myRigidbody.velocity = new Vector2(_isRunning ? -speedRun : -speed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, playerSwipeDuration);
                _isFacingRight = false;            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool(boolRun, true);
            myRigidbody.velocity = new Vector2(_isRunning ? speedRun : speed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, playerSwipeDuration);
                _isFacingRight = true;
            }
        }
        else
        {
            animator.SetBool(boolRun, false);
        }

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity -= friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity += friction;
        }

    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * forceJump;
            myRigidbody.transform.localScale = Vector3.one;

            if (_isFacingRight)
            {
                jumpScaleX = (Mathf.Sign(jumpScaleX) == 1) ? jumpScaleX : -jumpScaleX;
                myRigidbody.transform.localScale = Vector3.one;
            }
            else
            {
                jumpScaleX = (Mathf.Sign(jumpScaleX) == -1) ? jumpScaleX : -jumpScaleX;
                myRigidbody.transform.localScale = new Vector3(-1, 1, 1);
            }

                DOTween.Kill(myRigidbody.transform);

            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(jumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        myRigidbody.transform.DOScaleX(jumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }
}
