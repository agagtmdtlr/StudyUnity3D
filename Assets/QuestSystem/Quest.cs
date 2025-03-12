using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    QuestInfo info;
    EProgressionState state;

    int currentStepIndex;
    QuestStepState[] questStepStates;

    // call from quest manager
    void QuestStateChange(EProgressionState state)
    {

    }

}
