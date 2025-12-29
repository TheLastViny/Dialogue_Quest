using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <summary>
    /// The serializable data representing a runtime start node.
    /// </summary>
    [Serializable]
    public class DSStartNode : DSBaseNode, INextNode
    {
        [SerializeField] private string _nextNodeID;

        public string NextNodeID
        {
            get => _nextNodeID;
            set => _nextNodeID = value;
        }
    }
}
