using System;
using UnityEngine;

namespace NarrativeSystem.Variables
{
    /// <summary>
    ///  Class for the float in the narrative system that are constant.
    /// </summary>
    [Serializable]
    public class UnvariableFloat : FloatRef
    {
        [SerializeField] private float _Value;

        public UnvariableFloat(float value) : base()
        {
            _Value = value;
        }

        public override float GetValue(VariableRefDictionary ctx = null)
        {
            return _Value;
        }
    }
}
