using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestDrop : Interactable
{
    [Header("Quest Drop Configuration")]
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
        sr = GetComponent<SpriteRenderer>();
        initialColor = sr.color;
    }

    private void Start()
    {
        sequence.Play();
    }

    public override void Use()
    {
        PlayerCharacterController.player.AddToInventory(dropName);
        transform.DOScale(Vector3.zero, fadeOutSpeed).OnComplete(() => gameObject.SetActive(false));
        Debug.Log("Interacted with " + gameObject.name);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        sr.DOColor(interactableColor, fillColorSpeed);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        sr.DOColor(initialColor, fillColorSpeed);
    }
}
