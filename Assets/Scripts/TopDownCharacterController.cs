﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement Section")]
    public float speed;
    public bool canMove;

    [Header("Audio Section")]
    public float volume;
    public List<AudioClip> meowSoundEffects;

    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    private Vector2 direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        direction = Vector2.zero;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void SetAnimatorValues()
    {
        if (direction.x > 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (direction.x < 0)
        {
            animator.SetInteger("Direction", 3);
        }

        if (direction.y > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (direction.y < 0)
        {
            animator.SetInteger("Direction", 0);
        }

        animator.SetBool("IsMoving", direction.magnitude > 0);
    }

    private void UpdatePosition()
    {
        SetAnimatorValues();
        rb.velocity = speed * direction;
    }

    public void OnMove(InputValue value)
    {
        if (canMove)
        {
            direction = value.Get<Vector2>().normalized;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    public void OnMeow(InputValue value)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(
                meowSoundEffects[Random.Range(0, meowSoundEffects.Count)],
                volume
            );
        }
    }

    public void OnInteract(InputValue value) { }
}
