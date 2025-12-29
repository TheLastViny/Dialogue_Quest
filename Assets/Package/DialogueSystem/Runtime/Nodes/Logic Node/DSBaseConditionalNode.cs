using System;

namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// The base class for the runtime data of all dialogue system conditional nodes.
    /// </summary>
    [Serializable]
    public abstract class DSBaseConditionalNode : DSBaseNode
    {
        public string TrueNextNodeID;
        public string FalseNextNodeID;
    }
}
