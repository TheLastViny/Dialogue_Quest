using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// The runtime represention of a dialogue system graph.
    /// </summary>
    public class DialogueSystemRuntimeGraph : ScriptableObject
    {
        public DialogueNodeDictionary Nodes = new();
        public string StartNodeID;
    }


}
