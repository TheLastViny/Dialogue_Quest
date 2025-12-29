using NarrativeSystem.Utility;
using System;

namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// The serializable data representing a runtime conditional string node.
    /// </summary>
    [Serializable]
    public class DSStringConditionalNode : DSBaseConditionalNode
    {
        public string FirstCondition;
        public string SecondCondition;
        public EConditionalString Condition;
    }
}
