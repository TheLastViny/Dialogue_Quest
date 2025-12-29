using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Runtime
{
    using Variables;
    /// <summary>
    /// The serializable data representing a runtime wait for second node.
    /// </summary>
    [Serializable]
    public class DSWaitForSecondNode : DSBaseNode, INextNode
    {
        [SerializeReference] public FloatRef Seconds;
        [SerializeField] private string _nextNodeID;

        public string NextNodeID
        {
            get => _nextNodeID;
            set => _nextNodeID = value;
        }

    }
}
