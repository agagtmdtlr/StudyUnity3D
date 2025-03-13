using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;

class StatusBar : VisualElement
{
    public StatusBar()
    {
        m_Status = String.Empty;
    }

    string m_Status;
    public string status { get; set; }
}

public class QuestEditor : EditorWindow
{
   
}
