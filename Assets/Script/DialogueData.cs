using UnityEngine;
[CreateAssetMenu(fileName = "NewDialogueData",menuName ="ScriptableObjects/DialogueData",order = 1)]
[System.Serializable]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 5)]
    public string[] dialogue;
}
