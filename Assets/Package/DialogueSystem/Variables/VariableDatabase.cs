using UnityEngine;

namespace DialogueSystem.Variables
{

    [CreateAssetMenu(fileName = "VariableDatabase", menuName = "Assets/Narrative System/Variable Database")]
    public class VariableDatabase : ScriptableObject
    {
        [SerializeReference]
        public VariableRefDictionary Variables = new();
    }
}
