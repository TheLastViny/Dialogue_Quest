using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the string in the narrative system that can change.
    /// </summary>
    [Serializable]
    public class VariableString : StringRef
    {
        [SerializeField] private string _KeyRef;

        public VariableString(string keyRef) : base()
        {
            _KeyRef = keyRef;
        }

        public override string GetValue(VariableRefDictionary ctx)
        {
            if (ctx.GetValue(_KeyRef, out string value))
            {
                return value;
            }

            return string.Empty;
        }
    }
}
