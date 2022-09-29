using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestDrop : Interactable
{
    [Header("Quest Drop Configuration")]
    public string dropName;
    public SpriteRenderer spriteRenderer;
    public float wobbleOffset = 0.2F;
    public float wobbleSpeed = 0.5F;

    private Sequence sequence;
    private string pickUpSentence = "E- Pick up";

    private void Awake()
    {
        sequence = DOTween.Sequence();
        sequence.Append(
            spriteRenderer.transform.DOMoveY(
                spriteRenderer.transform.position.y + wobbleOffset,
                wobbleSpeed
            )
        );
        sequence.Append(
            spriteRenderer.transform.DOMoveY(spriteRenderer.transform.position.y, wobbleSpeed)
        );
        sequence.SetLoops(-1);
        sequence.Pause();
    }

    private void Start()
    {
        if (isInteractable)
        {
            sequence.Play();
        }
    }

    public override void Use()
    {
        // Debug.Log("Interacted with " + gameObject.name);
        PlayerCharacterController.player.AddToInventory(dropName);
        sequence.Kill();
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            DialogueUI.instance.ShowDialogue(dropName, pickUpSentence);
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            DialogueUI.instance.HideDialogue();
        }
    }
}
