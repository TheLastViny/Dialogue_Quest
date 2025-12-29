using System;
using UnityEngine;

namespace DialogueSystem.Variables
{
    /// <summary>
    ///  Class for the bool in the narrative system that can change.
    /// </summary>
    [Serializable]
    public class VariableBool : BoolRef
    {
        [SerializeField] private string _KeyRef;

        public VariableBool(string keyRef) : base()
        {
            _KeyRef = keyRef;
        }

        public override bool GetValue(VariableRefDictionary ctx)
        {
            if (ctx.GetValue(_KeyRef, out bool value))
            {
                return value;
            }

            return false;
        }
    }
}
