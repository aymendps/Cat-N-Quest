using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishGameExtension : MonoBehaviour
{
    public Quest finishGameQuest;
    public Image blackPanel;
    public Image jamLogo;
    public TextMeshProUGUI creditsText1;
    public TextMeshProUGUI creditsText2;

    private bool isFinished = false;

    private void Update()
    {
        if (finishGameQuest.isFinished && !isFinished)
        {
            StartCoroutine(Fading.FadeInImage(1, blackPanel));
            StartCoroutine(Fading.FadeInImage(1, jamLogo));
            StartCoroutine(Fading.FadeInText(1, creditsText1));
            StartCoroutine(Fading.FadeInText(1, creditsText2));
            isFinished = true;
        }
    }
}
