using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    public Image gameTitle;
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI exitButtonText;
    public CameraFollow cameraFollowScript;
    public float fadeOutSpeed;
    public float transitionSpeed = 1.0f;
    public float transitionOrthoSize = 5;
    private Sequence cameraAnimation;
    private bool started = false;

    private void Awake()
    {
        cameraAnimation = DOTween.Sequence();

        cameraAnimation.Append(
            Camera.main.transform.DOMove(
                new Vector3(
                    PlayerCharacterController.player.transform.position.x,
                    PlayerCharacterController.player.transform.position.y,
                    Camera.main.transform.position.z
                ),
                transitionSpeed
            )
        );

        cameraAnimation.Join(Camera.main.DOOrthoSize(transitionOrthoSize, transitionSpeed));

        cameraAnimation.OnComplete(() =>
        {
            PlayerCharacterController.player.TransitionFromMainMenu();
            cameraFollowScript.isInMainMenu = false;
        });

        cameraAnimation.Pause();
    }

    public void HandleExitButton()
    {
        if (started)
            return;

        Application.Quit();
    }

    public void AnimateCamera()
    {
        cameraAnimation.Play();
    }

    IEnumerator DisableUI()
    {
        StartCoroutine(Fading.FadeOutImage(fadeOutSpeed, gameTitle));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, playButtonText));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, exitButtonText));
        yield return new WaitForSeconds(fadeOutSpeed + 0.1f);
        gameTitle.gameObject.SetActive(false);
        playButtonText.transform.parent.gameObject.SetActive(false);
        exitButtonText.transform.parent.gameObject.SetActive(false);
    }

    public void HandlePlayButton()
    {
        if (started)
            return;

        started = true;
        StartCoroutine(DisableUI());
        AnimateCamera();
    }
}
