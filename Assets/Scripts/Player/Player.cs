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

    private bool _isRunning = false;

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        _isRunning = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = new Vector2(_isRunning ? -speedRun : -speed, myRigidbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.velocity = new Vector2(_isRunning ? speedRun : speed, myRigidbody.velocity.y);
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
            myRigidbody.transform.localScale = Vector2.one;

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
