using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class DialogueEditor : EditorWindow
    {
        private VisualElement _rightPane;
        private ListView _leftPane;

        [MenuItem("Window/DialogueEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(DialogueEditor));
        }

        public void CreateGUI()
        {
            //��������Ʈ ����Ʈ�� ��Ÿ������ AssetDatabase �Լ��� ����Ͽ� ������Ʈ�� ��� ��������Ʈ�� ã���ʽÿ�.
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
            _leftPane = new ListView();
            splitView.Add(_leftPane);
        

            // Initialize the list view with all sprites' names
            _leftPane.makeItem = () => new Label();
            _leftPane.bindItem = (item, index) => { ((item as Label)!).text = allObjects[index].name; };
            _leftPane.itemsSource = allObjects;

            // React to the user's selection
            _leftPane.selectionChanged += OnDialogueSelectionChange;

            _rightPane = new VisualElement();
            splitView.Add(_rightPane);
        }

        private void UpdateSelectedDialogueView(Dialogue dialogue)
        {
            // Clear all previous content from the pane
            _rightPane.Clear();

            // Add a new Image control and display the sprite
            var dialogueText = new Label(dialogue.name);

            // Add the Image control to the right-hand pane
            _rightPane.Add(dialogueText);

            var contentsView = new ScrollView();
            _rightPane.Add(contentsView);
            foreach (var t in dialogue.contents)
            {
                var content = new Label(t);
                contentsView.Add(content);
            }

            var branchView = new ScrollView(ScrollViewMode.Horizontal);
            _rightPane.Add(branchView);
            foreach (var branch in dialogue.branchs)
            {
                var branch1 = branch;
                var btn = new Button(() => { UpdateSelectedDialogueView(branch1); })
                {
                    text = branch.title
                };
                branchView.Add(btn);
            }        
        }

        private void OnDialogueSelectionChange(IEnumerable<object> selectedItems)
        {
            // Clear all previous content from the pane
            _rightPane.Clear();

            var dialogue = selectedItems.First() as Dialogue;
            if (dialogue is  null)
                return;

            UpdateSelectedDialogueView(dialogue);


        }

        void OnGUI()
        {
       
        }
    }
    ;
}