using System;
using UnityEditor;

namespace DialogueSystem.Editor
{
    using Variables;

    [Serializable]
    internal class GetString : Base
    {
        private const string BLACKBOARD_VARIABLES = "Assets/Package/DialogueSystem/BlackboardVariablesDatabase.asset";
        private const string GAME_VARIABLES = "Assets/Package/DialogueSystem/GameVariablesDatabase.asset";

        protected VariableDatabase _blackboardVariable;
        protected VariableDatabase _gameVariable;

        public override void OnEnable()
        {
            _blackboardVariable = AssetDatabase.LoadAssetAtPath<VariableDatabase>(BLACKBOARD_VARIABLES);
            _gameVariable = AssetDatabase.LoadAssetAtPath<VariableDatabase>(GAME_VARIABLES);
        }

        
        
    }
}
