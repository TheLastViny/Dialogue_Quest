using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// The serializable data representing a runtime wait for input node.
    /// </summary>
    [Serializable]
    public class DSWaitForInputNode : DSBaseNode, INextNode
    {
        [SerializeField] private string _nextNodeID;

        public string NextNodeID
        {
            get => _nextNodeID;
            set => _nextNodeID = value;
        }
    }
}
