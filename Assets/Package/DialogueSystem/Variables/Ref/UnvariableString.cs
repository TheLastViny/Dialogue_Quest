using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the string in the narrative system that are constant.
    /// </summary>
    [Serializable]
    public class UnvariableString : StringRef
    {
        [SerializeField] private string _Value;

        public UnvariableString(string value) : base()
        {

            _Value = value;
        }

        public override string GetValue(VariableRefDictionary ctx = null)
        {
            return _Value;
        }
    }
}
