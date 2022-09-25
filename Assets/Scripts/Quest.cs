using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public int stageIndex;
        public NonPlayableCharacter npc;
        public string dialogue;
        public float duration;
    }

    public string questName;
    public GameObject questReward;
    public int numberOfStages;
    public List<Dialogue> dialogueList = new List<Dialogue>();

    [HideInInspector]
    public bool isFinished = false;

    private int currentStage = 0;

    private void OnValidate()
    {
        if (questReward != null)
        {
            QuestDrop drop = questReward.GetComponent<QuestDrop>();
            if (drop == null)
            {
                questReward = null;
                Debug.LogWarning(
                    "Removed the selected quest reward because it did not have a 'Quest Drop' component"
                );
            }
        }
    }

    public void FinishQuest()
    {
        Debug.Log("Finished Quest: " + questName);
        isFinished = true;
    }

    public int GetCurrentStage()
    {
        return currentStage;
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
            if (d.npc == npc && d.stageIndex == currentStage)
            {
                npc.LookAtPlayer();
                npc.SaySentence(d.dialogue);
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
                d.npc.LookAtPlayer();
                d.npc.SaySentence(d.dialogue);
                yield return new WaitForSeconds(d.duration);
            }
            AdvanceToNextStage();
        }
        PlayerCharacterController.player.canMove = true;
    }

    public void StartQuestStage()
    {
        PlayerCharacterController.player.canMove = false;
        List<Dialogue> list = dialogueList.FindAll(d => d.stageIndex == currentStage);
        StartCoroutine(DialogueSequence(list));
    }
}
