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
    public float initialOrthoSize;
    public float dialogueOrthoSize;
    public float fadeInSpeed;
    public float fadeOutSpeed;

    [HideInInspector]
    public bool isShown = false;

    private void Awake()
    {
        instance = this;
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

    public void ShowDialogue(string sentence, bool withOrthoAnimation = true)
    {
        if (withOrthoAnimation)
        {
            Camera.main.DOOrthoSize(dialogueOrthoSize, fadeInSpeed);
        }
        ShowSentence(sentence);
        isShown = true;
    }

    public void HideDialogue()
    {
        Camera.main.DOOrthoSize(initialOrthoSize, fadeOutSpeed);
        HideSentence();
        isShown = false;
    }
}
