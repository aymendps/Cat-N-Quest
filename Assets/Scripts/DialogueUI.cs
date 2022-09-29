using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    public TextMeshProUGUI sentenceText;
    public Image sentenceTextPanel;
    public float dialogueOrthoSize;
    public float fadeInSpeed;
    public float fadeOutSpeed;
    private float initialOrthoSize;

    private void Awake()
    {
        instance = this;
        initialOrthoSize = Camera.main.orthographicSize;
    }

    private void ShowSentence(string sentence)
    {
        sentenceText.text = sentence;
        StartCoroutine(Fading.FadeInText(fadeInSpeed, sentenceText));
        StartCoroutine(Fading.FadeInImage(fadeInSpeed, sentenceTextPanel));
    }

    private void HideSentence()
    {
        StartCoroutine(Fading.FadeOutText(fadeOutSpeed, sentenceText));
        StartCoroutine(Fading.FadeOutImage(fadeOutSpeed, sentenceTextPanel));
    }

    public void ShowDialogue(string sentence)
    {
        Camera.main.DOOrthoSize(dialogueOrthoSize, fadeInSpeed);
        ShowSentence(sentence);
    }

    public void HideDialogue()
    {
        Camera.main.DOOrthoSize(initialOrthoSize, fadeOutSpeed);
        HideSentence();
    }
}
