using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class AnimationController : MonoBehaviour
{
    private MovementController movementController;
    private Animator animator;
    
    void Start()
    {
        movementController = GetComponent<MovementController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetFloat("speed", movementController.GetSpeed());
        if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Running")
            animator.speed = movementController.GetSpeed() / movementController.maxSpeed;
        else animator.speed = 1;

        animator.SetBool("isTumbling", movementController.GetIsTumbling());
        animator.SetBool("begin", GameManager.begin);
    }
}
