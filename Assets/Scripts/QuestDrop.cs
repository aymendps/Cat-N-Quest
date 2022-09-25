using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestDrop : Interactable
{
    [Header("Quest Drop Configuration")]
    public Quest relatedQuest;
    public int importantQuestStage;
    public string dropName;
    public Color interactableColor;
    public float fillColorSpeed = 0.3F;
    public float fadeOutSpeed = 0.2F;
    public float wobbleOffset = 0.2F;
    public float wobbleSpeed = 0.5F;

    private Sequence sequence;
    private SpriteRenderer sr;
    private Color initialColor;

    private void Awake()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y + wobbleOffset, wobbleSpeed));
        sequence.Append(transform.DOMoveY(transform.position.y, wobbleSpeed));
        sequence.SetLoops(-1);
        sequence.Pause();
        sr = GetComponent<SpriteRenderer>();
        initialColor = sr.color;
    }

    private void Start()
    {
        if (isInteractable)
        {
            sequence.Play();
        }
    }

    private void Update()
    {
        if (relatedQuest.GetCurrentStage() == importantQuestStage)
        {
            if (!isInteractable)
            {
                SetIsInteractable(true);
                sequence.Play();
            }
        }
        else
        {
            if (isInteractable)
            {
                SetIsInteractable(false);
            }
        }
    }

    public override void Use()
    {
        Debug.Log("Interacted with " + gameObject.name);
        PlayerCharacterController.player.AddToInventory(dropName);
        relatedQuest.AdvanceToNextStage();
        transform
            .DOScale(Vector3.zero, fadeOutSpeed)
            .OnComplete(() =>
            {
                transform.DOKill();
                sequence.Kill();
                Destroy(gameObject);
            });
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            sr.DOColor(interactableColor, fillColorSpeed);
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            sr.DOColor(initialColor, fillColorSpeed);
        }
    }
}
