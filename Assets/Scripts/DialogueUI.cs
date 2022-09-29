using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;
    public float dialogueOrthoSize;
    public float fadeInSpeed;
    public float fadeOutSpeed;
    private float initialOrthoSize;

    private void Awake()
    {
        instance = this;
        initialOrthoSize = Camera.main.orthographicSize;
    }

    public void ShowName(string actorName)
    {
        nameText.text = actorName + ':';
        StartCoroutine(Fading.FadeInText(fadeInSpeed, nameText));
    }

    public void HideName()
    {
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, nameText));
    }

    public void ShowSentence(string sentence)
    {
        sentenceText.text = sentence;
        StartCoroutine(Fading.FadeInText(fadeInSpeed, sentenceText));
    }

    public void HideSentence()
    {
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, sentenceText));
    }

    public void ShowDialogue(string actorName, string sentence)
    {
        Camera.main.DOOrthoSize(dialogueOrthoSize, fadeInSpeed);
        ShowName(actorName);
        ShowSentence(sentence);
    }

    public void HideDialogue()
    {
        Camera.main.DOOrthoSize(initialOrthoSize, fadeOutSpeed);
        HideName();
        HideSentence();
    }
}
