using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditor : EditorWindow
{
    [MenuItem("Tools/My Custom Editor")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("My Custom Editor");
    }
}
