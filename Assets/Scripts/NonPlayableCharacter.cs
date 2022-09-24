using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : Interactable
{
    public enum DefaultNPCView
    {
        front,
        side,
        back
    }

    public enum NPCEmotion
    {
        sad,
        happy
    }

    [System.Serializable]
    public struct NPCView
    {
        public Sprite front;
        public Sprite side;
        public Sprite back;
    }

    [Header("NPC Profile")]
    public string NPCName;
    public NPCEmotion currentEmotion;
    public NPCView sadNPCView;
    public NPCView happyNPCView;

    [Header("NPC Dialogue")]
    public float dialogueVolume;

    [TextArea]
    public string defaultSentence;
    public AudioClip defaultSentenceClip;

    private NPCView activeNPCView;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetActiveNPCView();
        spriteRenderer.sprite = activeNPCView.front;
    }

    public void SetActiveNPCView()
    {
        switch (currentEmotion)
        {
            case NPCEmotion.sad:
                activeNPCView = sadNPCView;
                break;

            case NPCEmotion.happy:
                activeNPCView = happyNPCView;
                break;
        }
    }

    public void SaySentence(string sentence, AudioClip audioClip)
    {
        Debug.Log("NPC " + NPCName + " says: '" + sentence + "'");
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, dialogueVolume);
        }
    }

    public void LookAtPlayer()
    {
        Vector2 vector = PlayerCharacterController.player.transform.position - transform.position;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y) + 1)
        {
            if (vector.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            spriteRenderer.sprite = activeNPCView.side;
        }
        else
        {
            if (vector.y > 0)
            {
                spriteRenderer.sprite = activeNPCView.back;
            }
            else
            {
                spriteRenderer.sprite = activeNPCView.front;
            }
            spriteRenderer.flipX = false;
        }

        Debug.DrawLine(
            transform.position,
            PlayerCharacterController.player.transform.position,
            Color.red,
            3
        );
    }

    public override void Use()
    {
        LookAtPlayer();
        SaySentence(defaultSentence, defaultSentenceClip);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        spriteRenderer.sprite = activeNPCView.front;
    }
}
