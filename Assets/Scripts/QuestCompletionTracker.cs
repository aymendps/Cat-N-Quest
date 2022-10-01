using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class QuestCompletionTracker : MonoBehaviour
{
    public static QuestCompletionTracker instance;
    public List<Quest> quests = new List<Quest>();
    public Image trackerBorder;
    public Image trackerFilled;
    public TextMeshProUGUI trackerPercentage;

    private float completedQuests = 0;
    private float completionPercentage = 0;

    private void Awake()
    {
        instance = this;
    }

    public void IncrementCompletedQuests()
    {
        completedQuests++;

        if (quests.Count == 0)
        {
            Debug.LogWarning("QuestCompletionPercentage: quests count cannot be equal to 0");
        }
        else
        {
            completionPercentage = completedQuests / quests.Count;
            DOTween.To(
                () => trackerFilled.fillAmount,
                x => trackerFilled.fillAmount = x,
                completionPercentage,
                1
            );
            trackerPercentage.text = Mathf.CeilToInt(completionPercentage * 100) + "%";
        }
    }
}
