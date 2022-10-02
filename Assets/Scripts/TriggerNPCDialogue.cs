using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNPCDialogue : MonoBehaviour
{
    public NonPlayableCharacter NPC;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            NPC.Use();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
