using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Quest : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public int stageIndex;
        public NonPlayableCharacter npc;

        [TextArea]
        public string dialogue;
        public bool sayWhileWaiting;
        public bool shouldGiveItem;
        public string itemToGive;
        public bool shouldTakeItem;
        public string itemToTake;
    }

    [System.Serializable]
    public struct QuestStageDrop
    {
        public string questDropName;
        public int questStage;
    }

    public string questName;
    public int numberOfStages;
    public List<Dialogue> dialogueList = new List<Dialogue>();
    public List<QuestStageDrop> requiredQuestDrops = new List<QuestStageDrop>();
    public bool hasQuestReward;
    public GameObject questReward;
    public NonPlayableCharacter rewardGiver;
    public Vector2 rewardPositionOffset;

    [HideInInspector]
    public bool isFinished = false;

    private int currentStage = 0;

    public void FinishQuest()
    {
        if (!isFinished)
        {
            Debug.Log("Finished Quest: " + questName);

            foreach (Dialogue d in dialogueList)
            {
                d.npc.ChangeEmotion(NPCEmotion.happy);
                if (!d.npc.startMovementRoutine)
                {
                    d.npc.StartMovementRoutine();
                }
            }

            if (hasQuestReward)
            {
                Instantiate(
                    questReward,
                    rewardGiver.transform.position + (Vector3)rewardPositionOffset,
                    Quaternion.identity
                );
            }

            QuestCompletionTracker.instance.IncrementCompletedQuests();

            isFinished = true;
        }
        else
        {
            Debug.Log("Tried to finish a quest that is already done: '" + questName + "'");
        }
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public bool PlayerHasRequiredDrops()
    {
        List<QuestStageDrop> requiredDrops = requiredQuestDrops.FindAll(
            d => d.questStage == currentStage
        );

        foreach (QuestStageDrop q in requiredDrops)
        {
            if (!PlayerCharacterController.player.isInInventory(q.questDropName))
            {
                return false;
            }
        }
        return true;
    }

    public void AdvanceToNextStage()
    {
        currentStage++;
        Debug.Log(
            "Quest: '"
                + questName
                + "' advanced from stage "
                + (currentStage - 1)
                + " to stage "
                + currentStage
        );
        if (currentStage == numberOfStages)
        {
            FinishQuest();
        }
    }

    public void UseWaitingDialogue(NonPlayableCharacter npc)
    {
        foreach (Dialogue d in dialogueList)
        {
            if (d.npc == npc && d.stageIndex == currentStage && d.sayWhileWaiting)
            {
                npc.LookAtPlayer();
                npc.SaySentence(d.dialogue, false);
                break;
            }
        }
    }

    public IEnumerator DialogueSequence(List<Dialogue> list)
    {
        if (list.Count != 0)
        {
            foreach (Dialogue d in list)
            {
                if (currentStage == numberOfStages - 1)
                {
                    d.npc.ChangeEmotion(NPCEmotion.happy);
                }

                if (!d.sayWhileWaiting)
                {
                    d.npc.LookAtPlayer();
                    d.npc.SaySentence(d.dialogue);

                    while (!PlayerCharacterController.player.skip)
                    {
                        yield return null;
                    }

                    PlayerCharacterController.player.skip = false;

                    if (d.shouldGiveItem)
                    {
                        PlayerCharacterController.player.AddToInventory(d.itemToGive);
                    }

                    if (d.shouldTakeItem)
                    {
                        PlayerCharacterController.player.RemoveFromInventory(d.itemToTake);
                    }
                }
            }
            AdvanceToNextStage();
        }
        DialogueUI.instance.HideDialogue();
        PlayerCharacterController.player.canMove = true;
    }

    public void StartQuestStage()
    {
        PlayerCharacterController.player.SetCannotMove();
        List<Dialogue> list = dialogueList.FindAll(d => d.stageIndex == currentStage);
        StartCoroutine(DialogueSequence(list));
    }
}
