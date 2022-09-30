using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public Image gameTitle;
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI exitButtonText;
    public CameraFollow cameraFollowScript;
    public float fadeOutSpeed;

    public void HandleExitButton()
    {
        Application.Quit();
    }

    IEnumerator DisableUI()
    {
        yield return new WaitForSeconds(fadeOutSpeed + 0.1f);
        gameTitle.gameObject.SetActive(false);
        playButtonText.transform.parent.gameObject.SetActive(false);
        exitButtonText.transform.parent.gameObject.SetActive(false);
    }

    public void HandlePlayButton()
    {
        StartCoroutine(Fading.FadeOutImage(fadeOutSpeed, gameTitle));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, playButtonText));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, exitButtonText));
        StartCoroutine(DisableUI());
        cameraFollowScript.TransitionFromMainMenu();
    }
}
