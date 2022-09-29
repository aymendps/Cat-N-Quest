using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class QuestDrop : Interactable
{
    [Header("Quest Drop Configuration")]
    public string dropName;
    public TextMesh textMesh;
    public SpriteRenderer spriteRenderer;
    public float wobbleOffset = 0.2F;
    public float wobbleSpeed = 0.5F;

    private Sequence sequence;

    private void Awake()
    {
        textMesh.text = dropName;
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
        transform.DOKill();
        sequence.Kill();
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            StartCoroutine(Fading.FadeInText(0.3f, textMesh));
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == playerTag && isInteractable)
        {
            StartCoroutine(Fading.FadeOutText(0.3f, textMesh));
        }
    }
}
