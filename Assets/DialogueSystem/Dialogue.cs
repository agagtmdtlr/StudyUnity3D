using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
    public class Dialogue : ScriptableObject
    {
        public string title;
        public string[] contents;
        public Dialogue[] branchs;
    }
}
