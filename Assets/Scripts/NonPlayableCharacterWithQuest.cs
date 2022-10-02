using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NonPlayableCharacterWithQuest : NonPlayableCharacter
{
    [Header("NPC Quest")]
    public Quest quest;
    public List<int> importantQuestStages = new List<int>();

    public override void Use()
    {
        if (quest.isFinished)
        {
            base.Use();
        }
        else
        {
            bool shouldStartQuest = importantQuestStages.Contains(quest.GetCurrentStage());
            bool hasRequiredDrops = quest.PlayerHasRequiredDrops();

            if (startMovementRoutine)
            {
                navMeshAgent.isStopped = true;
                canMove = false;

                if (movementRoutine != null)
                {
                    StopCoroutine(movementRoutine);
                }
            }

            if (shouldStartQuest && hasRequiredDrops)
            {
                importantQuestStages.Remove(quest.GetCurrentStage());
                quest.StartQuestStage();
            }
            else
            {
                quest.UseWaitingDialogue(this);
            }
        }
    }
}
