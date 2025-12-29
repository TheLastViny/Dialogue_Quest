using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the float in the narrative system that can change.
    /// </summary>
    [Serializable]
    public class VariableFloat : FloatRef
    {
        [SerializeField] private string _KeyRef;

        public VariableFloat(string keyRef) : base()
        {
            _KeyRef = keyRef;
        }

        public override float GetValue(VariableRefDictionary ctx)
        {
            if (ctx.GetValue(_KeyRef, out float value))
            {
                return value;
            }

            return 0;
        }
    }
}
