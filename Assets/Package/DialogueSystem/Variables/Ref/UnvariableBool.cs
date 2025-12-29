using System;
using UnityEngine;

namespace DialogueSystem.Variables
{
    /// <summary>
    ///  Class for the bool in the narrative system that are constant.
    /// </summary>
    [Serializable]
    public class UnvariableBool : BoolRef
    {
        [SerializeField] private bool _Value;

        public UnvariableBool(bool value) : base()
        {
            _Value = value;
        }

        public override bool GetValue(VariableRefDictionary ctx = null)
        {
            return _Value;
        }
    }
}
