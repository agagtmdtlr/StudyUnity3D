using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProgressionState
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS, // quest step quest step
    CAN_FINISH,
    FINISHED
}


// static information
public class QuestInfo : ScriptableObject
{
    string id;
    string displayName;

    public QuestInfo[] requirement; // ¼±Çà Äù
    public GameObject[] questSteps;

    public QuestReward reward;
}
