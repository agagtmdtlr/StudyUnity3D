using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;

public class DialogueEditor : EditorWindow
{
    private VisualElement m_RightPane;

    [MenuItem("Window/DialogueEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DialogueEditor));
    }

    public void CreateGUI()
    {
        //스프라이트 리스트를 나타내려면 AssetDatabase 함수를 사용하여 프로젝트의 모든 스프라이트를 찾으십시오.
        // Get a list of all sprites in the project
        var allObjectGuids = AssetDatabase.FindAssets("t:Dialogue");
        var allObjects = new List<Dialogue>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Dialogue>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the view to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);

        // A TwoPaneSplitView always needs exactly two child elements
        var leftPane = new ListView();
        splitView.Add(leftPane);
        

        // Initialize the list view with all sprites' names
        leftPane.makeItem = () => new Label();
        leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        leftPane.itemsSource = allObjects;

        // React to the user's selection
        leftPane.onSelectionChange += OnDialogueSelectionChange;

        m_RightPane = new VisualElement();
        splitView.Add(m_RightPane);
    }

    private void UpdateSelectedDialogueView(Dialogue dialogue)
    {
        // Clear all previous content from the pane
        m_RightPane.Clear();

        // Add a new Image control and display the sprite
        var dialogueText = new Label(dialogue._name);

        // Add the Image control to the right-hand pane
        m_RightPane.Add(dialogueText);

        var contentsView = new ScrollView();
        m_RightPane.Add(contentsView);
        for (int i = 0; i < dialogue._contents.Length; i++)
        {
            var content = new Label(dialogue._contents[i]);
            contentsView.Add(content);
        }

        var branchView = new ScrollView(ScrollViewMode.Horizontal);
        m_RightPane.Add(branchView);
        for (int i = 0; i < dialogue._branchs.Length; i++)
        {
            var branch = dialogue._branchs[i];
            var btn = new Button(() => { UpdateSelectedDialogueView(branch); });
            btn.text = branch._name;
            branchView.Add(btn);
            //branchView.Add(new Label(branch._name));
        }        
    }

    private void OnDialogueSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane
        m_RightPane.Clear();

        var dialogue = selectedItems.First() as Dialogue;
        if (dialogue == null)
            return;

        UpdateSelectedDialogueView(dialogue);


    }

    void OnGUI()
    {
       
    }
}
;