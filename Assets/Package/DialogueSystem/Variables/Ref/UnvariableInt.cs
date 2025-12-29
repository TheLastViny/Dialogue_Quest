using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the int in the narrative system that are constant.
    /// </summary>
    [Serializable]
    public class UnvariableInt : IntRef
    {
        [SerializeField] private int _Value;

        public UnvariableInt(int value) : base()
        {
            _Value = value;
        }

        public override int GetValue(VariableRefDictionary ctx = null)
        {
            return _Value;
        }
    }
}
