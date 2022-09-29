using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;

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
    public TextMesh textMesh;
    public NPCEmotion currentEmotion;
    public NPCView sadNPCView;
    public NPCView happyNPCView;

    public View initialView;

    [Header("NPC Dialogue")]
    public float dialogueVolume;

    [TextArea]
    public string defaultSentence;
    public AudioClip defaultAudioClip;

    [Header("NPC AI")]
    public NavMeshAgent navMeshAgent;
    public bool startMovementRoutine;
    public List<Vector2> positionsInOrder = new List<Vector2>();
    public float timeBetweenPositions;

    private NPCView activeNPCView;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private bool canMove = false;
    private Coroutine movementRoutine = null;
    private int routinePositionIndex = 0;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh.text = NPCName;
        SetActiveNPCView();
        SetInitialView();
    }

    public void Start()
    {
        if (startMovementRoutine)
        {
            StartMovementRoutine();
        }
    }

    public void Update()
    {
        UpdateMovementRoutine();
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
        // Debug.Log("NPC " + NPCName + " says: '" + sentence + "'");
        DialogueUI.instance.ShowDialogue(NPCName, sentence);
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

    public void LookAtDirection(Vector2 velocity)
    {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y) + 0.5)
        {
            if (velocity.x > 0)
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
            if (velocity.y > 0)
            {
                spriteRenderer.sprite = activeNPCView.back;
            }
            else
            {
                spriteRenderer.sprite = activeNPCView.front;
            }
            spriteRenderer.flipX = false;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)velocity, Color.blue);
    }

    IEnumerator MovementRoutine()
    {
        yield return new WaitForSeconds(timeBetweenPositions);

        if (canMove)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination((Vector3)positionsInOrder[routinePositionIndex]);
        }
    }

    public void StartMovementRoutine()
    {
        if (positionsInOrder.Count != 0 && navMeshAgent != null)
        {
            startMovementRoutine = true;
            canMove = true;
            movementRoutine = StartCoroutine(MovementRoutine());
        }
    }

    public void UpdateMovementRoutine()
    {
        if (navMeshAgent)
        {
            transform.position = navMeshAgent.transform.position;
        }

        if (canMove && startMovementRoutine)
        {
            if (navMeshAgent.velocity != Vector3.zero)
            {
                LookAtDirection(navMeshAgent.velocity);
            }

            if (
                Vector2.Distance(transform.position, positionsInOrder[routinePositionIndex]) <= 0.2F
            )
            {
                routinePositionIndex++;
                if (routinePositionIndex >= positionsInOrder.Count)
                {
                    routinePositionIndex = 0;
                }

                if (movementRoutine != null)
                {
                    StopCoroutine(movementRoutine);
                }

                movementRoutine = StartCoroutine(MovementRoutine());
            }
        }
    }

    public override void Use()
    {
        if (startMovementRoutine)
        {
            navMeshAgent.isStopped = true;
            canMove = false;

            if (movementRoutine != null)
            {
                StopCoroutine(movementRoutine);
            }
        }

        LookAtPlayer();
        SaySentence(defaultSentence, defaultAudioClip);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.tag == playerTag)
        {
            StartCoroutine(Fading.FadeInText(0.3f, textMesh));
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == playerTag)
        {
            SetInitialView();

            StartCoroutine(Fading.FadeOutText(0.3f, textMesh));

            DialogueUI.instance.HideDialogue();

            if (startMovementRoutine)
            {
                canMove = true;

                if (movementRoutine != null)
                {
                    StopCoroutine(movementRoutine);
                }

                movementRoutine = StartCoroutine(MovementRoutine());
            }
        }
    }
}
