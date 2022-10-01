using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed = 1.0f;
    public bool isInMainMenu = true;

    private void Start()
    {
        if (player == null)
            return;
    }

    private void Update()
    {
        if (player == null || isInMainMenu)
            return;

        transform.position = Vector3.Lerp(
            transform.position,
            player.position,
            lerpSpeed * Time.deltaTime
        );
    }
}
