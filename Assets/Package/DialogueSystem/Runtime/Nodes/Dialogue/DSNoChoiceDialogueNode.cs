using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <summary>
    /// The serializable data representing a runtime dialogue node without choices.
    /// </summary>
    [Serializable]
    public class DSNoChoiceDialogueNode : DSBaseDialogueNode, INextNode
    {
        [SerializeField] private string _nextNodeID;

        public string NextNodeID
        {
            get => _nextNodeID;
            set => _nextNodeID = value;
        }
    }
}
