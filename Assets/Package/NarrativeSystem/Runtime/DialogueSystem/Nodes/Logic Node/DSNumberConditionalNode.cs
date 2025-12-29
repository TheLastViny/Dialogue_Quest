using NarrativeSystem.Utility;
using System;
using UnityEngine;

namespace NarrativeSystem.Dialogue.Runtime
{
    /// <summary>
    /// The base class for the runtime data of all dialogue system conditional number nodes.
    /// </summary>
    [Serializable]
    public class DSNumberConditionalNode : DSBaseConditionalNode
    {
        public float FirstCondition;
        public float SecondCondition;
        public EConditionalNumbers Condition;
    }
}
