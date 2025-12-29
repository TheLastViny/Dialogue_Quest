using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// Base class for the float in the narrative system.
    /// </summary>
    [Serializable]
    public abstract class FloatRef
    {
        protected FloatRef() { }

        public abstract float GetValue(VariableRefDictionary ctx);
    }
}
