using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed = 1.0f;
    public float transitionSpeed = 1.0f;
    public float transitionOrthoSize = 5;
    private bool isInMainMenu = true;

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

    public void TransitionFromMainMenu()
    {
        transform.DOMove(
            new Vector3(player.position.x, player.position.y, transform.position.z),
            transitionSpeed
        );
        Camera.main
            .DOOrthoSize(transitionOrthoSize, transitionSpeed)
            .OnComplete(() =>
            {
                PlayerCharacterController.player.TransitionFromMainMenu();
                isInMainMenu = false;
            });
    }
}
