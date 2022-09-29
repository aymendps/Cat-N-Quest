using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Quest : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public int stageIndex;
        public NonPlayableCharacter npc;
        public string dialogue;
        public bool sayWhileWaiting;
        public float duration;
    }

    public string questName;
    public int numberOfStages;
    public List<Dialogue> dialogueList = new List<Dialogue>();
    public bool hasQuestReward;

    [HideInInspector]
    public GameObject questReward;

    [HideInInspector]
    public NonPlayableCharacter rewardGiver;

    [HideInInspector]
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

                if (!d.sayWhileWaiting)
                {
                    d.npc.LookAtPlayer();
                    d.npc.SaySentence(d.dialogue);
                    yield return new WaitForSeconds(d.duration);
                }
            }
            AdvanceToNextStage();
        }
        DialogueUI.instance.HideDialogue();
        PlayerCharacterController.player.canMove = true;
    }

    public void StartQuestStage()
    {
        PlayerCharacterController.player.canMove = false;
        List<Dialogue> list = dialogueList.FindAll(d => d.stageIndex == currentStage);
        StartCoroutine(DialogueSequence(list));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Quest script = (Quest)target;

        if (script.hasQuestReward) // if bool is true, show other fields
        {
            script.questReward =
                EditorGUILayout.ObjectField(
                    "Quest Reward",
                    script.questReward,
                    typeof(GameObject),
                    true
                ) as GameObject;

            script.rewardGiver =
                EditorGUILayout.ObjectField(
                    "Reward Giver",
                    script.rewardGiver,
                    typeof(NonPlayableCharacter),
                    true
                ) as NonPlayableCharacter;

            script.rewardPositionOffset = EditorGUILayout.Vector2Field(
                "Reward Position Offset",
                script.rewardPositionOffset
            );
        }
    }
}
#endif
