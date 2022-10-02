using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    [Header("Development Section")]
    [TextArea]
    public string devNotes;
    public float radius;
    public string playerTag = "Player";

    [SerializeField]
    protected bool isInteractable = true;

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<CircleCollider2D>().radius = radius;
    }

    public void SetIsInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }

    public abstract void Use();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == playerTag && isInteractable)
        {
            PlayerCharacterController.player.SetCurrentInteractable(this);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == playerTag && isInteractable && PlayerCharacterController.player.canMove)
        {
            PlayerCharacterController.player.SetCurrentInteractable(null);
        }
    }
}
