using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the int in the narrative system that can change.
    /// </summary>
    [Serializable]
    public class VariableInt : IntRef
    {
        [SerializeField] private string _KeyRef;

        public VariableInt(string keyRef) : base()
        {
            _KeyRef = keyRef;
        }

        public override int GetValue(VariableRefDictionary ctx)
        {
            if (ctx.GetValue(_KeyRef, out int value))
            {
                return value;
            }

            return 0;
        }
    }
}
