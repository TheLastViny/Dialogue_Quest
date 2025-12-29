using System;
using System.Collections.Generic;

namespace NarrativeSystem.Dialogue.Runtime
{
    using UnityEngine;
    using Variables;

    /// <summary>
    /// The serializable data representing a runtime dialogue node with choices.
    /// </summary>
    [Serializable]
    public class DSChoiceDialogueNode : DSBaseDialogueNode
    {
        public List<TextChoice> Choices = new List<TextChoice>();
    }

    /// <summary>
    /// The serializable data representing a text choice.
    /// </summary>
    [Serializable]
    public class TextChoice : INextNode
    {
        [SerializeReference] public StringRef Text;
        [SerializeField] private string _nextNodeID;

        public string NextNodeID
        {
            get => _nextNodeID;
            set => _nextNodeID = value;
        }
    }
}
