using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class QuestStep 
{
    protected abstract void FinishQuestStep();
}

// Concrete QuestStep  class to Prefab
// then add to QuestInfo

// Find Stuff
public class FindItemQuestStep : QuestStep
{
    string[] itemsToFind;
    bool isFoundAllItems;

    protected override void FinishQuestStep()
    {
        throw new System.NotImplementedException();
    }

    void TrackingStep()
    {
        // do something tracking
        // if complete => FinishQuestStep();
        if (isFoundAllItems)
        {
            FinishQuestStep();
        }
            
    }
}

public class FeedQuestStep : QuestStep
{
    bool isFeed;
    string whatIsCanFeed;

    protected override void FinishQuestStep()
    {
        throw new System.NotImplementedException();
    }

    void TrackingStep()
    {
        // do something tracking
        // if complete => FinishQuestStep();
        if(isFeed)
        {
            FinishQuestStep();
        }
    }
}
