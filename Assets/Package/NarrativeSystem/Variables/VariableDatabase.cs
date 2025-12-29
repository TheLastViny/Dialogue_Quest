using UnityEngine;

namespace NarrativeSystem.Variables
{

    [CreateAssetMenu(fileName = "VariableDatabase", menuName = "Assets/Create/Narrative System/Variable Database")]
    public class VariableDatabase : ScriptableObject
    {
        [SerializeReference]
        public VariableRefDictionary Variables = new();
    }
}
