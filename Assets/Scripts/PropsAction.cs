using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PropsActionType
{
    dropItem,
    showText
}

public class PropsAction : Interactable
{
    public string actionName;
    public PropsActionType actionType;

    [HideInInspector]
    public GameObject itemToDrop;

    [HideInInspector]
    public Vector2 itemPositionOffset;

    [HideInInspector]
    public string textToShow;

    [HideInInspector]
    public bool hasAudio;

    [HideInInspector]
    public AudioClip audioClip;

    [HideInInspector]
    public float volume;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void DropItem()
    {
        Instantiate(
            itemToDrop,
            transform.position + (Vector3)itemPositionOffset,
            Quaternion.identity
        );
    }

    private void ShowText()
    {
        Debug.Log(textToShow);
    }

    public override void Use()
    {
        switch (actionType)
        {
            case PropsActionType.dropItem:
                DropItem();
                break;

            case PropsActionType.showText:
                ShowText();
                break;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.tag == playerTag && isInteractable)
        {
            Debug.Log("Props Interactable: " + actionName);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PropsAction))]
public class PropsActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        PropsAction script = (PropsAction)target;

        switch (script.actionType)
        {
            case PropsActionType.dropItem:
                script.itemToDrop =
                    EditorGUILayout.ObjectField(
                        "Item To Drop",
                        script.itemToDrop,
                        typeof(GameObject),
                        true
                    ) as GameObject;

                script.itemPositionOffset = EditorGUILayout.Vector2Field(
                    "Item Position Offset",
                    script.itemPositionOffset
                );
                break;

            case PropsActionType.showText:

                EditorGUILayout.LabelField("Text");
                script.textToShow = EditorGUILayout.TextArea(
                    script.textToShow,
                    GUILayout.Height(45)
                );
                break;
        }

        script.hasAudio = EditorGUILayout.Toggle("Has Audio", script.hasAudio);

        if (script.hasAudio) // if bool is true, show other fields
        {
            script.audioClip =
                EditorGUILayout.ObjectField("Audio Clip", script.audioClip, typeof(AudioClip), true)
                as AudioClip;
            script.volume = EditorGUILayout.FloatField("Volume", script.volume);
        }
    }
}
#endif