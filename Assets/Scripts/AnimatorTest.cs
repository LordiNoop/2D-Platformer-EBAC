using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public Animator animator;

    public KeyCode keyToTrigger = KeyCode.A;
    public string triggerToPlay = "Fly";

    private void OnValidate()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToTrigger))
        {
            animator.SetBool(triggerToPlay, !animator.GetBool(triggerToPlay));
        }
    }
}
