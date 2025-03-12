using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    void OnRequirementsChange()
    {
        // update require
        // and get new quest
    }

    void OnStartQuest(string id)
    {
        // begin quest
        // init first quest step
    }
    void OnAdvanceQuest(string id) 
    {
        // move on to next quest step
        // if no more quest steps, then move in to can_finish state
    }
    void OnFinishQuest(string id) 
    { 
        // claim any reward
    }
}
