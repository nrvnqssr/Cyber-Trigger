using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public PlayerMotor playerMotor;
    public float currentSpeed;

    private void Awake()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        currentSpeed = new Vector3(playerMotor.characterController.velocity.x, 0, playerMotor.characterController.velocity.z).magnitude;
        if (animator != null)
            animator.SetFloat("Speed", currentSpeed);
    }
}