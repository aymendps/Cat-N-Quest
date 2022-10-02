using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public struct NoiseClip
    {
        public AudioClip audioClip;
        public float waitBeforePlaying;
    }

    public Image gameTitle;
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI exitButtonText;
    public Image trackerBorder;
    public Image trackerFilled;
    public TextMeshProUGUI trackerPercentage;
    public TextMeshProUGUI controlsText;
    public Quest startingQuest;
    public CameraFollow cameraFollowScript;
    public List<NoiseClip> noiseClips = new List<NoiseClip>();
    public float fadeOutSpeed = 2;
    public float fadeInSpeed = 1;
    public float cameraShakeDuration = 0.2f;
    public float cameraShakeStrength = 3;
    public float transitionSpeed = 1.0f;
    public float transitionOrthoSize = 5;
    private Sequence cameraAnimation;
    private bool started = false;
    private AudioSource audioSource;
    private float initialAsleepPSRateOverTime = 1.2f;
    private bool isShown = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

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
            cameraFollowScript.isInMainMenu = false;
            PlayerCharacterController.player.TransitionFromMainMenu();
        });

        cameraAnimation.Pause();
    }

    private void Update()
    {
        if (startingQuest.GetCurrentStage() > 0 && isShown == false)
        {
            ShowQuestCompletionTracker();
            isShown = true;
        }
    }

    public void HandleExitButton()
    {
        if (started)
            return;

        Application.Quit();
    }

    IEnumerator MainMenuTransition()
    {
        var emission = PlayerCharacterController.player.asleepParticleSystem.emission;

        foreach (NoiseClip clip in noiseClips)
        {
            yield return new WaitForSeconds(clip.waitBeforePlaying);
            audioSource.PlayOneShot(clip.audioClip);
            Camera.main.DOShakePosition(cameraShakeDuration, cameraShakeStrength);
            initialAsleepPSRateOverTime -= 0.25f;
            emission.rateOverTime = initialAsleepPSRateOverTime;
        }

        PlayerCharacterController.player.SetAsleep(false);
        PlayerCharacterController.player.ShowAngrySymbol();
        PlayerCharacterController.player.asleepParticleSystem.gameObject.SetActive(false);

        yield return new WaitForSeconds(cameraShakeDuration * 2);

        cameraAnimation.Play();

        yield return new WaitForSeconds(transitionSpeed * 2);

        PlayerCharacterController.player.HideAngrySymbol();

        StartCoroutine(Fading.FadeInText(fadeInSpeed, controlsText));
    }

    public void ShowQuestCompletionTracker()
    {
        StartCoroutine(Fading.FadeInImage(fadeInSpeed, trackerBorder));
        StartCoroutine(Fading.FadeInImage(fadeInSpeed, trackerFilled));
        StartCoroutine(Fading.FadeInText(fadeInSpeed, trackerPercentage));
    }

    void PlayMainMenuTransition()
    {
        StartCoroutine(MainMenuTransition());
    }

    IEnumerator DisableUIRoutine()
    {
        StartCoroutine(Fading.FadeOutImage(fadeOutSpeed, gameTitle));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, playButtonText));
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, exitButtonText));
        yield return new WaitForSeconds(fadeOutSpeed + 0.1f);
        gameTitle.gameObject.SetActive(false);
        playButtonText.transform.parent.gameObject.SetActive(false);
        exitButtonText.transform.parent.gameObject.SetActive(false);
    }

    void DisableUI()
    {
        StartCoroutine(DisableUIRoutine());
    }

    public void HandlePlayButton()
    {
        if (started)
            return;

        started = true;
        DisableUI();
        PlayMainMenuTransition();
    }
}
