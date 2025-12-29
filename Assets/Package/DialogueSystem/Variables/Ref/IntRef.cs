using System;

namespace DialogueSystem.Variables
{
    /// <summary>
    /// Base class for the int text in the narrative system.
    /// </summary>
    [Serializable]
    public abstract class IntRef
    {
        protected IntRef() { }

        public abstract int GetValue(VariableRefDictionary ctx);
    }
}
