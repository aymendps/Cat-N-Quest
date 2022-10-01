using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;

public class PlayerCharacterController : MonoBehaviour
{
    public static PlayerCharacterController player;

    [Header("Sprites Section")]
    public SpriteRenderer angrySymbol;

    [Header("Movement Section")]
    public float speed;
    public bool canMove = false;

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
    private Sequence angrySymbolSequence;

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    public void AddToInventory(string interactableName)
    {
        inventory.Add(interactableName);
        Debug.Log("Added " + interactableName + " to player inventory");
    }

    public void RemoveFromInventory(string interactableName)
    {
        inventory.Remove(interactableName);
        Debug.Log("Removed " + interactableName + " from player inventory");
    }

    public bool isInInventory(string interactableName)
    {
        return inventory.Contains(interactableName);
    }

    private void Awake()
    {
        player = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        direction = Vector2.zero;
        meowIndex = 0;
        angrySymbolSequence = DOTween.Sequence();
        angrySymbolSequence.Append(
            angrySymbol.transform.DOScale(new Vector3(1.2f, 1.92f, 1), 0.5f)
        );
        angrySymbolSequence.Append(angrySymbol.transform.DOScale(new Vector3(1, 1.6f, 1), 0.5f));
        angrySymbolSequence.SetLoops(-1);
        angrySymbolSequence.Pause();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void SetAsleep(bool isAsleep)
    {
        animator.SetBool("InMainMenu", isAsleep);
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
        if (canMove)
        {
            SetAnimatorValues();
            rb.velocity = speed * direction;
        }
    }

    public void TransitionFromMainMenu()
    {
        SetAsleep(false);
        canMove = true;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    public void ShowAngrySymbol()
    {
        angrySymbol.enabled = true;
        angrySymbolSequence.Play();
    }

    public void HideAngrySymbol()
    {
        angrySymbol.enabled = false;
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
        if (!audioSource.isPlaying && canMove)
        {
            audioSource.PlayOneShot(meowSoundEffects[meowIndex % meowSoundEffects.Count], volume);
            meowIndex++;
        }
    }

    public void OnInteract(InputValue value)
    {
        if (canMove)
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
}
