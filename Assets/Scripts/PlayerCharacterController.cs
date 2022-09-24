using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public static PlayerCharacterController player;

    [Header("Movement Section")]
    public float speed;
    public bool canMove;

    [Header("Audio Section")]
    public float volume;
    public List<AudioClip> meowSoundEffects;

    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private int meowIndex;
    private Vector2 direction;
    private Interactable currentInteractable;
    private List<string> inventory = new List<string>();

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    public void AddToInventory(string interactableName)
    {
        inventory.Add(interactableName);
        Debug.Log("Added " + interactableName + " to player inventory");
    }

    private void Awake()
    {
        player = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        direction = Vector2.zero;
        meowIndex = 0;
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
            audioSource.PlayOneShot(meowSoundEffects[meowIndex % meowSoundEffects.Count], volume);
            meowIndex++;
        }
    }

    public void OnInteract(InputValue value)
    {
        if (currentInteractable)
        {
            currentInteractable.Use();
        }
        else
        {
            Debug.Log("No current interactable");
        }
    }
}
