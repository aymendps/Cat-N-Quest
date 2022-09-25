using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCEmotion
{
    sad,
    happy
}

public class NonPlayableCharacter : Interactable
{
    public enum View
    {
        front,
        lookRight,
        lookLeft,
        back
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

    public View initialView;

    [Header("NPC Dialogue")]
    public float dialogueVolume;

    [TextArea]
    public string defaultSentence;
    public AudioClip defaultAudioClip;

    private NPCView activeNPCView;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetActiveNPCView();
        SetInitialView();
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

    private void SetInitialView()
    {
        switch (initialView)
        {
            case View.front:
                spriteRenderer.sprite = activeNPCView.front;
                break;

            case View.lookRight:
                spriteRenderer.sprite = activeNPCView.side;
                spriteRenderer.flipX = true;
                break;

            case View.lookLeft:
                spriteRenderer.sprite = activeNPCView.side;
                break;

            case View.back:
                spriteRenderer.sprite = activeNPCView.back;
                break;
        }
    }

    public void ChangeEmotion(NPCEmotion emotion)
    {
        currentEmotion = emotion;
        SetActiveNPCView();
    }

    public void SaySentence(string sentence)
    {
        Debug.Log("NPC " + NPCName + " says: '" + sentence + "'");
    }

    public void SaySentence(string sentence, AudioClip audioClip)
    {
        SaySentence(sentence);

        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, dialogueVolume);
        }
    }

    public void LookAtPlayer()
    {
        Vector2 vector = PlayerCharacterController.player.transform.position - transform.position;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y) + 0.5)
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
        SaySentence(defaultSentence, defaultAudioClip);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == playerTag)
        {
            SetInitialView();
        }
    }
}
