using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishermanQuestExtension : MonoBehaviour
{
    public NonPlayableCharacter nearFisherman;
    public int stageToMoveNearFisherman;
    private Quest quest;

    private void Awake()
    {
        quest = GetComponent<Quest>();
    }

    private void Update()
    {
        if (quest.GetCurrentStage() == stageToMoveNearFisherman)
        {
            nearFisherman.StartMovementRoutine();
        }
    }
}
