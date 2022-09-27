using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when object exit the trigger, put it to the assigned layer and sorting layers
//used in the stair objects for player to travel between layers
public class LayerTrigger : MonoBehaviour
{
    public string layer;
    public string sortingLayer;

    IEnumerator ChangeLayer(Collider2D other) {

        other.gameObject.layer = LayerMask.NameToLayer(layer);

        if(other.gameObject.tag != "Player") {
            yield return new WaitForSeconds(0.3F);
        }

        other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
        SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.sortingLayerName = sortingLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(ChangeLayer(other));
    }
}
