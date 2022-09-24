using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public float radius;
    public string playerTag = "Player";

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<CircleCollider2D>().radius = radius;
    }

    public abstract void Use();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == playerTag)
        {
            PlayerCharacterController.player.SetCurrentInteractable(this);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        PlayerCharacterController.player.SetCurrentInteractable(null);
    }
}
