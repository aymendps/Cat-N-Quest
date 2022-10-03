using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when object exit the trigger, put it to the assigned layer and sorting layers
//used in the stair objects for player to travel between layers
public class LayerTrigger : MonoBehaviour
{
    public string layer;
    public string sortingLayer;

    private Coroutine routine = null;
    private Coroutine playerRoutine = null;
    private Coroutine npcRoutine = null;

    IEnumerator ChangeLayer(Collider2D other, string tag)
    {
        other.gameObject.layer = LayerMask.NameToLayer(layer);

        if (other.gameObject.tag != "Player")
        {
            yield return new WaitForSeconds(0.3F);
        }

        other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
        SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.sortingLayerName = sortingLayer;
        }

        if (tag == "NPC")
        {
            npcRoutine = null;
        }
        else if (tag == "Player")
        {
            playerRoutine = null;
        }
        else
        {
            routine = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(layer) && routine == null)
        {
            if (other.gameObject.tag == "NPC")
            {
                if (other.GetType() == typeof(BoxCollider2D))
                {
                    npcRoutine = StartCoroutine(ChangeLayer(other, "NPC"));
                }
            }
            else if (other.gameObject.tag == "Player")
            {
                playerRoutine = StartCoroutine(ChangeLayer(other, "Player"));
            }
            else
            {
                routine = StartCoroutine(ChangeLayer(other, other.tag));
            }
        }
    }
}
