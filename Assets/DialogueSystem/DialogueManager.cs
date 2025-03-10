using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public Text text;
        public Dialogue beginDialogue;
        public Transform branchList;
        public GameObject branchButtonPrefab;
        
        private void Start()
        {
            StartCoroutine(ShowDialogue(beginDialogue));
        }

        IEnumerator ShowDialogue(Dialogue dialogue)
        { 
            int contentIndex = 0;
            var contentLength = dialogue.contents.Length;
            while (contentIndex < contentLength || Input.GetKeyDown(KeyCode.Escape) )
            {
                int characterIndex = 0;
                var content = dialogue.contents[contentIndex];
                
                text.text = "";
                while (characterIndex < content.Length)
                {
                    text.text += content[characterIndex];
                    characterIndex++;
                    yield return new WaitForSeconds(0.1f);
                }
                contentIndex++;
                if (contentIndex == dialogue.contents.Length)
                {
                    yield return StartCoroutine(ChooseBranch(dialogue));
                }
                else
                {
                    yield return StartCoroutine(NextDialogue(contentIndex, dialogue));
                }
            }
            
            Debug.Log("End Show Dialogue");
        }

        bool selectedBranch = false;
        void OnSelectBranch()
        {
            selectedBranch = true;
        }

        IEnumerator ChooseBranch(Dialogue dialogue)
        {
            int newButtonCount = dialogue.branchs.Length - branchList.childCount;

            for (int i = 0; i < newButtonCount; i++)
            {
                Instantiate(branchButtonPrefab, branchList.transform);
            }
                
            selectedBranch = false; 
            var btns = branchList.GetComponentsInChildren<ButtonHandler>();
            for (var i = 0; i < btns.Length; i++)
            { 
                var btn = btns[i];
                if (i < dialogue.branchs.Length)
                {
                    btn.gameObject.SetActive(true);
                    btn.text.text = dialogue.branchs[i].title;
                    btn.button.onClick.RemoveAllListeners();
                    btn.button.onClick.AddListener(OnSelectBranch);
                }
                else
                {
                    btn.gameObject.SetActive(false);
                }
            }

            while (!selectedBranch && !Input.GetKeyDown(KeyCode.Escape))
            {
                yield return null;
            }
        }

        IEnumerator NextDialogue(int contentIndex , Dialogue dialogue)
        {
            
                
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }
    
    }
}
